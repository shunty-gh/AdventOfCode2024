using System.Security.Cryptography.X509Certificates;
using Spectre.Console;

namespace Shunty.AoC;

public static class AocUtils
{
    public const int AoCYear = 2024;

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
    /// sub-directory (if one exists) and then repeat in each
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

    /// <summary>
    /// Parse the command line args for any day numbers required
    /// </summary>
    public static List<int> GetDaysRequested(string[] args)
    {
        var today = DateTime.Today;
        var curentDay = (today.Year == AoCYear && today.Month == 12 && today.Day <= 25) ? today.Day : 0;
        var runAll = args.Any(a => a == "-a" || a == "--all");
        if (runAll)
            return [];

        // Get any day numbers off the command line
        var daysRequested = args
            .Select(a => int.TryParse(a, out var ia) ? ia : 0)
            .Where(i => i > 0 && i <= 25)
            .OrderBy(x => x)
            .Distinct()
            .ToList();
        // Just today if we haven't specified anything else
        if (daysRequested.Count == 0 && curentDay > 0)
        {
            daysRequested.Add(curentDay);
        }
        return daysRequested;
    }

    // Grid and map type helpers

    /// <summary>
    /// From index Y in the source list, return the character at index position X.
    /// If X or Y is out of bounds return the `notFound` character rather than throwing an exception.
    /// </summary>
    public static char CharAt(this IReadOnlyList<string> source, (int X, int Y) pt, char notFound = ' ') =>
        CharAt(source, pt.X, pt.Y, notFound);

    /// <summary>
    /// From the string at index Y in the source list, return the character at index position X.
    /// If X or Y is out of bounds return the `notFound` character rather than throwing an exception.
    /// </summary>
    public static char CharAt(this IReadOnlyList<string> source, int X, int Y, char notFound = ' ') =>
        (X >= 0 && X < source[0].Length && Y >= 0 && Y < source.Count) ? source[Y][X] : notFound;

    /// <summary>
    /// From the string at index point.Y in the source list, return the character at index position point.X.
    /// If X or Y is out of bounds return the `notFound` character rather than throwing an exception.
    /// </summary>
    public static char CharAt(this IReadOnlyList<string> source, Point2d point, char notFound = ' ') =>
        (point.X >= 0 && point.X < source[0].Length && point.Y >= 0 && point.Y < source.Count) ? source[point.Y][point.X] : notFound;

}

public record Point2d(int X, int Y)
{
    public static readonly List<Point2d> NS = [new(0,-1), new(0,1)];
    public static readonly List<Point2d> NESW = [new(0,-1), new(1,0), new(0,1), new(-1,0)];
    public static readonly List<Point2d> Diagonals = [new(1,-1), new(1,1), new(-1,1), new(-1,-1)];
}

public record Point3d(int X, int Y, int Z);

public interface AocDaySolver
{
    int DayNumber { get; }
    string Title { get; }
    Task Solve();
}

public static class AocDaySolverExtensions
{
    /// <summary>
    /// These extensions require the `Spectre.Console` Nuget package
    /// </summary>

    public static void ShowDayHeader(this AocDaySolver solver)
    {
        AnsiConsole.MarkupLine($"[bold]Day {solver.DayNumber}[/] - {solver.Title} (https://adventofcode.com/{AocUtils.AoCYear}/day/{solver.DayNumber})");
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
