using Spectre.Console;
using WindowsActivator.Licenses;

namespace WindowsActivator;

public class Worker
{
    public static bool DevMode { get; set; } = false;

    public static void InstallLicense(License license)
    {
        if (license == null)
        {
            Menu.DrawMainScreen();
            return;
        }

        bool skipUpgrade = false;
        if (license.Category == "Windows" && !string.IsNullOrEmpty(license.TargetEdition))
        {
            skipUpgrade = AnsiConsole.Confirm("[grey italic]Do you want to skip the Windows edition upgrade?[/]");
        }

        AnsiConsole.MarkupLine($"[bold green]You are about to install the {license.Category} license:[/] [italic]{license.Name}[/]");
        
        if (!AnsiConsole.Confirm("[grey italic]Do you want to proceed with the installation?[/]"))
        {
            Menu.DrawMainScreen();
            return;
        }

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots2)
            .SpinnerStyle(Style.Parse("green bold"))
            .Start("Starting installation...", ctx =>
            {
                if (license.Category == "Windows")
                {
                    WindowsLicenseManager.InstallWindowsLicense(license, ctx, skipUpgrade);
                }
                else if (license.Category == "Office")
                {
                    OfficeLicenseManager.InstallOfficeLicense(license, ctx);
                }
            });

        AnsiConsole.MarkupLine($"[bold blue]{license.Category} license installation completed:[/] {license.Name}");
        Thread.Sleep(2000);
        Menu.DrawMainScreen();
    }

    public static void UninstallLicense()
    {
        var licenseType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold green]Select the type of license to uninstall:[/]")
                .AddChoices("Windows License", "Office License", "Back to Main Menu"));

        if (licenseType == "Back to Main Menu")
        {
            Menu.DrawMainScreen();
            return;
        }

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(Style.Parse("red bold"))
            .Start("Uninstalling license...", ctx =>
            {
                if (licenseType == "Windows License")
                {
                    WindowsLicenseManager.UninstallWindowsLicense();
                }
                else if (licenseType == "Office License")
                {
                    OfficeLicenseManager.UninstallOfficeLicense();
                }
            });

        Thread.Sleep(2000);
        Menu.DrawMainScreen();
    }
}
