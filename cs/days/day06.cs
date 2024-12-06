namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/6 - Guard Gallivant

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

        var part1 = RunRoute(input, start);
        this.ShowDayResult(1, part1);

        int part2 = 0;
        // Re-run the route while trying an obstacle at each possible position
        for (var y = 0; y < input.Count; y++)
            for (var x = 0; x < input[y].Length; x++)
                if (input[y][x] == '.')
                {
                    var loop = RunRoute(input, start, new Point(x, y));
                    if (loop < 0)
                        part2 += 1;
                }

        this.ShowDayResult(2, part2);
    }

    /// <summary>
    /// Run through the guards route with an optional extra obstacle at a given point (for part 2).
    /// </summary>
    /// <returns>The number of unique locations visited before exiting the area or -1 if we end up in a loop</returns>
    private int RunRoute(IReadOnlyList<string> map, Point start, Point? obstacle = null)
    {
        const char OOB = ' '; // the character we'll use to indicate out of bounds
        var current = start;
        var direction = new Point(0, -1);
        HashSet<Point> visited1 = new([start]);
        HashSet<(Point, Point)> visited2 = new([(start, direction)]);

        obstacle ??= new Point(-1, -1);
        var p1 = obstacle.X < 0;
        var p2 = !p1;
        while (true)
        {
            var next = new Point(current.X + direction.X, current.Y + direction.Y);
            var ch = map.CharAt(next.X, next.Y, OOB);
            if (ch == '#' || next == obstacle)
            {
                // and then a step to the right
                direction = new(-direction.Y, direction.X);
                next = new(current.X + direction.X, current.Y + direction.Y);
                ch = map.CharAt(next.X, next.Y, OOB);
                if (ch == '#' || next == obstacle)
                {
                    // Turn right again
                    // We won't need more than two right turns because the second turn will have
                    // us walking back to our previous location - which we know must be ok
                    direction = new(-direction.Y, direction.X);
                    next = new(current.X + direction.X, current.Y + direction.Y);
                    ch = map.CharAt(next.X, next.Y, OOB);
                }
            }
            if (ch == OOB)
            {
                // Elvis (she/they) is leaving the area
                break;
            }

            if (p2 && visited2.Contains((next, direction)))
            {
                // We've been here, going in this direction, already. Therefore it's a loop.
                return -1;
            }
            // else
            if (p1) visited1.Add(next);
            if (p2) visited2.Add((next, direction));
            current = new(next.X, next.Y);
        }
        return p1 ? visited1.Count : visited2.Count;
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
