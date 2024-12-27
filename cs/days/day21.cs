namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/21 - Keypad Conundrum

public class Day21 : AocDaySolver
{
    public int DayNumber => 21;
    public string Title => "Keypad Conundrum";

/*

+---+---+---+
| 7 | 8 | 9 |
+---+---+---+
| 4 | 5 | 6 |
+---+---+---+
| 1 | 2 | 3 |
+---+---+---+
    | 0 | A |
    +---+---+

    +---+---+
    | ^ | A |
+---+---+---+
| < | v | > |
+---+---+---+

 */

    // Hard code the best paths for the direction pad 'cos there aren't that many. We'll add all paths for the
    // number pad in AddNumpadPaths() call. We'll ignore zig-zag paths such as "<^<".
    // Must take care not to include paths that cross the empty space.
    private static Dictionary<string, List<string>> AllPadPaths = new() {
        { "AA", ["A"] },            { "A^", ["<A"] },         { "A<", ["v<<A"] },  { "Av", ["<vA", "v<A"] }, { "A>", ["vA"] },
        { "^A", [">A"] },           { "^^", ["A"] },          { "^<", ["v<A"] },   { "^v", ["vA"] },         { "^>", ["v>A", ">vA"] },
        { "<A", [">>^A"] },         { "<^", [">^A"] },        { "<<", ["A"] },     { "<v", [">A"] },         { "<>", [">>A"] },
        { "vA", ["^>A", ">^A"] },   { "v^", ["^A"] },         { "v<", ["<A"] },    { "vv", ["A"] },          { "v>", [">A"] },
        { ">A", ["^A"] },           { ">^", ["<^A", "^<A"] }, { "><", ["<<A"] },   { ">v", ["<A"] },         { ">>", ["A"] },
    };

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToList();
        //var input = (await Task.FromResult(TestInput.Trim())).Split('\n').Select(s => s.Trim()).ToList();

        AddNumpadPaths();

        long part1 = 0, part2 = 0;
        foreach (var line in input)
        {
            var p1 = PathLength(line, 3);
            var p2 = PathLength(line, 26);
            //AnsiConsole.WriteLine($"{line} => {p1}; {p2}");
            part1 += p1 * int.Parse(line[..^1]);
            part2 += p2 * int.Parse(line[..^1]);
        }
        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
    }

    private static Dictionary<(string, int), long> Cache = [];
    private static long PathLength(string path, int level)
    {
        if (level == 0)
            return path.Length;
        if (Cache.TryGetValue((path, level), out long result))
            return result;

        result = 0;
        for (var i = 0; i < path.Length; i++)
        {
            // Split the path into 2 character chunks that we can look up in the paths list
            // Make sure to prefix the entire search path with A before splitting
            string frag = i == 0 ? "A" + path[0] : path[(i-1)..(i+1)];
            var paths = AllPadPaths[frag];
            if (paths.Count == 1)
                result += PathLength(paths[0], level-1);
            else if (paths.Count == 2) // There's never more than 2
                result += Math.Min(PathLength(paths[0], level-1), PathLength(paths[1], level-1));

        }
        Cache.TryAdd((path, level), result);
        return result;
    }

    /// <summary>
    /// Add shortest paths from each button on the numberpad to every other button
    /// </summary>
    private static void AddNumpadPaths()
    {
        // Could (should?) do this with BFS. But this way seems easy enough.

        foreach (var c0 in new char[11] {'A', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'})
        {
            foreach (var c1 in new char[11] {'A', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'})
            {
                if (c0 == c1)
                    continue;

                // Shortest distance is Manhattan distance, but we must avoid the empty space
                // 'A' -> '0' => (2,3) -> (1,3); dx = -1, dy = 0
                // To decide best paths we should avoid zig-zag paths - ie go straight across first then straight up/down, or vice-versa
                List<string> paths = [];
                var (x0, y0) = NumPadPoint(c0);
                var (x1, y1) = NumPadPoint(c1);
                var (dx, dy) = (x1 - x0, y1 - y0);
                // Left/right first, if we can avoid the empty space
                if (dx > 0 /* ok if we're going right */ || !(y0 == 3 && (x0 == 0 || x1 == 0)))
                {
                    var path = "";
                    path += dx < 0 ?  new string('<', -dx) : new string('>', dx);
                    path += dy < 0 ?  new string('^', -dy) : new string('v', dy);
                    path += "A";
                    paths.Add(path);
                }

                // Up/down first, avoiding empty space
                if (dy < 0 /* ok if we're going up */ || !(y1 == 3 && (x0 == 0 || x1 == 0)))
                {
                    var path = "";
                    path += dy < 0 ?  new string('^', -dy) : new string('v', dy);
                    path += dx < 0 ?  new string('<', -dx) : new string('>', dx);
                    path += "A";
                    paths.Add(path);
                }
                AllPadPaths.Add($"{c0}{c1}", paths);
            }
        }
    }

    private static readonly (int, int) InvalidPoint = (-1, -1);
    private static (int X, int Y) NumPadPoint(char c) => c switch {
        'A' => (2,3),
        '0' => (1,3),
        '1' => (0,2),
        '2' => (1,2),
        '3' => (2,2),
        '4' => (0,1),
        '5' => (1,1),
        '6' => (2,1),
        '7' => (0,0),
        '8' => (1,0),
        '9' => (2,0),
        _ => InvalidPoint,
    };

    // Expect P1: 126384 = 68 * 29 + 60 * 980 + 68 * 179 + 64 * 456 + 64 * 379
    // private const string TestInput = """
    // 029A
    // 980A
    // """;
    private const string TestInput = """
    029A
    980A
    179A
    456A
    379A
    """;
}
