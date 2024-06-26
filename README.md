<h1 align="center">
    <img alt="Depalyzer logo" src="header.svg">
</h1>

Depalyzer analyzes a .NET solution's dependencies for possible vulnerabilities. Behind the scenes it'll run `dotnet` CLI commands.

## Prerequisites
* .NET SDK 7+
* dotnet CLI

## Usage
To use depalyzer use it as a CLI.
The `[directory]` optional parameter defines the directory of the solution to analyze. It'll default to the current directory.

```
depalyzer [directory]
```

## Parameters
| Parameter name                | Shorthand | Default value | Description                                                                                                               |
|-------------------------------|-----------|---------------|---------------------------------------------------------------------------------------------------------------------------|
| --verbose                     |           | false         | Defines if extra data should be displayed.                                                                                |
| --show-projects-in-transitive |           | false         | Defines if the projects in the solution should be displayed when referenced in other projects of the solution.            |
| --simplified                  | -s        | false         | Defines if a simplified view should be displayed, where only the root and leaves of the dependency graph should be shown. |
| --version                     | -v        |               | Displays the current version of Depalyzer.                                                                                |
| --help                        | -h        |               | Displays help in the terminal.                                                                                            |
