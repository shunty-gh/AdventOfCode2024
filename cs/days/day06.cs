namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/6 - Guard Gallivant

// Runs much faster when compiled in release mode. But still takes a few seconds.

public class Day06 : AocDaySolver
{
    private record Point(int X, int Y);
    public int DayNumber => 6;
    public string Title => "Guard Gallivant";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToImmutableList();
        //var input = TestData.Split('\n').ToList();

        // Find start position
        Point start = new(-1, -1);
        for (var y = 0; y < input.Count && start.X < 0; y++)
            for (var x = 0; x < input[y].Length && start.X < 0; x++)
                if (input[y][x] == '^')
                    start = new(x, y);

        var (part1, p1visited) = RunRoute(input, start);
        this.ShowDayResult(1, part1);

        int part2 = 0;
        // Re-run the route with an obstacle included
        for (var y = 0; y < input.Count; y++)
            for (var x = 0; x < input[y].Length; x++)
                // Only try obstacles at points that the original route visted. If the point wasn't
                // in the original route then an obstacle at that point won't make any difference
                if (input[y][x] == '.' && p1visited.Contains(new Point(x, y)))
                {
                    var (p2, _) = RunRoute(input, start, new Point(x, y));
                    part2 += p2 < 0 ? 1 : 0;
                }

        this.ShowDayResult(2, part2);
    }

    /// <summary>
    /// Run through the guards route with an optional extra obstacle at a given point (for part 2).
    /// </summary>
    /// <returns>The number of unique locations visited before exiting the area or -1 if we end up in a loop</returns>
    private (int, HashSet<Point>) RunRoute(IReadOnlyList<string> map, Point start, Point? obstacle = null)
    {
        var current = start;
        var direction = new Point(0, -1);
        HashSet<Point> visited1 = new([start]);
        HashSet<(Point, Point)> visited2 = new([(start, direction)]);

        obstacle ??= new Point(-1, -1);
        var p2 = obstacle.X >= 0;
        while (true)
        {
            var next = new Point(current.X + direction.X, current.Y + direction.Y);

            if (next.X < 0 || next.Y < 0 || next.Y >= map.Count || next.X >= map[0].Length)
            {
                // Elvis (she/they) is leaving the area
                return (visited1.Count, visited1);
            }

            if (p2 && visited2.Contains((next, direction)))
            {
                // We've been here, going in this direction, already. Therefore it's a loop.
                return (-1, []);
            }

            if (map[next.Y][next.X] == '#' || next == obstacle)
            {
                direction = new(-direction.Y, direction.X);
            }
            else
            {
                if (!p2) visited1.Add(next);
                if (p2) visited2.Add((next, direction));
                current = new(next.X, next.Y);
            }
        }
    }

    // Test data, expect P1 = 41; P2 = 6
    private const string TestData = """
    ....#.....
    .........#
    ..........
    ..#.......
    .......#..
    ..........
    .#..^.....
    ........#.
    #.........
    ......#...
    """;
}
