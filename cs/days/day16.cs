namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/16 - Reindeer Maze

public class Day16 : AocDaySolver
{
    public int DayNumber => 16;
    public string Title => "Reindeer Maze";

    public bool PrintIt { get; set; } = false;
    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToImmutableList();
        //var input = (await Task.FromResult(TestInput1.Trim().Split('\n').Select(s => s.Trim()))).ToImmutableList();

        HashSet<Point2d> walls = [];
        Point2d start = new(0,0);
        Point2d end = new(0,0);
        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[0].Length; x++)
            {
                var ch = input[y][x];
                if (ch == '#')
                    walls.Add(new(x,y));
                else if (ch == 'S')
                    start = new(x, y);
                else if (ch == 'E')
                    end = new(x, y);
            }
        }

        Queue<(List<Point2d>, Point2d, int)> q = [];
        Dictionary<Point2d, int> seen = [];
        q.Enqueue(([start], new(1,0), 0));
        long bestscore = 0;
        List<(int, List<Point2d>)> bestpaths = [];
        while (q.Count > 0)
        {
            var (path, dir, score) = q.Dequeue();
            var curr = path.Last();

            if (bestscore > 0 && score > bestscore)
                continue;

            // This is a sneaky bit of the puzzle. Normally we'd do `&& score >= currsc` which
            // would be fine and we'd drop paths that were going to be too high. *But* in this
            // scenario it is possible that `currsc` could be 1000 higher than `score` because
            // it's path had previously taken one extra turn which the other path to curr would
            // only take in its next move whereas curr would be going stright on. This would
            // mean that the /next/ position on the path would have the same score whichever
            // route we took and, therefore we would need to keep it for part 2.
            if (seen.TryGetValue(curr, out var currsc) && score > currsc + 1000)
                continue;
            seen[curr] = score;

            foreach (var d in new List<Point2d> { dir, new(dir.Y, -dir.X), new(-dir.Y, dir.X) } ) // straight on, left, right
            {
                Point2d nx = new(curr.X + d.X, curr.Y + d.Y);
                var turn = d == dir ? 0 : 1000;

                var nxscore = score + turn + 1;
                if (walls.Contains(nx)|| (bestscore > 0 && bestscore < nxscore))
                    continue;
                // Skip if we've already determined that we can get to the next location with a lower score
                if (seen.TryGetValue(nx, out var nxsc) && nxscore > nxsc)
                    continue;

                List<Point2d> nxpath = [.. path, nx];
                // Quit if we're at the end point
                if (nx == end)
                {
                    if (bestscore == 0 || nxscore <= bestscore) // NB '<=' because we need all best paths for part 2, not just the first/any one
                    {
                        bestscore = nxscore;
                        bestpaths.Add((nxscore, nxpath));
                    }
                    continue;
                }
                q.Enqueue((nxpath, d, nxscore));
            }
        }

        if (PrintIt)
        {
            bestpaths.ForEach(bp => {
                if (bp.Item1 == bestscore)
                    Print(bp.Item2, walls);
            });
        }

        // For part 2, find the number of distinct points visited by all of the best paths
        HashSet<Point2d> part2 = [.. bestpaths
            .Where(bp => bp.Item1 == bestscore)
            .SelectMany(bp => bp.Item2)];

        this.ShowDayResult(1, bestscore);
        this.ShowDayResult(2, part2.Count);
    }

    private static void Print(List<Point2d> path, HashSet<Point2d> walls)
    {
        var xlen = walls.Max(w => w.X);
        var ylen = walls.Max(w => w.Y);
        List<string> screen = [];
        var hs = path.ToHashSet();
        for (var y = 0; y <= ylen; y++)
        {
            var sb = new StringBuilder();
            for (var x = 0; x <= xlen; x++)
            {
                Point2d p = new Point2d(x, y);
                if (walls.Contains(p))
                    sb.Append('#');
                else if (hs.Contains(p))
                    sb.Append('0');
                else
                    sb.Append('.');
            }
            screen.Add(sb.ToString());
        }

        Console.WriteLine();
        screen.ForEach(Console.WriteLine);
        Console.WriteLine();
    }

    // P1: 7036, P2: 45
    private const string TestInput1 = """
    ###############
    #.......#....E#
    #.#.###.#.###.#
    #.....#.#...#.#
    #.###.#####.#.#
    #.#.#.......#.#
    #.#.#####.###.#
    #...........#.#
    ###.#.#####.#.#
    #...#.....#.#.#
    #.#.#.###.#.#.#
    #.....#...#.#.#
    #.###.#.#.#.#.#
    #S..#.....#...#
    ###############
    """;

    // P1: 11048, P2: 64
    private const string TestInput2 = """
    #################
    #...#...#...#..E#
    #.#.#.#.#.#.#.#.#
    #.#.#.#...#...#.#
    #.#.#.#.###.#.#.#
    #...#.#.#.....#.#
    #.#.#.#.#.#####.#
    #.#...#.#.#.....#
    #.#.#####.#.###.#
    #.#.#.......#...#
    #.#.###.#####.###
    #.#.#...#.....#.#
    #.#.#.#####.###.#
    #.#.#.........#.#
    #.#.#.#########.#
    #S#.............#
    #################
    """;
}
