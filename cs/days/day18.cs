namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/18 - RAM Run

public class Day18 : AocDaySolver
{
    public int DayNumber => 18;
    public string Title => "RAM Run";

    public bool IsTest = false;

    public async Task Solve()
    {
        var bytes = (IsTest
            ? (await Task.FromResult(TestInput.Trim().Split('\n'))).Select(s => s.Trim())
            : (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).Select(s => s.Trim())
            )
            .Select(s => new Point2d(int.Parse(s.Split(',')[0]), int.Parse(s.Split(',')[1])))
            .ToList();

        // Part 1
        int p1start = IsTest ? 12 : 1024;
        int part1 = FindShortest(bytes[.. p1start]);
        this.ShowDayResult(1, part1);

        // Part 2
        int bc = bytes.Count;
        while (true)
        {
            var pp = FindShortest(bytes[.. bc], true);
            if (pp != 0)
                break;
            bc -= 1;
        }
        Point2d part2 = bytes[bc];
        this.ShowDayResult(2, $"{part2.X},{part2.Y}");
    }

    private static int FindShortest(IReadOnlyList<Point2d> bytes, bool part2 = false)
    {
        HashSet<Point2d> bb = [.. bytes];
        var xlen = bb.Max(b => b.X);
        var ylen = bb.Max(b => b.Y);

        Dictionary<Point2d, int> seen = [];
        Queue<Point2d> q = [];
        q.Enqueue(new(0,0));
        seen.Add(new(0,0), 0);
        Point2d target = new(xlen, ylen);
        while (q.Count > 0)
        {
            var c = q.Dequeue();
            if (c == target)
            {
                if (part2)
                    return 1;
                continue;
            }

            var cost = seen[c];
            foreach (var (dx, dy) in Point2d.NESW)
            {
                Point2d nx = new(c.X + dx, c.Y + dy);
                if (nx.X < 0 || nx.X > xlen || nx.Y < 0 || nx.Y > ylen || bb.Contains(nx))
                    continue;
                if (!seen.TryGetValue(nx, out var nxseen) || nxseen > cost+1)
                {
                    seen[nx] = cost + 1;
                    q.Enqueue(nx);
                }
            }
        }
        return part2 ? 0 : seen[target];
    }

    // Expect P1: 22; P2: 6,1
    private const string TestInput = """
    5,4
    4,2
    4,5
    3,0
    2,1
    6,3
    2,4
    1,5
    0,6
    3,3
    2,6
    5,1
    1,2
    5,5
    2,5
    6,5
    1,4
    0,4
    6,4
    1,1
    6,1
    1,0
    0,5
    1,6
    2,0
    """;
}
