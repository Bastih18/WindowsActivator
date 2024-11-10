using Spectre.Console;

namespace WindowsActivator;

public class AvailLicenses
{
    public static void GetAvailableLicenses()
    {
        Draw.DrawLogo();
        // Prompt for selecting Windows or Office licenses
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold green]Select a category to view available licenses:[/]")
                .AddChoices("Windows", "Office", "Back to Main Menu"));

        // Show corresponding licenses based on selection
        if (choice == "Windows")
        {
            ShowWindowsLicenses();
        }
        else if (choice == "Office")
        {
            ShowOfficeLicenses();
        } else if (choice == "Back to Main Menu")
        {
            Menu.DrawMainScreen();
        }
    }

    private static void ShowWindowsLicenses()
    {
        AnsiConsole.Clear();
        // Table for Windows licenses
        var table = new Table()
            .Title("[bold green]Available Windows Licenses[/]")
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Blue)
            .AddColumn("[bold blue]License Name[/]");

        // Add each license as a row in the table
        table.AddRow("Windows 11/10 Pro");
        table.AddRow("Windows 11/10 Enterprise");
        table.AddRow("Windows Server 2022 (Standard, Datacenter, Datacenter: Azure Edition)");
        table.AddRow("Windows Server 2019 (Standard, Datacenter, Essentials)");
        table.AddRow("Windows Server 2016 (Standard, Datacenter, Essentials)");
        table.AddRow("Windows 11 Enterprise LTSC 2024");
        table.AddRow("Windows 10 Enterprise LTSC 2021");
        table.AddRow("Windows 10 Enterprise LTSC 2019");
        table.AddRow("Windows 10 Enterprise LTSB 2016");
        table.AddRow("Windows 10 Enterprise LTSB 2015");

        // Render the table
        AnsiConsole.Write(table);

        // Prompt to continue back to the main menu
        AnsiConsole.MarkupLine("[grey italic]Press Enter to go back to the main menu...[/]");
        
        var input = AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());

        if (input == "-1")
        {
            Worker.DevMode = !Worker.DevMode;
            if (Worker.DevMode)
                AnsiConsole.MarkupLine("[bold cyan]Dev Mode has been[/] [bold green]Enabled![/]");
            else
                AnsiConsole.MarkupLine("[bold cyan]Dev Mode has been[/] [bold red]Disabled![/]");
            Thread.Sleep(2000);
        }

        // Return to the main menu
        GetAvailableLicenses();
    }

    private static void ShowOfficeLicenses()
    {
        AnsiConsole.Clear();
        // Table for Office licenses
        var table = new Table()
            .Title("[bold orange1]Available Office Licenses[/]")
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Orange1)
            .AddColumn("[bold orange1]License Name[/]");

        // Add each license as a row in the table
        table.AddRow("Office LTSC Professional Plus 2024");
        table.AddRow("Office LTSC Standard 2024");
        table.AddRow("Office LTSC Professional Plus 2021");
        table.AddRow("Office LTSC Standard 2021");
        table.AddRow("Office Professional Plus 2019");
        table.AddRow("Office Standard 2019");
        table.AddRow("Office Professional Plus 2016");
        table.AddRow("Office Standard 2016");

        // Render the table
        AnsiConsole.Write(table);

        // Prompt to continue back to the main menu
        AnsiConsole.MarkupLine("[grey italic]Press Enter to go back to the main menu...[/]");
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());

        // Return to the main menu
        GetAvailableLicenses();
    }
}
