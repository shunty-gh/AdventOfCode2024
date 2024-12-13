# Advent Of Code 2024

## C# solutions

Some cross platform solutions written in C# / .Net 9 for the [Advent of Code 2024](https://adventofcode.com/2024) puzzles.

### Pre-requisites

* [.Net 9 installed](https://dotnet.microsoft.com/download/dotnet)
* (Optional) [Visual Studio Code](https://code.visualstudio.com/)
* (Optional) Any other C# code editor, notepad, IDE, environment, cloud, whatever you so desire
* (Optional) If using VS Code it would be a good idea to install the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) extension

### Run

```
cd <path-to-this-cs-directory>
dotnet run
```

or add `--project ./cs` to the line if running from the repository root directory (ie the parent to this directory).


By default, with no command line arguments, it will run the solution for the current day if we are within 1-25 December range or it will run all solutions if we are not within the puzzle date range. To specify one or more specific days just list them on the command line separated by spaces. Obviously(?) it will only run days where the solution has been written. eg:

```
dotnet run 1 11 19
```

To run all available solutions use the `--all` flag on the command line. ie:

```
dotnet run --all
```

To show brief usage information / help text use:

```
dotnet run --?
```

### Native Compilation

To build a Release version using all the ahead of time, platform specific compilation and trimming goodness use the command:

```
dotnet publish -c Release
```
This will produce a (possibly) much faster, platform native executable. By default it will be output to the `<repo>/cs/bin/Release/net9.0/<platform>/publish/` directory.
eg `./bin/Release/net9.0/win-x64/publish/aoc2024.exe` on Windows and `./bin/Release/net9.0/linux-x64/publish/aoc2024` on Linux.

Run this compiled exectuable with the same parameters as listed in the [Run](#run) section, above