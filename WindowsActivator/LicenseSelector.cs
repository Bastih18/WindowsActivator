using Spectre.Console;
using WindowsActivator.Licenses;

namespace WindowsActivator
{
    public class LicenseSelector
    {
        public static License SelectLicense()
        {
            var mainChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Select a license category:[/]")
                    .AddChoices("Windows", "Office", "Back to Main Menu"));

            return mainChoice switch
            {
                "Windows" => SelectWindowsLicense(),
                "Office" => SelectOfficeLicense(),
                _ => License.NoLicense // Represents "Back to Main Menu"
            };
        }

        private static License SelectWindowsLicense()
        {
            var windowsChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Select a Windows license type:[/]")
                    .AddChoices("Windows 10/11 Pro", "Windows 10/11 Enterprise", "Server", "Enterprise LTSC", "Back"));

            if (windowsChoice == "Back")
            {
                return SelectLicense();
            }
            return windowsChoice switch
            {
                "Windows 10/11 Pro" => WindowsLicense.WinPro,
                "Windows 10/11 Enterprise" => WindowsLicense.WinEnterprise,
                "Server" => SelectWindowsServerLicense(),
                "Enterprise LTSC" => SelectWindowsEnterpriseLTSC(),
                _ => License.NoLicense // Go back to main menu
            };
        }

        private static License SelectWindowsServerLicense()
        {
            var serverVersion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Select a Windows Server version:[/]")
                    .AddChoices("2022", "2019", "2016", "Back"));

            if (serverVersion == "Back") return SelectLicense(); // Go back to Windows menu

            var serverEdition = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[bold green]Select an edition for Windows Server {serverVersion}:[/]")
                    .AddChoices("Standard", "Datacenter", serverVersion == "2022" ? "Datacenter: Azure Edition" : "Essentials", "Back"));

            if (serverEdition == "Back") return SelectWindowsServerLicense(); // Go back to server version menu

            return (serverVersion, serverEdition) switch
            {
                ("2022", "Standard") => WindowsLicense.Server2022Standard,
                ("2022", "Datacenter") => WindowsLicense.Server2022Datacenter,
                ("2022", "Datacenter: Azure Edition") => WindowsLicense.Server2022Azure,
                ("2019", "Standard") => WindowsLicense.Server2019Standard,
                ("2019", "Datacenter") => WindowsLicense.Server2019Datacenter,
                ("2019", "Essentials") => WindowsLicense.Server2019Essentials,
                ("2016", "Standard") => WindowsLicense.Server2016Standard,
                ("2016", "Datacenter") => WindowsLicense.Server2016Datacenter,
                ("2016", "Essentials") => WindowsLicense.Server2016Essentials,
                _ => License.NoLicense
            };
        }

        private static License SelectWindowsEnterpriseLTSC()
        {
            var ltscVersion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Select a Windows Enterprise LTSC version:[/]")
                    .AddChoices("2024", "2021", "2019", "2016", "2015", "Back"));

            if (ltscVersion == "Back") return SelectLicense();
            return ltscVersion switch
            {
                "2024" => WindowsLicense.EnterpriseLTSC2024,
                "2021" => WindowsLicense.EnterpriseLTSC2021,
                "2019" => WindowsLicense.EnterpriseLTSC2019,
                "2016" => WindowsLicense.EnterpriseLTSC2016,
                "2015" => WindowsLicense.EnterpriseLTSC2015,
                _ => License.NoLicense
            };
        }

        private static License SelectOfficeLicense()
        {
            var officeVersion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Select an Office version:[/]")
                    .AddChoices("2024", "2021", "2019", "2016", "Back"));

            if (officeVersion == "Back") return SelectLicense();

            var officeEdition = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[bold green]Select an edition for Office {officeVersion}:[/]")
                    .AddChoices("Standard", "Professional Plus", "Back"));

            if (officeEdition == "Back") return SelectOfficeLicense();
            return (officeVersion, officeEdition) switch
            {
                ("2024", "Standard") => OfficeLicense.Office2024Standard,
                ("2024", "Professional Plus") => OfficeLicense.Office2024ProPlus,
                ("2021", "Standard") => OfficeLicense.Office2021Standard,
                ("2021", "Professional Plus") => OfficeLicense.Office2021ProPlus,
                ("2019", "Standard") => OfficeLicense.Office2019Standard,
                ("2019", "Professional Plus") => OfficeLicense.Office2019ProPlus,
                ("2016", "Standard") => OfficeLicense.Office2016Standard,
                ("2016", "Professional Plus") => OfficeLicense.Office2016ProPlus,
                _ => License.NoLicense
            };
        }
    }
}
