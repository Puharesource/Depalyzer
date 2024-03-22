namespace Depalyzer.Cli.Helpers;

using Spectre.Console;
using Spectre.Console.Rendering;

public static class AnsiConsoleHelper
{
    public static void WriteLine(IRenderable renderable)
    {
        AnsiConsole.Write(renderable);
        AnsiConsole.WriteLine();
    }
}