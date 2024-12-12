namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/12 - Garden Groups

public class Day12 : AocDaySolver
{
    public int DayNumber => 12;
    public string Title => "Garden Groups";

    private record Plot(Point2d Location, char Value, int Bounds, int RegionId);

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToImmutableList();
        //var input = (await Task.FromResult(TestInput)).Split('\n').Select(s => s.Trim()).ToImmutableList();

        // Build a list of all plots and which regions they belong too
        Dictionary<Point2d, Plot> plots = [];
        int regionId = 0;
        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                Point2d key = new(x, y);
                if (plots.ContainsKey(key))
                    continue;
                DiscoverRegion(key, regionId++, input, plots);
            }
        }

        int part1 = 0, part2 = 0;
        var regions = plots.GroupBy(x => x.Value.RegionId, x => x.Value);
        foreach (var region in regions)
        {
            part1 += region.Count() * region.Sum(r => r.Bounds);
            part2 += region.Count() * SidesCount(region.ToImmutableList());
        }
        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
    }

    private void DiscoverRegion(Point2d start, int regionId, IReadOnlyList<string> grid, Dictionary<Point2d, Plot> plots)
    {
        var q = new Queue<Point2d>([start]);
        while (q.Count > 0)
        {
            var curr = q.Dequeue();
            if (plots.ContainsKey(curr))
                continue;

            var bounds = 4; // Max no of external boundaries. Reduce for each adjacent plot.
            var ch = grid[curr.Y][curr.X];
            foreach (var (dx, dy) in Point2d.NESW)
            {
                Point2d next = new(curr.X + dx, curr.Y + dy);
                if (grid.CharAt(next.X, next.Y) == ch)
                {
                    bounds -= 1;
                    if (!plots.ContainsKey(next))
                        q.Enqueue(next);
                }
            }
            plots.Add(curr, new(curr, ch, bounds, regionId));
        }
    }

    private int SidesCount(IReadOnlyList<Plot> region)
    {
        // The number of sides can be determined by the total number of corners in the region.
        // A corner is either an internal or an external corner.

        // A region containing a single plot must have 4 sides
        if (region.Count == 1)
            return 4;

        var result = 0;
        Dictionary<Point2d, Plot> plotmap = region.ToDictionary(p => p.Location, p => p);
        foreach (var plot in region)
        {
            // for each plot we need to determine if it is part of an internal corner
            // or an external corner (or neither)

            // if the plot has 3 external edges (bounds) then it must have 2 external corners
            if (plot.Bounds == 3)
                result += 2;
            else
            {
                // An external corner (X) has two touching, external facing (to the region) sides
                //
                //  . . . .
                //  . X #
                //  . # #
                //
                foreach (var neigh in Point2d.NS)
                {
                    Point2d pp = new(plot.Location.X + neigh.X, plot.Location.Y + neigh.Y);
                    if (!plotmap.ContainsKey(pp))
                    {
                        // A side 90 degrees cw or -cw must also not be in the region
                        Point2d e1 = new(plot.Location.X - neigh.Y, plot.Location.Y + neigh.X);
                        Point2d e2 = new(plot.Location.X + neigh.Y, plot.Location.Y - neigh.X);
                        if (!plotmap.ContainsKey(e1) || !plotmap.ContainsKey(e2))
                            result += 1;
                            break; // There can only be, at most, one. If there were more then the number of bounds would be 3 and we've already dealt with that situation
                    }
                }

                // An internal corner (X) is where the diagonal plot is not in the same region
                // but the plots in adjacent sides are.
                //
                //  # # # .
                //  # X # .
                //  # # . .
                //
                foreach (var diag in Point2d.Diagonals)
                {
                    Point2d pp = new(plot.Location.X + diag.X, plot.Location.Y + diag.Y);
                    if (!plotmap.ContainsKey(pp))
                    {
                        // The adjacent sides must be in the region
                        Point2d e1 = new(plot.Location.X + diag.X, plot.Location.Y);
                        Point2d e2 = new(plot.Location.X, plot.Location.Y + diag.Y);
                        if (plotmap.ContainsKey(e1) && plotmap.ContainsKey(e2))
                            result += 1;
                    }
                }
            }
        }

        return result;
    }

    // Expect P1 = 1930; P2 = 1206
    private const string TestInput = """
    RRRRIICCFF
    RRRRIICCCF
    VVRRRCCFFF
    VVRCCCJFFF
    VVVVCJJCFE
    VVIVCCJJEE
    VVIIICJJEE
    MIIIIIJJEE
    MIIISIJEEE
    MMMISSJEEE
    """;

    // Expect P2 = 368
    // private const string TestInput = """
    // AAAAAA
    // AAABBA
    // AAABBA
    // ABBAAA
    // ABBAAA
    // AAAAAA
    // """;
}
