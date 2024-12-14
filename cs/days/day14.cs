#define PRINT_TREE

using System.Text.RegularExpressions;

namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/14 - Restroom Redoubt


public partial class Day14 : AocDaySolver
{
    public int DayNumber => 14;
    public string Title => "Restroom Redoubt";

    private bool IsTest => false;
    private record Robot(int Id, Point2d Location, Point2d Velocity);
    public async Task Solve()
    {
        ImmutableList<string> input;
        if (IsTest)
            input = (await Task.FromResult(TestInput.Split('\n'))).ToImmutableList();
        else
            input = (await File.ReadAllTextAsync(AocUtils.FindInputFile(DayNumber))).Split("\n").ToImmutableList();
        Dictionary<int, Robot> robots = [];
        var robotId = 0;
        foreach (var line in input.Where(s => !string.IsNullOrWhiteSpace(s)))
        {
            var match = Re().Match(line);
            Point2d p = new(int.Parse(match.Groups["px"].Value), int.Parse(match.Groups["py"].Value));
            Point2d v = new(int.Parse(match.Groups["vx"].Value), int.Parse(match.Groups["vy"].Value));
            robots.Add(robotId, new(robotId, p, v));
            robotId += 1;
            //Console.WriteLine($"P: {p}; V: {v}");
        }

        var part1 = Part1(robots);
        int part2 = await FindATree(robots);
        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
    }

    private int XMod => IsTest ? 11 : 101;
    private int YMod => IsTest ? 7 : 103;
    // C# mod operator, '%', returns -ve numbers for -ve input - which is not what we
    // want (nor what (most, I dunno?) other languages do). Therefore we have to provde our
    // own augmentation of it:
    private int Mod(int v, int mod) => ((v % mod) + mod) % mod;

    private long Part1(IReadOnlyDictionary<int, Robot> robots)
    {
        var state = Move(robots, 100);
        var q1 = state.Where(k => k.Key.X < (XMod - 1) / 2 && k.Key.Y < (YMod - 1) / 2).Sum(k => k.Value.Count);
        var q2 = state.Where(k => k.Key.X > (XMod - 1) / 2 && k.Key.Y < (YMod - 1) / 2).Sum(k => k.Value.Count);
        var q3 = state.Where(k => k.Key.X < (XMod - 1) / 2 && k.Key.Y > (YMod - 1) / 2).Sum(k => k.Value.Count);
        var q4 = state.Where(k => k.Key.X > (XMod - 1) / 2 && k.Key.Y > (YMod - 1) / 2).Sum(k => k.Value.Count);
        return q1 * q2 * q3 * q4;
    }

    private Dictionary<Point2d, List<Robot>> Move(IReadOnlyDictionary<int, Robot> robots, int count)
    {
        Dictionary<Point2d, List<Robot>> state = [];
        foreach (var (id,r) in robots)
        {
            var rr = r with { Location = new(Mod(r.Location.X + count * r.Velocity.X, XMod), Mod(r.Location.Y + count * r.Velocity.Y, YMod)) };
            if (!state.ContainsKey(rr.Location))
                state[rr.Location] = [];
            state[rr.Location].Add(rr);
        }

        return state;
    }

    private async Task<int> FindATree(Dictionary<int, Robot> robots)
    {
#if PRINT_TREE
        var printit = true;
#else
        var printit = false;
#endif
        // Assume it's in the upper range of possibilities so start high and work our way down
        for (var i = XMod * YMod - 1; i > 0; i--)
        {
            var state = Move(robots, i);
            if (await MightItBeATree(state, i, printit))
            {
                // Also we'll assume (from limited testing) that the first possible hit is actually the only possible one
                return i;
            }
        }
        return -1;
    }

    private async Task<bool> MightItBeATree(Dictionary<Point2d, List<Robot>> state, int count, bool printIt)
    {
        var result = false;
        var screen = new List<string>();
        for (var y = 0; y < YMod; y++)
        {
            var sb = new StringBuilder();
            for (var x = 0; x < XMod; x++)
            {
                Point2d p = new(x, y);
                if (state.ContainsKey(p))
                {
                    sb.Append('.');
                }
                else
                {
                    sb.Append(' ');
                }
            }
            var s = sb.ToString();
            // The tree will contain at least one sequence of solid output. Let's assume an arbitrary length.
            if (s.Contains("..............."))
                result = true;
            screen.Add(s);
        }

        if (result && printIt)
        {
            screen.ForEach(s => Console.WriteLine(s));
            Console.WriteLine($"*** Iterations: {count} ***");
            Console.WriteLine();
            Console.WriteLine();
            await Task.Delay(500);
        }
        return result;
    }

    private const string Pattern = @"(?<px>\d+),(?<py>\d+)\sv=(?<vx>\-?\d+),(?<vy>\-?\d+)";
    [GeneratedRegex(Pattern)]
    private static partial Regex Re();
    private const string TestInput = """
    p=0,4 v=3,-3
    p=6,3 v=-1,-3
    p=10,3 v=-1,2
    p=2,0 v=2,-1
    p=0,0 v=1,3
    p=3,0 v=-2,-2
    p=7,6 v=-1,-3
    p=3,0 v=-1,-2
    p=9,3 v=2,3
    p=7,3 v=-1,2
    p=2,4 v=2,-3
    p=9,5 v=-3,-3
    """;
}
