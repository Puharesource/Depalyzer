namespace Depalyzer.Cli.Process;

using Spectre.Console;

public static class ConsoleHelper
{
    public static async Task<string> RunCommand(string workingDirectory, string fileName, string arguments, bool monkey = false) => await AnsiConsole.Status()
        .Spinner(monkey ? Spinner.Known.Monkey : Spinner.Known.Dots)
        .StartAsync<string>(
            $"Running command: [yellow]{Markup.Escape(fileName)} {Markup.Escape(arguments)}[/]",
            async _ =>
            {
                using var process = new CommandProcess(workingDirectory, fileName, arguments);
                var output = await process.Run();

                return output;
            });
}