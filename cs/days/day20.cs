namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/20 -

public class Day20 : AocDaySolver
{
    public int DayNumber => 20;
    public string Title => "";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToList();
        //var input = (await File.ReadAllTextAsync(AocUtils.FindInputFile(DayNumber))).Split("\n\n");
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

        var bestNoCheat = Shortest(walls, start!, end, xmax, ymax, new(0,0), 0);
        var part1 = 0;
        foreach (var wall in walls)
        {
            if (wall.X == 0 || wall.Y == 0 || wall.X == xmax - 1 || wall.Y == ymax - 1)
                continue;
            var cheating = Shortest(walls, start!, end, xmax, ymax, wall, bestNoCheat);
            if (bestNoCheat - cheating >= 100)
                part1 += 1;
            // if (cheating < bestNoCheat)
            //     this.ShowDayResult(1, $"{cheating} (saving {bestNoCheat - cheating})");
        }

        int part2 = 0;
        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
    }

    private long Shortest(HashSet<Point2d> walls, Point2d start, Point2d end, int xmax, int ymax, Point2d missing, long target)
    {
        Queue<Point2d> q = [];
        q.Enqueue(start);
        Dictionary<Point2d, long> seen = [];
        seen.Add(start, 0);
        seen.Add(end, long.MaxValue);

        while (q.Count > 0)
        {
            var curr = q.Dequeue();

            var cost = seen[curr];
            if ((target > 0 && cost > target) || cost > seen[end])
                continue;
            foreach (var (dx, dy) in Point2d.NESW)
            {
                Point2d nx = new(curr.X + dx, curr.Y + dy);
                if (nx.X < 0 || nx.Y < 0 || nx.X >= xmax || nx.Y >= ymax || (walls.Contains(nx) && nx != missing))
                    continue;
                if (!seen.TryGetValue(nx, out var nxcost) || nxcost > cost + 1)
                {
                    q.Enqueue(nx);
                    seen[nx] = cost + 1;
                }
            }
        }
        return seen[end];
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
