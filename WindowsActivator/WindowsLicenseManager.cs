using System.Diagnostics;
using System.Threading;
using Spectre.Console;
using WindowsActivator.Licenses;

namespace WindowsActivator
{
    public static class WindowsLicenseManager
    {
        public static void InstallWindowsLicense(License license, StatusContext ctx, bool skipUpgrade)
        {
            ctx.Status("Step 1: Unregistering any previous product key...");
            if (Worker.DevMode) AnsiConsole.MarkupLine("[bold blue]Unregistering any previous product key...[/]");
            UnregisterOldProductKey();

            ctx.Status("Step 2: Preparing Windows license files...");
            if (Worker.DevMode) AnsiConsole.MarkupLine("[bold blue]Preparing Windows license files...[/]");
            Thread.Sleep(1500);

            ctx.Status("Step 3: Configuring system settings for Windows license...");
            if (Worker.DevMode) AnsiConsole.MarkupLine("[bold blue]Configuring system settings for Windows license...[/]");
            Thread.Sleep(1500);

            if (!skipUpgrade && !string.IsNullOrEmpty(license.TargetEdition))
            {
                ctx.Status("Upgrading Windows edition...");
                if (Worker.DevMode) AnsiConsole.MarkupLine($"[bold blue]Upgrading Windows edition to {license.TargetEdition}...[/]");
                ChangeWindowsEdition(license.TargetEdition, license.ProductKey);
            }

            ctx.Status("Step 4: Applying new Windows license...");
            if (Worker.DevMode) AnsiConsole.MarkupLine($"[bold blue]Applying KMS key: {license.ProductKey}[/]");
            ApplyKmsKey(license.ProductKey);

            AnsiConsole.MarkupLine($"[bold green]Windows license {license.Name} installed successfully![/]");
            if (Worker.DevMode) WaitForEnter();
        }

        public static void UninstallWindowsLicense()
        {
            if (Worker.DevMode) AnsiConsole.MarkupLine("[bold blue]Uninstalling Windows license...[/]");
            UnregisterOldProductKey();
            ClearKmsServer();
            AnsiConsole.MarkupLine("[bold yellow]Windows license uninstalled successfully![/]");
            if (Worker.DevMode) WaitForEnter();
        }

        private static void UnregisterOldProductKey()
        {
            if (Worker.DevMode) AnsiConsole.MarkupLine("[bold blue]Executing: slmgr /upk and slmgr /cpky to remove any existing keys[/]");
            ExecuteCommand("slmgr /upk");
            ExecuteCommand("slmgr /cpky");
            AnsiConsole.MarkupLine("[bold yellow]Previous product key unregistered successfully.[/]");
        }

        private static void ClearKmsServer()
        {
            if (Worker.DevMode) AnsiConsole.MarkupLine("[bold blue]Clearing any existing KMS server with: slmgr /ckms[/]");
            ExecuteCommand("slmgr /ckms");
        }

        private static void ChangeWindowsEdition(string targetEdition, string productKey)
        {
            string command = $"dism /online /Set-Edition:{targetEdition} /AcceptEula /ProductKey:{productKey}";
            if (Worker.DevMode) AnsiConsole.MarkupLine($"[bold blue]Changing Windows edition with command: {command}[/]");
            ExecuteCommand(command);
            AnsiConsole.MarkupLine($"[bold green]Windows edition changed to {targetEdition}. A restart is required to complete the upgrade.[/]");
        }

        private static void ApplyKmsKey(string productKey)
        {
            if (Worker.DevMode) AnsiConsole.MarkupLine("[bold blue]Setting KMS server to kms.luax.xyz and activating with the KMS key...[/]");
            ExecuteCommand($"slmgr /ipk {productKey}");
            ExecuteCommand("slmgr /skms kms.luax.xyz");
            ExecuteCommand("slmgr /ato");
        }

        private static void ExecuteCommand(string command)
        {
            if (command.StartsWith("slmgr") && !Worker.DevMode)
            {
                command += " //b"; // Add `//b` to `slmgr` commands to suppress pop-ups if not in Dev Mode
            }

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
