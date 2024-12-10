namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/10 - Hoof It

public class Day10 : AocDaySolver
{
    public int DayNumber => 10;
    public string Title => "Hoof It";

    private record Point(int X, int Y);

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToImmutableList();
        //var input = await Task.FromResult(TestInput.Split('\n').Select(s => s.Trim()).ToImmutableList());

        List<Point> starts = [];
        foreach (var (r,ln) in input.Select((rr, i) => (i, rr)))
            foreach (var (c,ch) in ln.Select((cc, j) => (j, cc)))
                if (ch == '0')
                    starts.Add(new(c, r));

        int part1 = 0, part2 = 0;
        foreach (var start in starts)
        {
            var (p1, p2) = ScoreTrail(input, start);
            part1 += p1;
            part2 += p2;
        }

        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
    }

    private static readonly List<Point> NESW = [new(0,-1), new(1,0), new(0,1), new(-1,0)];
    private static (int, int) ScoreTrail(IReadOnlyList<string> grid, Point start)
    {
        var q = new Queue<Point>([start]);
        var visited = new HashSet<Point>(); // For part 1
        int allroutes = 0;                  // For part 2
        while (q.Count > 0)
        {
            var (cx, cy) = q.Dequeue();
            var cv = grid[cy][cx] - '0';

            foreach (var (dx,dy) in NESW)
            {
                var (nx, ny) = (cx + dx, cy + dy);
                if (nx < 0 || nx >= grid[0].Length || ny < 0 || ny >= grid.Count)
                    continue;

                var nch = grid[ny][nx];
                if (nch == '9' && cv == 8) // Reached the peak of the trail
                {
                    visited.Add(new(nx, ny)); // Part1 requires how many unique '9's are visited per start point
                    allroutes += 1;           // Part2 asks how many different routes there are to the top - ie in this case, the total number of times we get there
                }
                else if (nch >= '0' && nch <= '9' && nch - '0' == cv + 1)
                {
                    q.Enqueue(new(nx, ny));
                }
            }
        }
        return (visited.Count, allroutes);
    }

    // Test data, expect P1 = 36; P2 = 81
    private const string TestInput = """
    89010123
    78121874
    87430965
    96549874
    45678903
    32019012
    01329801
    10456732
    """;
}
