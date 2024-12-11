namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/11 - Plutonian Pebbles

public class Day11 : AocDaySolver
{
    public int DayNumber => 11;
    public string Title => "Plutonian Pebbles";

    private Dictionary<(long, int), long> _cache = new();

    public async Task Solve()
    {
        var input = (await File.ReadAllTextAsync(AocUtils.FindInputFile(DayNumber)))
        //var input = (await Task.FromResult(TestInput))
            .Trim()
            .Split(' ')
            .Select(x => long.Parse(x))
            .ToArray();

        var part1 = EvolveStones(input, 25);
        this.ShowDayResult(1, part1);
        var part2 = EvolveStones(input, 75);
        this.ShowDayResult(2, part2);
    }

    private long EvolveStones(long[] stones, int count)
    {
        long result = 0;
        _cache.Clear();
        for (var i = 0; i < stones.Length; i++)
        {
            result += EvolveStone(stones[i], count);
        }
        return result;
    }

    private long EvolveStone(long stone, int count)
    {
        long[] nums;
        if (stone.Equals(0))
        {
            nums = [1];
        }
        else if (stone.ToString().Length % 2 == 0)
        {
            var s = stone.ToString();
            var hi = long.Parse(s[..(s.Length / 2)]);
            var lo = long.Parse(s[(s.Length / 2)..]);
            nums = [hi, lo];
        }
        else
        {
            nums = [stone * 2024];
        }

        // Recursion.. I hate recursion..
        if (count == 1)
        {
            return nums.Length;
        }
        else
        {
            long result = 0;
            for (var i = 0; i < nums.Length; i++)
            {
                // ...and memoization. Have we seen this stone before, with this count.
                if (_cache.TryGetValue((nums[i], count - 1), out var cached))
                {
                    result += cached;
                }
                else
                {
                    var res = EvolveStone(nums[i], count - 1);
                    _cache[(nums[i], count - 1)] = res;
                    result += res;
                }
            }
            return result;
        }
    }

    // Expect P1 = 55312
    private const string TestInput = """
    125 17
    """;
}
