namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/20 - Race Condition

public class Day20 : AocDaySolver
{
    public int DayNumber => 20;
    public string Title => "Race Condition";

    public bool IsTest = true;

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToList();
        //var input = (await Task.FromResult(TestInput.Trim())).Split('\n').Select(s => s.Trim()).ToList();

        HashSet<Point2d> walls = [];
        Point2d start = new(0,0), end = new(0,0);
        var xmax = input[0].Length;
        var ymax = input.Count;
        for (var y = 0; y < ymax; y++)
        {
            for (var x = 0; x < xmax; x++)
            {
                if (input[y][x] == '#')
                    walls.Add(new(x,y));
                else if (input[y][x] == 'S')
                    start = new(x,y);
                else if (input[y][x] == 'E')
                    end = new(x,y);
            }
        }

        var fromStart = Bfs(walls, start, end, xmax, ymax);
        var fromEnd = Bfs(walls, end, start, xmax, ymax);
        var bestNoCheat = fromStart[end];
        this.ShowDayResult(1, Part1(fromStart, fromEnd, start, end, xmax, ymax));

        int part2 = 0;
        this.ShowDayResult(2, part2);
    }

    private static int Part1(Dictionary<Point2d, long> fromStart, Dictionary<Point2d, long> fromEnd, Point2d start, Point2d end, int xmax, int ymax)
    {
        var result = 0;
        var threshold = 100;
        var noCheatDist = fromStart[end];
        for (var y = 0; y < ymax; y++)
        {
            for (var x = 0; x < xmax; x++)
            {
                Point2d p0 = new(x,y);
                // Must not be a wall - ie must be on the track
                if (!fromStart.ContainsKey(p0))
                    continue;
                foreach (var (dx1, dy1) in Point2d.NESW)
                {
                    Point2d p1 = new(p0.X + dx1, p0.Y + dy1);
                    // Must be a wall - a cheat must be a wall followed by racetrack
                    if (fromStart.ContainsKey(p1))
                        continue;
                    foreach (var (dx2, dy2) in Point2d.NESW)
                    {
                        Point2d p2 = new(p1.X + dx2, p1.Y + dy2);
                        // Must be racetrack
                        if (fromStart.ContainsKey(p2))
                        {
                            // Is it a shorter distance now
                            var distToHere = fromStart[p0] + 2;
                            var distFromHere = fromEnd[p2];
                            var cheatDist = distToHere + distFromHere;
                            if (noCheatDist - cheatDist >= threshold)
                                result += 1;
                        }
                    }
                }
            }
        }
        return result;
    }

    private static Dictionary<Point2d, long> Bfs(HashSet<Point2d> walls, Point2d start, Point2d end, int xmax, int ymax)
    {
        Queue<Point2d> q = [];
        Dictionary<Point2d, long> seen = [];
        seen.Add(start, 0);
        seen.Add(end, long.MaxValue);
        q.Enqueue(start);

        while (q.Count > 0)
        {
            var curr = q.Dequeue();
            var cost = seen[curr];
            foreach (var (dx, dy) in Point2d.NESW)
            {
                Point2d nx = new(curr.X + dx, curr.Y + dy);
                if (nx.X <= 0 || nx.Y <= 0 || nx.X >= xmax-1 || nx.Y >= ymax-1 || walls.Contains(nx))
                    continue;
                if (!seen.TryGetValue(nx, out var nxcost) || nxcost > cost + 1)
                {
                    q.Enqueue(nx);
                    seen[nx] = cost + 1;
                }
            }
        }
        return seen;
    }

    private const string TestInput = """
    ###############
    #...#...#.....#
    #.#.#.#.#.###.#
    #S#...#.#.#...#
    #######.#.#.###
    #######.#.#...#
    #######.#.###.#
    ###..E#...#...#
    ###.#######.###
    #...###...#...#
    #.#####.#.###.#
    #.#...#.#.#...#
    #.#.#.#.#.#.###
    #...#...#...###
    ###############
    """;
}
