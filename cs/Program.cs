/*
* ***
* The main program class for running one or more solutions for
* the [Advent Of Code 2024](https://adventofcode.com/2024) puzzles.
*
* The solution classes need to implement the `AocDaySolver` interface
* and, in this project, are saved in the `./days` directory.
*
* The timing of each solution is a bit academic as the AnsiConsole code
* with spinners and colouring etc takes longer than normal, simple stdout
* writing. But it doesn't really matter.
* ***
*/


if (args.Contains("-h") || args.Contains("--help") || args.Contains("--?"))
{
    ShowHelp();
    return;
}

var daySolutions = new Dictionary<int, Func<AocDaySolver>>()
{
    {  1, () => new Day01() },
    {  2, () => new Day02() },
    {  3, () => new Day03() },
    {  4, () => new Day04() },
    {  5, () => new Day05() },
    {  6, () => new Day06() },
    {  7, () => new Day07() },
    {  8, () => new Day08() },
    {  9, () => new Day09() },
    { 10, () => new Day10() },
    // { 11, () => new Day11() },
    // { 12, () => new Day12() },
    // { 13, () => new Day13() },
    // { 14, () => new Day14() },
    // { 15, () => new Day15() },
    // { 16, () => new Day16() },
    // { 17, () => new Day17() },
    // { 18, () => new Day18() },
    // { 19, () => new Day19() },
    // { 20, () => new Day20() },
    // { 21, () => new Day21() },
    // { 22, () => new Day22() },
    // { 23, () => new Day23() },
    // { 24, () => new Day24() },
    // { 25, () => new Day25() },
}.ToFrozenDictionary();


Console.WriteLine();
AocUtils.PrintTitle("Advent of Code 2024 (C#)");
Console.WriteLine();

var days = AocUtils.GetDaysRequested(args);
if (days.Count == 0)
{
    days = daySolutions.Keys
        .OrderBy(k => k)
        .ToList();
}

var sw = new System.Diagnostics.Stopwatch();
await AnsiConsole.Status()
    .StartAsync($"Running solution{(days.Count == 1 ? "" : "s")}...", async ctx =>
    {
        sw.Start();
        var flipflop = false;
        ctx.Spinner(Spinner.Known.Christmas);

        foreach (var day in days)
        {
            var start = sw.ElapsedMilliseconds;
            if (!daySolutions.ContainsKey(day))
            {
                Console.WriteLine($"No solution for day {day}");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                continue;
            }

            ctx.Spinner(flipflop ? Spinner.Known.Star : Spinner.Known.Christmas);
            flipflop = !flipflop;
            //await Task.Delay(1000);  // Optional - So we get time to see the christmassy spinners :-)

            var solver = daySolutions[day]();
            solver.ShowDayHeader();
            await solver.Solve();
            AnsiConsole.MarkupLine($"  [blue]Day {solver.DayNumber} completed in [yellow]{sw.ElapsedMilliseconds - start}[/]ms[/]");
            Console.WriteLine();
        }
    });

static void ShowHelp()
{
    AocUtils.PrintTitle("Advent of Code 2024 (C#)");
    Console.WriteLine();
    Console.WriteLine("Usage: dotnet run                   Run the solution for the current day, if available, or all days if Christmas is over");
    Console.WriteLine("       dotnet run [day1] [day2]...  Run the solution for the given day number(s)");
    Console.WriteLine("       dotnet run --all             Run the solution for all available days");
    Console.WriteLine("       dotnet run --?               Show this help text");
    Console.WriteLine();
    Console.WriteLine("If using the compiled executable then replace 'dotnet run' with");
    Console.WriteLine("the executable name 'aoc2024' (& filepath if necessary).");
    Console.WriteLine();
    Console.WriteLine("eg: dotnet run 1 2 14");
    Console.WriteLine("    aoc2024 2 3 6");
    Console.WriteLine();
}
