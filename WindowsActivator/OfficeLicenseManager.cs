using System.Diagnostics;
using System.IO;
using Spectre.Console;
using WindowsActivator.Licenses;

namespace WindowsActivator
{
    public static class OfficeLicenseManager
    {
        public static void InstallOfficeLicense(License license, StatusContext ctx)
        {
            string officePath = GetOfficePath();
            if (string.IsNullOrEmpty(officePath))
            {
                if (Worker.DevMode) AnsiConsole.MarkupLine("[bold red]Office installation not found. Ensure Office is installed.[/]");
                return;
            }

            if (Worker.DevMode) AnsiConsole.MarkupLine($"[bold yellow]Detected Office Path: {officePath}[/]");

            ctx.Status("Step 1: Converting retail license to volume license...");
            if (Worker.DevMode) AnsiConsole.MarkupLine("[bold blue]Starting license conversion...[/]");
            ConvertRetailToVolume(license.Name);

            ctx.Status("Step 2: Setting KMS server...");
            if (Worker.DevMode) AnsiConsole.MarkupLine("[bold blue]Setting KMS server and port...[/]");
            SetOfficeKmsServer("kms.luax.xyz", 1688);

            ctx.Status("Step 3: Uninstalling previous Office key...");
            UninstallOfficeLicense();

            ctx.Status("Step 4: Installing new Office GVLK...");
            if (Worker.DevMode) AnsiConsole.MarkupLine($"[bold blue]Installing GVLK for {license.Name} with key: {license.ProductKey}[/]");
            InstallOfficeGvlk(license.ProductKey);

            ctx.Status("Step 5: Activating Office...");
            if (Worker.DevMode) AnsiConsole.MarkupLine("[bold blue]Activating Office license...[/]");
            ActivateOffice();

            if (Worker.DevMode)
            {
                AnsiConsole.MarkupLine($"[bold green]Office license {license.Name} installed and activated successfully![/]");
                WaitForEnter();
            }
        }

        public static void UninstallOfficeLicense()
        {
            string officePath = GetOfficePath();
            if (string.IsNullOrEmpty(officePath))
            {
                AnsiConsole.MarkupLine("[bold red]Office installation not found. Ensure Office is installed.[/]");
                return;
            }

            // Retrieve the last 5 characters of the installed Office key
            string lastFive = GetInstalledOfficeKeyLastFive(officePath);
            if (string.IsNullOrEmpty(lastFive))
            {
                AnsiConsole.MarkupLine("[bold red]No installed Office product key detected.[/]");
                return;
            }

            if (Worker.DevMode) AnsiConsole.MarkupLine($"[bold yellow]Removing Office license with last 5 characters: {lastFive}[/]");

            // Uninstall the product key and clear the KMS host
            ExecuteCommand($"cscript \"{officePath}ospp.vbs\" /unpkey:{lastFive} >nul");
            ExecuteCommand($"cscript \"{officePath}ospp.vbs\" /remhst");

            AnsiConsole.MarkupLine("[bold yellow]Office license and KMS server settings removed successfully.[/]");

            if (Worker.DevMode) WaitForEnter();
        }

        private static string GetInstalledOfficeKeyLastFive(string officePath)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", $"/c cscript \"{officePath}ospp.vbs\" /dstatus")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                foreach (string line in output.Split('\n'))
                {
                    if (line.Contains("Last 5 characters of installed product key"))
                    {
                        return line.Split(':')[1].Trim();
                    }
                }
            }

            return string.Empty;
        }

        private static void ConvertRetailToVolume(string version)
        {
            string officePath = GetOfficePath();
            string volumeLicensePattern;

            if (Worker.DevMode) AnsiConsole.MarkupLine($"[bold yellow]Converting retail to volume for version: {version}[/]");

            if (version.Contains("2024"))
            {
                volumeLicensePattern = "ProPlus2024VL_KMS";
            }
            else if (version.Contains("2021"))
            {
                volumeLicensePattern = "ProPlus2021VL_KMS";
            }
            else if (version.Contains("2019"))
            {
                volumeLicensePattern = "ProPlus2019VL";
            }
            else if (version.Contains("2016"))
            {
                volumeLicensePattern = "proplusvl_kms";
            }
            else
            {
                throw new ArgumentException($"Unsupported Office version: {version}");
            }

            string command = $"cd /d \"{officePath}\" && for /f %x in ('dir /b \"..\\root\\Licenses16\\{volumeLicensePattern}*.xrm-ms\"') do cscript ospp.vbs /inslic:\"..\\root\\Licenses16\\%x\"";
    
            if (Worker.DevMode) AnsiConsole.MarkupLine($"[bold blue]Executing license conversion command: {command}[/]");
    
            ExecuteCommand(command);
        }

        private static void InstallOfficeGvlk(string productKey)
        {
            string officePath = GetOfficePath();
            if (Worker.DevMode) AnsiConsole.MarkupLine($"[bold blue]Installing GVLK key: {productKey} at path: {officePath}[/]");
            ExecuteCommand($"cscript \"{officePath}ospp.vbs\" /inpkey:{productKey}");
        }

        private static void SetOfficeKmsServer(string kmsHost, int kmsPort)
        {
            string officePath = GetOfficePath();
            if (Worker.DevMode) AnsiConsole.MarkupLine($"[bold blue]Setting KMS host to {kmsHost} and port to {kmsPort}[/]");
            ExecuteCommand($"cscript \"{officePath}ospp.vbs\" /sethst:{kmsHost}");
            ExecuteCommand($"cscript \"{officePath}ospp.vbs\" /setprt:{kmsPort}");
        }

        private static void ActivateOffice()
        {
            string officePath = GetOfficePath();
            if (Worker.DevMode) AnsiConsole.MarkupLine("[bold blue]Attempting to activate Office...[/]");
            ExecuteCommand($"cscript \"{officePath}ospp.vbs\" /act");
        }

        private static string GetOfficePath()
        {
            string[] paths = {
                @"C:\Program Files\Microsoft Office\Office16\",
                @"C:\Program Files (x86)\Microsoft Office\Office16\",
                @"C:\Program Files\Microsoft Office\Office15\",
                @"C:\Program Files (x86)\Microsoft Office\Office15\"
            };

            foreach (var path in paths)
            {
                if (File.Exists(Path.Combine(path, "ospp.vbs")))
                {
                    if (Worker.DevMode) AnsiConsole.MarkupLine($"[bold green]Found ospp.vbs at path: {path}[/]");
                    return path;
                }
            }

            throw new FileNotFoundException("ospp.vbs not found in common Office installation paths.");
        }

        private static void ExecuteCommand(string command)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", $"/c {command}")
            {
                UseShellExecute = false,
                CreateNoWindow = !Worker.DevMode,
                WindowStyle = Worker.DevMode ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
                RedirectStandardOutput = Worker.DevMode,
                RedirectStandardError = Worker.DevMode
            };

            using (var process = new Process { StartInfo = processInfo })
            {
                process.Start();

                if (Worker.DevMode)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (!string.IsNullOrEmpty(output))
                    {
                        AnsiConsole.MarkupLine($"[bold green]{output}[/]");
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        AnsiConsole.MarkupLine($"[bold red]{error}[/]");
                    }
                }

                process.WaitForExit();

                if (process.ExitCode != 0 && Worker.DevMode)
                {
                    AnsiConsole.MarkupLine("[bold red]An error occurred while executing the command.[/]");
                }
            }
        }

        private static void WaitForEnter()
        {
            AnsiConsole.MarkupLine("[grey italic]Press Enter to continue...[/]");
            Console.ReadLine();
        }
    }
}
