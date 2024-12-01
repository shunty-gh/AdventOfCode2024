using System.Collections.Immutable;

namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/1 - Historian Hysteria

public class Day01 : AocDaySolver
{
    public int DayNumber => 1;

    public async Task Solve()
    {
        var lines = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber)))
            .Select(x => {
                var parts = x.Split("  ");
                return (int.Parse(parts[0]), int.Parse(parts[^1]));
            })
            .ToImmutableList();
        var left1 = lines.Select(x => x.Item1).OrderBy(x => x).ToImmutableList();
        var right1 = lines.Select(x => x.Item2).OrderBy(x => x).ToImmutableList();

        var part1 = left1.Zip(right1, (x, y) => Math.Abs(x - y)).Sum();

        this.ShowDayHeader();
        this.ShowDayResult(1, part1);

        // Part 2
        // "Calculate a total similarity score by adding up each number in the left
        // list after multiplying it by the number of times that number appears in the right list."
        var left2 = left1.GroupBy(x => x).ToImmutableDictionary(x => x.Key, x => x.Count());
        var right2 = right1.GroupBy(x => x).ToImmutableDictionary(x => x.Key, x => x.Count());

        var part2 = left2.Sum(x => right2.ContainsKey(x.Key) ? x.Key * x.Value * right2[x.Key] : 0);
        this.ShowDayResult(2, part2);
    }
}
