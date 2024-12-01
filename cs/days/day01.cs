using System.Collections.Immutable;

namespace Shunty.AoC.Days;

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

        // "Calculate a total similarity score by adding up each number in the left
        // list after multiplying it by the number of times that number appears in the right list."
        var left2 = left1.GroupBy(x => x).ToImmutableDictionary(x => x.Key, x => x.Count());
        var right2 = right1.GroupBy(x => x).ToImmutableDictionary(x => x.Key, x => x.Count());

        var part2 = 0L;
        foreach (var (key, value) in left2)
        {
            if (right2.ContainsKey(key))
            {
                part2 += key * value * right2[key];
            }
        }
        this.ShowDayResult(2, part2);
    }
}
