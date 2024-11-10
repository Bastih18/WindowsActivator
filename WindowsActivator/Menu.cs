using Spectre.Console;
using WindowsActivator.Licenses;

namespace WindowsActivator;

public abstract class Menu
{
    public static void DrawMainScreen()
    {
        AnsiConsole.Clear();
        Draw.DrawLogo();

        AnsiConsole.MarkupLine("[bold underline green]Welcome to WinActivator[/]");
        
        // Provide a selection menu
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold green]Select an action to proceed:[/]")
                .PageSize(4)
                .AddChoices(new[] {
                    "Install License",
                    "View Available Licenses",
                    "Remove License",
                    "Exit"
                }));

        // Handle the selected option
        switch (option)
        {
            case "Install License":
                License licenseAdd = LicenseSelector.SelectLicense();
                if (licenseAdd.Equals(License.NoLicense)) DrawMainScreen();
                Worker.InstallLicense(licenseAdd);
                break;

            case "View Available Licenses":
                AvailLicenses.GetAvailableLicenses();
                break;

            case "Remove License":
                Worker.UninstallLicense();
                break;

            case "Exit":
                AnsiConsole.MarkupLine("[bold yellow]Exiting...[/]");
                AnsiConsole.MarkupLine("[bold yellow]Thank you for using [/][blue]Win[/][green]Activator![/]");
                break;
        }
    }
}
