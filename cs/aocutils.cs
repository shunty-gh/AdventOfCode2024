using Spectre.Console;

namespace Shunty.AoC;

public static class AocUtils
{
    /// <summary>
    /// Output the given title text to the console in random ANSI
    /// colouring (if the shell supports it)
    /// </summary>
    public static void PrintTitle(string title)
    {
        var colours = new string[] { "red", "orange1", "yellow", "green", "blue", "purple", "violet" };
        var rnd = new Random();
        foreach(var c in $" * * * {title} * * * ")
        {
            var colour = colours[rnd.Next(colours.Length)];
            AnsiConsole.Markup($"[bold {colour}]{c}[/]");
        }
        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Try and find the input file for the given day.
    /// Search in the current directory, then in a './input/
    /// sub-directory i(f one exists) and then repeat in each
    /// parent directory tree for a few levels until we find
    /// an appropriate input file. If we reach the max number
    /// of levels then return an empty string.
    /// <para>
    /// This should make it easier to run the program from a number of different starting
    /// directories. eg from the solution (.sln) directory, from the project directory, or
    /// from the bin directory etc.
    /// </para>
    /// </summary>
    /// <param name="day">The day number of the input file we are looking for</param>
    /// <param name="suffix">An optional suffix to add to the file name to search for, eg: 'test'</param>
    /// <returns>The full file name of the input file, if found, otherwise an empty string</returns>
    public static string FindInputFile(int day, string suffix = "")
    {
        // An arbitrary number of levels to search up the directory tree
        const int maxParentLevels = 6;

        var dstart = Directory.GetCurrentDirectory();
        var dir = dstart;
        var dayfile = string.IsNullOrWhiteSpace(suffix)
            ? $"day{day:D2}-input"
            : $"day{day:D2}-input-{suffix}";
        int parentLevel = 0;
        while (parentLevel <= maxParentLevels)
        {
            // Look in this directory
            var fn = Path.Combine(dir, dayfile);
            if (File.Exists(fn))
            {
                return fn;
            }

            // Look in ./input sub-directory
            fn = Path.Combine(dir, "input", dayfile);
            if (File.Exists(fn))
            {
                return fn;
            }

            // Otherwise go up a directory
            var dinfo = Directory.GetParent(dir);
            if (dinfo == null)
            {
                break;
            }
            parentLevel++;
            dir = dinfo.FullName;
        }
        // Not found
        return "";
    }

}

public interface AocDaySolver
{
    int DayNumber { get; }
    Task Solve();
}

public static class AocDaySolverExtensions
{
    /// <summary>
    /// These extensions require the `Spectre.Console` Nuget package
    /// </summary>

    public static void ShowDayHeader(this AocDaySolver solver)
    {
        AnsiConsole.MarkupLine($"[bold]Day {solver.DayNumber}[/]");
    }

    public static void ShowDayResult<T>(this AocDaySolver _, int part, T solution, string suffix = "")
    {
        AnsiConsole.MarkupLine($"  [bold]Part {part}:[/] {solution?.ToString() ?? "<Unknown>"} {(string.IsNullOrWhiteSpace(suffix) ? "" : suffix)}");
    }

    public static void ShowDayResults<T>(this AocDaySolver _, T solution1, T solution2)
    {
        AnsiConsole.MarkupLine($"  [bold]Part 1:[/] {solution1?.ToString() ?? "<Unknown>"}");
        AnsiConsole.MarkupLine($"  [bold]Part 2:[/] {solution2?.ToString() ?? "<Unknown>"}");
    }

    public static void ShowDayResults<T1, T2>(this AocDaySolver _, T1 solution1, T2 solution2)
    {
        AnsiConsole.MarkupLine($"  [bold]Part 1:[/] {solution1?.ToString() ?? "<Unknown>"}");
        AnsiConsole.MarkupLine($"  [bold]Part 2:[/] {solution2?.ToString() ?? "<Unknown>"}");
    }
}
