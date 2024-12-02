using System.Collections.Immutable;

namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/2 -

public class Day02 : AocDaySolver
{
    public int DayNumber => 2;

    private enum LevelDiff
    {
        None,
        Increase,
        Decrease,
        Fail,
    }

    public async Task Solve()
    {
        var lines = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber)))
            .Select(x => x.Split(' '))
            .Select(x => x.Select(y => int.Parse(y)).ToImmutableList())
            .ToImmutableList();

        var part1 = lines.Sum(x => ComparePart1(x));
            this.ShowDayResult(1, part1);
    }

    private int ComparePart1(IReadOnlyList<int> nums)
    {
        Func<int, int, LevelDiff, LevelDiff>  compPart1 = (x, y, ld) =>
        {
            if (x == y) return LevelDiff.Fail;
            if (x < y && y - x <= 3) return ld == LevelDiff.None || ld == LevelDiff.Increase ? LevelDiff.Increase : LevelDiff.Fail;
            if (x > y && x - y <= 3) return ld == LevelDiff.None || ld == LevelDiff.Decrease ? LevelDiff.Decrease : LevelDiff.Fail;
            return LevelDiff.Fail;
        };

        var result = compPart1(nums[0], nums[1], LevelDiff.None);
        if (result == LevelDiff.Fail)
            return 0;

        for (var i = 1; i < nums.Count - 1; i++)
        {
            var diff = compPart1(nums[i], nums[i+1], result);
            if (diff == LevelDiff.Fail)
                return 0;
        }
        return 1;
    }
}
