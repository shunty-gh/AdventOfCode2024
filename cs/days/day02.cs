using System.Collections.Immutable;

namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/2 - Red-Nosed Reports

public class Day02 : AocDaySolver
{
    public int DayNumber => 2;

    public async Task Solve()
    {
        var lines = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber)))
        //var lines = TestInput.Trim().Split('\n')
            .Select(x => x.Split(' '))
            .Select(x => x.Select(y => int.Parse(y)).ToImmutableList())
            .ToImmutableList();

        var part1 = lines.Sum(x => TestLine(x));
        this.ShowDayResult(1, part1);

        var part2 = 0;
        foreach (var line in lines)
        {
            for (var idx = -1; idx <= line.Count - 1; idx++)
            {
                // Make a copy of the line, take out one element at a time
                // (except for the first run) and re-test
                var ln = line.ToList();
                if (idx >= 0)
                    ln.RemoveAt(idx);
                var res = TestLine(ln);
                if (res == 1)
                {
                    part2 += 1;
                    break;
                }
            }
        }
        this.ShowDayResult(2, part2);
    }

    private int TestLine(IReadOnlyList<int> nums)
    {
        // Are they ordered up or down
        var inc = nums.Zip(nums.Skip(1), (x, y) => (x, y)).All(z => z.x < z.y);
        var dec = nums.Zip(nums.Skip(1), (x, y) => (x, y)).All(z => z.x > z.y);
        // If so, then are they not too far apart
        if (inc || dec)
        {
            for (var i = 0; i < nums.Count - 1; i++)
            {
                if (Math.Abs(nums[i] - nums[i+1]) > 3)
                    return 0;
            }
        }
        return inc || dec ? 1 : 0;
    }

    //Test data, expect P1 = 2, P2 = 4
    private const string TestInput = """
    7 6 4 2 1
    1 2 7 8 9
    9 7 6 2 1
    1 3 2 4 5
    8 6 4 4 1
    1 3 6 7 9
    """;
}
