namespace Depalyzer.Cli.Helpers;

using System.Collections.Immutable;
using Depalyzer.Cli.Exceptions;
using Microsoft.Build.Construction;
using Spectre.Console;

public static class SolutionHelper
{
    public static string GetProjectOrSolution(string fileOrDirectory)
    {
        if (Directory.Exists(fileOrDirectory))
        {
            var possibleSolutionPath = FindSolutionPaths(fileOrDirectory);

            switch (possibleSolutionPath.Length)
            {
                case > 1:
                    throw new RenderableException(
                        new Markup($"""[red]Can't determine which solution to scan as there are more than one solution file in directory "{possibleSolutionPath}"[/]"""));
                case 1:
                    return possibleSolutionPath[0];
            }

            var possibleProjectPath = FindProjectPaths(fileOrDirectory);

            return possibleProjectPath.Length switch
            {
                0 => throw new RenderableException(new Markup($"""[red]No solution or project files found in directory "{possibleSolutionPath}"[/]""")),
                1 => possibleProjectPath[0],
                _ => throw new RenderableException(
                    new Markup($"""[red]Can't determine which project to scan as there are more than one project file in directory "{possibleSolutionPath}"[/]""")),
            };
        }

        if (!File.Exists(fileOrDirectory))
        {
            throw new RenderableException(new Markup($"""[red]Unable to find file "{fileOrDirectory}"[/]"""));
        }

        return Path.GetFullPath(fileOrDirectory);
    }

    public static SolutionFile GetSolutionFileForDirectoryOrFile(string fileOrDirectory)
    {
        fileOrDirectory = GetProjectOrSolution(fileOrDirectory);
        var fullPath = Path.GetFullPath(fileOrDirectory);

        return SolutionFile.Parse(fullPath);
    }

    public static ImmutableArray<ProjectInSolution> GetProjectsForDirectoryOrFile(string fileOrDirectory)
    {
        var solutionFile = GetSolutionFileForDirectoryOrFile(fileOrDirectory);

        return solutionFile.ProjectsInOrder.ToImmutableArray();
    }

    public static string AbsoluteDirectory(this ProjectInSolution project)
    {
        return Directory.GetParent(project.AbsolutePath)!.FullName;
    }

    private static string[] FindSolutionPaths(string fileOrDirectory) => Directory.GetFiles(fileOrDirectory, "*.sln", SearchOption.TopDirectoryOnly);

    private static string[] FindProjectPaths(string fileOrDirectory) => Directory.GetFiles(fileOrDirectory, "*.*proj", SearchOption.TopDirectoryOnly)
        .Where(path => !path.EndsWith(".xproj", StringComparison.OrdinalIgnoreCase))
        .Select(Path.GetFullPath)
        .ToArray();

    public static List<ProjectInSolution> FilteredProjectsInOrder(this SolutionFile solutionFile) =>
        solutionFile.ProjectsInOrder.Where(project => project.ProjectName != "Solution Items").ToList();
}