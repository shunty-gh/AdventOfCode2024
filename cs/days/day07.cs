namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/7 - Bridge Repair

public class Day07 : AocDaySolver
{
    public int DayNumber => 7;
    public string Title => "Bridge Repair";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToImmutableList();
        //var input = TestInput.Split('\n').ToList();


        long part1 = 0, part2 = 0;
        foreach (var line in input)
        {
            var sp = line.Split(":");
            var target = long.Parse(sp[0].Trim());
            var nums = sp[1].Trim().Split(" ").Select(long.Parse).ToList();
            part1 += CheckEquation(target, nums);
            part2 += CheckEquation(target, nums, true);
        }
        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
    }

    private long CheckEquation(long target, IReadOnlyList<long> nums, bool p2 = false)
    {
        Queue<List<long>> q = new();
        q.Enqueue([.. nums]);
        while (q.Count > 0)
        {
            var tocheck = q.Dequeue();
            if (tocheck.Count == 1)
            {
                if (tocheck[0] == target)
                    return target;
                continue;
            }

            q.Enqueue([tocheck[0] + tocheck[1], .. tocheck[2..]]);
            q.Enqueue([tocheck[0] * tocheck[1], .. tocheck[2..]]);

            if (p2)
            {
                // Concat the first two numbers
                var nn = long.Parse(tocheck[0].ToString() + tocheck[1].ToString());
                q.Enqueue([nn, .. tocheck[2..]]);
            }
        }
        return 0; // No solution found
    }

    private const string TestInput = """
    190: 10 19
    3267: 81 40 27
    83: 17 5
    156: 15 6
    7290: 6 8 6 15
    161011: 16 10 13
    192: 17 8 14
    21037: 9 7 18 13
    292: 11 6 16 20
    """;
}
