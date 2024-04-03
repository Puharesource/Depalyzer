using System.Text;
using Depalyzer.Cli.Commands;
using Depalyzer.Cli.Exceptions;
using Depalyzer.Cli.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;

Console.OutputEncoding = Encoding.UTF8;

var app = new CommandApp<DepalyzerCommand>();

app.Configure(
    config =>
    {
        config.SetExceptionHandler(
            ex =>
            {
                if (ex is RenderableException renderableException)
                {
                    AnsiConsoleHelper.WriteLine(renderableException.Renderable);
                }

                AnsiConsole.WriteException(ex);
            });
    });

return await app.RunAsync(args);
