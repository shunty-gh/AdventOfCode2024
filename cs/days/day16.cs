namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/16 -

public class Day16 : AocDaySolver
{
    public int DayNumber => 16;
    public string Title => "";

    public async Task Solve()
    {
        //var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToImmutableList();
        var input = (await Task.FromResult(TestInput2.Trim().Split('\n'))).ToImmutableList();

        HashSet<Point2d> map = [];
        Point2d start = new(0,0);
        Point2d end = new(0,0);
        for (var y = 0; y < input.Count; y++)
        {
            var line = input[y];
            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] == '#')
                {
                    map.Add(new(x,y));
                }
                else if (line[x] == 'S')
                    start = new(x, y);
                else if (line[x] == 'E')
                    end = new(x, y);
            }
        }

        Queue<(Point2d, Point2d, int, int)> q = [];
        Dictionary<Point2d, long> seen = [];
        q.Enqueue((start, new(1,0) , 0, 0));
        long bestscore = 0;
        while (q.Count > 0)
        {
            var (curr, dir, moves, turns) = q.Dequeue();
            var score = moves + 1000 * turns;
            if (curr == end)
            {
                if (bestscore == 0 || score < bestscore)
                    bestscore = score;
                continue;
            }

            if (!seen.ContainsKey(curr) || score < seen[curr])
                seen[curr] = score;
            else
                continue;

            foreach (var d in new List<Point2d> { dir, new(dir.Y, -dir.X), new(-dir.Y, dir.X) } )
            {
                Point2d nx = new(curr.X + d.X, curr.Y + d.Y);
                var t = d == dir ? turns : turns + 1;

                if (map.Contains(nx))
                    continue;
                if (nx == end)
                {
                    var sc = moves + 1 + 1000 * t;
                    if (bestscore == 0 || sc < bestscore)
                        bestscore = sc;
                    continue;
                }
                q.Enqueue((nx, d, moves + 1, t));
            }

        }

        int part2 = 0;
        this.ShowDayResult(1, bestscore);
        this.ShowDayResult(2, part2);
    }

    // P1: 7036
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

    // P1: 11048
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
