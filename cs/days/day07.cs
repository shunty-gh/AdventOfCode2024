using System.Globalization;

namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/7 -

public class Day07 : AocDaySolver
{
    public int DayNumber => 7;
    public string Title => "";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToImmutableList();
        //var input = (await File.ReadAllTextAsync(AocUtils.FindInputFile(DayNumber))).Split("\n\n");
        //var input = TestInput.Split('\n').ToList();


        Int64 part1 = 0, part2 = 0;
        foreach (var line in input)
        {
            var sp = line.Split(":");
            var target = Int64.Parse(sp[0].Trim());
            var nums = sp[1].Trim().Split(" ").Select(Int64.Parse).ToList();
            part1 += CheckEquation(target, nums);
        }
        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
    }

    private Int64 CheckEquation(Int64 target, IReadOnlyList<Int64> nums)
    {
        Queue<List<Int64>> q = new();
        q.Enqueue(nums.ToList());
        while (q.Count > 0)
        {
            var tocheck = q.Dequeue();
            if (tocheck.Count == 1)
            {
                if (tocheck[0] == target)
                    return target;
                continue;
            }

            var nplus = tocheck[0] + tocheck[1];
            List<Int64> nxP = new([nplus]);
            nxP.AddRange(tocheck[2..]);
            q.Enqueue(nxP);

            var ntimes = tocheck[0] * tocheck[1];
            List<Int64> nxT = new([ntimes]);
            nxT.AddRange(tocheck[2..]);
            q.Enqueue(nxT);
        }
        return 0;
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
