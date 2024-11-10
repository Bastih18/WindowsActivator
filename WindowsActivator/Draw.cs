using Spectre.Console;

namespace WindowsActivator;

public class Draw
{
    public static void DrawLogo(bool clear = true)
    {
        if (clear)
            AnsiConsole.Clear();
        
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[blue]888       888 d8b             [/][green]         d8888          888    d8b                   888                    [/]");
        AnsiConsole.MarkupLine("[blue]888   o   888 Y8P             [/][green]        d88888          888    Y8P                   888                    [/]");
        AnsiConsole.MarkupLine("[blue]888  d8b  888                 [/][green]       d88P888          888                          888                    [/]");
        AnsiConsole.MarkupLine("[blue]888 d888b 888 888 88888b.     [/][green]      d88P 888  .d8888b 888888 888 888  888  8888b.  888888 .d88b.  888d888 [/]");
        AnsiConsole.MarkupLine("[blue]888d88888b888 888 888 \"88b   [/][green]      d88P  888 d88P\"    888    888 888  888     \"88b 888   d88\"\"88b 888P\"   [/]");
        AnsiConsole.MarkupLine("[blue]88888P Y88888 888 888  888    [/][green]    d88P   888 888      888    888 Y88  88P .d888888 888   888  888 888     [/]");
        AnsiConsole.MarkupLine("[blue]8888P   Y8888 888 888  888    [/][green]   d8888888888 Y88b.    Y88b.  888  Y8bd8P  888  888 Y88b. Y88..88P 888     [/]");
        AnsiConsole.MarkupLine("[blue]888P     Y888 888 888  888    [/][green]  d88P     888  \"Y8888P  \"Y888 888   Y88P   \"Y888888  \"Y888 \"Y88P\"  888     [/]");
        AnsiConsole.WriteLine();

        if (Worker.DevMode)
        {
            AnsiConsole.MarkupLine("[aqua]DEV MODE[/]");
        }
    }
}