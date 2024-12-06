using System.Transactions;

namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/6 -

public class Day06 : AocDaySolver
{
    private record Point(int X, int Y);
    public int DayNumber => 6;
    public string Title => "";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToImmutableList();
        //var input = (await File.ReadAllTextAsync(AocUtils.FindInputFile(DayNumber))).Split("\n\n");
        //var input = TestData.Split('\n').ToList();

        // Find ^
        var start = new Point(-1,-1);
        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == '^')
                {
                    start = new(x, y);
                    break;
                }
            }
        }

        int part1 = 0, part2 = 0;
        var current = start;
        var direction = new Point(0, -1);
        var leave = new Point(-1, -1);
        HashSet<Point> visited = new([start]);
        while (true)
        {
            var next = new Point(current.X + direction.X, current.Y + direction.Y);
            if (next.X < 0 || next.X >= input[0].Length || next.Y < 0 || next.Y >= input.Count)
            {
                // Elvis is leaving the area
                leave = new(current.X, current.Y);
                break;
            }

            if (input[next.Y][next.X] == '#')
            {
                // Turn right
                direction = new(-direction.Y, direction.X);
                next = new Point(current.X + direction.X, current.Y + direction.Y);
                if (next.X < 0 || next.X >= input[0].Length || next.Y < 0 || next.Y >= input.Count)
                {
                    // Elvis is leaving the area
                    leave = new(current.X, current.Y);
                    break;
                }
            }

            // else
            visited.Add(next);
            current = new(next.X, next.Y);
        }

        this.ShowDayResult(1, visited.Count);
        this.ShowDayResult(2, part2);
    }

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
