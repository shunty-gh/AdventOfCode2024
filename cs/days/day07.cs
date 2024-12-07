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

    private static long CheckEquation(long target, IReadOnlyList<long> nums, bool p2 = false)
    {
        var nlen = nums.Count;
        Queue<(long, int)> q = new();
        q.Enqueue((nums[0], 1));
        while (q.Count > 0)
        {
            var (acc, idx) = q.Dequeue();
            var next = nums[idx];
            var nplus = acc + next;
            var nmult = acc * next;
            var nconc = p2 ? long.Parse(acc.ToString() + next.ToString()) : 0;

            if (idx + 1 == nlen)
            {
                if (nplus == target || nmult == target || nconc == target)
                    return target;
                continue;
            }
            if (nplus <= target) q.Enqueue((nplus, idx + 1));
            if (nmult <= target) q.Enqueue((nmult, idx + 1));
            if (p2 && nconc <= target) q.Enqueue((nconc, idx + 1));
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
