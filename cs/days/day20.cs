namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/20 - Race Condition

public class Day20 : AocDaySolver
{
    public int DayNumber => 20;
    public string Title => "Race Condition";

    public bool IsTest => false;
    public async Task Solve()
    {
        var input = IsTest
            ? (await Task.FromResult(TestInput.Trim())).Split('\n').Select(s => s.Trim()).ToList()
            : (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToList();

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

        var fromStart = Bfs(walls, start, xmax, ymax);
        var fromEnd = Bfs(walls, end, xmax, ymax);

        this.ShowDayResult(1, Part1(fromStart, fromEnd, end, xmax, ymax));
        this.ShowDayResult(2, Part2(fromStart, fromEnd, end, xmax, ymax));
    }

    /// <summary>
    /// At each point on the track look for an opportunity to cheat by going through a wall.
    /// The point after the wall must be back on the racetrack.
    /// </summary>
    /// <param name="fromStart">The set of all distances from the given point to the end of the racetrack</param>
    /// <param name="fromEnd">The set of all distances from the end of the racetrack to the given point</param>
    /// <param name="end">The location of the end of the race</param>
    /// <param name="xmax">Width of the racing area</param>
    /// <param name="ymax">Height of the racing area</param>
    /// <returns>The total number of possible cheats that are at least 100 picoseconds quicker than the time without cheating</returns>
    private int Part1(Dictionary<Point2d, long> fromStart, Dictionary<Point2d, long> fromEnd, Point2d end, int xmax, int ymax)
    {
        var result = 0;
        var threshold = IsTest ? 50 : 100;
        var noCheatDist = fromStart[end];

        foreach (var (p0, dist0) in fromStart)
        {
            foreach (var (dx1, dy1) in Point2d.NESW)
            {
                // Look for a piece of wall to go through
                Point2d p1 = new(p0.X + dx1, p0.Y + dy1);
                // Must be a wall, not track
                if (fromStart.ContainsKey(p1))
                    continue;

                // Now look for a piece of track to get back on to
                foreach (var (dx2, dy2) in Point2d.NESW)
                {
                    Point2d p2 = new(p1.X + dx2, p1.Y + dy2);
                    // Must be racetrack
                    if (fromStart.ContainsKey(p2))
                    {
                        // Was it worth it. Dist to the point of the cheat + one step through wall + one step
                        // to get back on track + distance to/from the end point
                        var cheatDist = dist0 + 2 + fromEnd[p2];
                        if (noCheatDist - cheatDist >= threshold)
                            result += 1;
                    }
                }
            }
        }
        return result;
    }

    /// <summary>
    /// At each point on the track look for an opportunity to cheat by going through any combination
    /// of walls and track for up to 20 moves (pico seconds)
    /// </summary>
    /// <param name="fromStart">The set of all distances from the given point to the end of the racetrack</param>
    /// <param name="fromEnd">The set of all distances from the end of the racetrack to the given point</param>
    /// <param name="end">The location of the end of the race</param>
    /// <param name="xmax">Width of the racing area</param>
    /// <param name="ymax">Height of the racing area</param>
    /// <returns>The total number of possible extended cheats that are at least 100 picoseconds quicker than the time without cheating</returns>
    private int Part2(Dictionary<Point2d, long> fromStart, Dictionary<Point2d, long> fromEnd, Point2d end, int xmax, int ymax)
    {
        var result = 0;
        var threshold = IsTest ? 50 : 100;
        var noCheatDist = fromStart[end];
        var cheatlimit = 20;

        foreach (var (p0, dist0) in fromStart)
        {
            // Every point with a Manhattan distance of `cheatlimit` or less, that is also
            // a bit of track, is a valid cheat end point
            for (var dx = -20; dx <= 20; dx++ )
            {
                for (var dy = -20; dy <= 20; dy++)
                {
                    Point2d p1 = new(p0.X + dx, p0.Y + dy);
                    // Is it in range
                    if (Math.Abs(dx) + Math.Abs(dy) <= cheatlimit && p0 != p1)
                    {
                        // Is it racetrack (ie not wall)
                        if (fromEnd.TryGetValue(p1, out var distToEnd))
                        {
                            var cheatDist = dist0 + Math.Abs(dx) + Math.Abs(dy) + distToEnd;
                            if (noCheatDist - cheatDist >= threshold)
                                result += 1;
                        }
                    }
                }
            }
        }
        return result;
    }

    /// <summary>
    /// A basic breadth first search to get shortest path distances to all points in a grid from a given start
    /// point. Paths cannot go through points marked as walls.
    /// </summary>
    /// <param name="walls">A list of points designated as walls</param>
    /// <param name="start">The location of the start point, from which all distances are measured</param>
    /// <param name="xmax">Width of the available area</param>
    /// <param name="ymax">Height of the available area</param>
    /// <returns>A dictionary/map of points and their shortest distance to the start point</returns>
    private static Dictionary<Point2d, long> Bfs(HashSet<Point2d> walls, Point2d start, int xmax, int ymax)
    {
        Queue<Point2d> q = [];
        Dictionary<Point2d, long> seen = [];
        seen.Add(start, 0);
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
