using System.ComponentModel;
using System.Text;
using Depalyzer.Cli;
using Depalyzer.Cli.Exceptions;
using Depalyzer.Cli.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;

Console.OutputEncoding = Encoding.UTF8;

var app = new CommandApp<DependencyAnalyzerCommand>();

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

namespace Depalyzer.Cli
{
    public sealed class DependencyAnalyzerCommand : AsyncCommand<DependencyAnalyzerCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [Description("The directory of the solution to scan.")]
            [CommandArgument(0, "[directory]")]
            public string Directory { get; init; } = System.IO.Directory.GetCurrentDirectory();

            [Description("Defines if extra data should be displayed.")]
            [CommandOption("--verbose")]
            [DefaultValue(false)]
            public bool IsVerbose { get; init; }

            [Description("Defines if the projects in the solution should be displayed when referenced in other projects of the solution.")]
            [CommandOption("--show-projects-in-transitive")]
            [DefaultValue(false)]
            public bool IsShowingProjectsInTransitive { get; init; }

            [Description("Defines if a simplified view should be displayed, where only the root and leaves of the dependency graph should be shown.")]
            [CommandOption("-s|--simplified")]
            [DefaultValue(false)]
            public bool IsSimplifiedView { get; init; }

            [Description("Show a monkey emoji as a spinner.")]
            [CommandOption("--monkey")]
            [DefaultValue(false)]
            public bool IsUsingMonkeySpinner { get; init; }

            public string FullPathDirectory => Path.GetFullPath(this.Directory);
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var scanner = new VulnerabilityScanner(settings);

            await scanner.Scan();

            return 0;
        }
    }
}