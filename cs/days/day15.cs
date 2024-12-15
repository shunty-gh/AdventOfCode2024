namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/15 - Warehouse Woes

public class Day15 : AocDaySolver
{
    public int DayNumber => 15;
    public string Title => "Warehouse Woes";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToList();
        //var input = (await Task.FromResult(TestInput.Trim())).Split('\n').ToList();
        //var input = (await Task.FromResult(TestInput2.Trim())).Split('\n').ToList();
        var map = new Dictionary<Point2d, char>();
        var moves = new List<Point2d>();

        var robotStart = ParseInput(input, map, moves, false);
        var part1 = ProcessMoves(robotStart, map, moves, false);
        this.ShowDayResult(1, part1);

        map.Clear();
        moves.Clear();
        robotStart = ParseInput(input, map, moves, true);
        var part2 = ProcessMoves(robotStart, map, moves, true, false);
        this.ShowDayResult(2, part2);
    }

    private long ProcessMoves(Point2d robotStart, Dictionary<Point2d, char> startState, List<Point2d> moves, bool part2, bool print = false)
    {
        var curr = robotStart;
        var state = new Dictionary<Point2d, char>(startState);
        if (print)
            Print(startState);
        foreach (var mv in moves)
        {
            Point2d next = new(curr.X + mv.X, curr.Y + mv.Y);
            if (!state.ContainsKey(next))
            {
                state.Remove(curr);
                curr = next;
                state[curr] = '@';
            }
            else
            {
                var ch = state[next];
                if (ch == '#')
                    continue;
                else if (!part2) // It's a box
                {
                    List<Point2d> boxesToMove = [next];
                    var blocked = false;
                    // Iterate along the direction of travel, past any boxes and find either the first empty space or a wall
                    Point2d nx = new(next.X + mv.X, next.Y + mv.Y);
                    while (!blocked)
                    {
                        if (!state.ContainsKey(nx))
                        {
                            foreach (var box in boxesToMove)
                            {
                                state.Remove(box);
                                state.Add(new(box.X + mv.X, box.Y + mv.Y), 'O');
                            }
                            state.Remove(curr);
                            curr = next;
                            state[curr] = '@';
                            break;
                        }
                        else
                        {
                            var nch = state[nx];
                            if (nch == '#') // Can't move anything
                                blocked = true;
                            else // Must be a box
                            {
                                boxesToMove.Insert(0, nx);
                            }
                        }
                        nx = new(nx.X + mv.X, nx.Y + mv.Y);
                    }
                }
                else // it's part of a box
                {
                    if (mv.Y == 0) // a HORIZONTAL move, easy
                    {
                        List<(Point2d, char)> partsToMove = [(next, ch)];
                        Point2d nx = new(next.X + mv.X, next.Y + mv.Y);
                        while (true)
                        {
                            if (!state.ContainsKey(nx))
                            {
                                foreach (var box in partsToMove)
                                {
                                    state.Remove(box.Item1);
                                    state.Add(new(box.Item1.X + mv.X, box.Item1.Y), box.Item2);
                                }
                                state.Remove(curr);
                                curr = next;
                                state[curr] = '@';
                                break;
                            }
                            else
                            {
                                var nch = state[nx];
                                if (nch == '#') // Can't move anything
                                    break;
                                else // Must be another part of a box
                                {
                                    partsToMove.Insert(0, (nx, nch));
                                }
                            }
                            nx = new(nx.X + mv.X, nx.Y + mv.Y);
                        }
                    }
                    else // a VERTICAL move, ie x == 0
                    {
                        // ch must be a left or right part of a box, so get the other half
                        Point2d ll = ch == '[' ? next : new(next.X - 1, next.Y);
                        Point2d rr = ch == ']' ? next : new(next.X + 1, next.Y);
                        List<Point2d> sectionToMove = [ll, rr];
                        List<List<Point2d>> sectionsToMove = [];
                        var canmove = true;
                        while (sectionToMove.Count > 0)
                        {
                            // Need to check that we can move every piece up or down
                            List<Point2d> nextSectionToMove = [];
                            foreach (var part in sectionToMove)
                            {
                                Point2d p2 = new(part.X, part.Y + mv.Y);
                                if (state.ContainsKey(p2))
                                {
                                    if (state[p2] == '#')
                                    {
                                        canmove = false;
                                        break;
                                    }
                                    nextSectionToMove.Add(p2);
                                }
                            }
                            if (canmove)
                            {
                                sectionsToMove.Insert(0,new(sectionToMove));
                                sectionToMove.Clear();
                                // Now we need to check that all box parts are included in the next section to move. Sheesh)
                                char seek = '[';
                                foreach (var sec in nextSectionToMove)
                                {
                                    var sch = state[sec];
                                    if (sch == seek)
                                    {
                                        sectionToMove.Add(sec);
                                        seek = seek == '[' ? ']' : '[';
                                    }
                                    else
                                    {
                                        // We need to add the other half
                                        if (seek == '[')
                                        {
                                            sectionToMove.Add(new(sec.X-1, sec.Y));
                                            sectionToMove.Add(sec);
                                        }
                                        else // looking for ] but find [
                                        {
                                            sectionToMove.Add(sec);
                                            sectionToMove.Add(new(sec.X+1, sec.Y));
                                        }
                                    }
                                }
                                // Check that we've moved all pairs
                                if (seek == ']' && sectionToMove.Count > 0)
                                {
                                    var last = sectionToMove.Last();
                                    sectionToMove.Add(new(last.X+1, last.Y));
                                }
                            }
                            else
                                break;
                        }
                        // And now, actually move the damn things
                        if (canmove)
                        {
                            foreach (var stm in sectionsToMove)
                            {
                                foreach (var sec in stm)
                                {
                                    char sch = state[sec];
                                    state.Remove(sec);
                                    state.Add(new(sec.X + mv.X, sec.Y + mv.Y), sch);
                                }
                            }
                            state.Remove(curr);
                            curr = next;
                            state[curr] = '@';
                        }
                    }
                }
            }
            if (print)
                Print(state);
        }

        var boxch = part2 ? '[' : 'O';
        long result = state.Sum(kvp => kvp.Value == boxch ? 100 * kvp.Key.Y + kvp.Key.X : 0);
        return result;
    }

    private static void Print(Dictionary<Point2d, char> state)
    {
        var xlen = state.Keys.Max(k => k.X);
        var ylen = state.Keys.Max(k => k.Y);
        var screen = new List<string>();
        for (var y = 0; y <= ylen; y++)
        {
            var sb = new StringBuilder();
            for (var x = 0; x <= xlen; x++)
            {
                Point2d p = new(x, y);
                if (state.TryGetValue(p, out var ch))
                {
                    sb.Append(ch);
                }
                else
                {
                    sb.Append('.');
                }
            }
            screen.Add(sb.ToString());
        }

        Console.WriteLine();
        screen.ForEach(s => Console.WriteLine(s));
        Console.WriteLine();
    }

    private static Point2d DirFromArrow(char ch) => ch switch
    {
        '^' => new(0, -1),
        '>' => new(1, 0),
        'v' => new(0, 1),
        '<' => new(-1, 0),
        _ => new(0, 0),
    };

    private Point2d ParseInput(List<string> input, Dictionary<Point2d, char> map, List<Point2d> moves, bool part2)
    {
        var dirs = false;
        Point2d result = new(0,0);
        for (var y = 0; y < input.Count; y++)
        {
            var line = input[y].Trim();
            if (string.IsNullOrWhiteSpace(line))
                dirs = true;

            for (var x = 0; x < line.Length; x++)
            {
                var ch = input[y][x];
                if (dirs)
                {
                    moves.Add(DirFromArrow(ch));
                }
                else
                {
                    var xx = part2 ? x * 2 : x;
                    if (ch == '@')
                        result = new(xx, y);
                    else if (ch == '#')
                    {
                        map.Add(new(xx,y), ch);
                        if (part2)
                            map.Add(new(xx+1,y), ch);
                    }
                    else if (ch == 'O')
                    {
                        if (part2)
                        {
                            map.Add(new(xx,y), '[');
                            map.Add(new(xx+1,y), ']');
                        }
                        else
                        {
                            map.Add(new(xx,y), ch);
                        }
                    }
                    // don't bother storing spaces
                }
            }
        }
        return result;
    }

    // Expect P1: 10092; P2: 9021
    private const string TestInput = """
    ##########
    #..O..O.O#
    #......O.#
    #.OO..O.O#
    #..O@..O.#
    #O#..O...#
    #O..O..O.#
    #.OO.O.OO#
    #....O...#
    ##########

    <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
    vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
    ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
    <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
    ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
    ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
    >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
    <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
    ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
    v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
    """;

    private const string TestInput2 = """
    #######
    #...#.#
    #.....#
    #..OO@#
    #..O..#
    #.....#
    #######

    <vv<<^^<<^^
    """;
}
