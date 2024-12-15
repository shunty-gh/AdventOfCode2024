namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/15 - Warehouse Woes

public class Day15 : AocDaySolver
{
    public int DayNumber => 15;
    public string Title => "Warehouse Woes";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToList();
        //var input = (await File.ReadAllTextAsync(AocUtils.FindInputFile(DayNumber))).Split("\n\n");
        //var input = (await Task.FromResult(TestInput.Trim())).Split('\n').ToList();
        var map = new Dictionary<Point2d, char>();
        var moves = new List<Point2d>();
        var robotStart = ParseInput(input, map, moves);

        var part1 = ProcessMoves(robotStart, map, moves);
        int part2 = 0;
        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
    }

    private long ProcessMoves(Point2d robotStart, Dictionary<Point2d, char> startState, List<Point2d> moves)
    {
        var curr = robotStart;
        var state = new Dictionary<Point2d, char>(startState);
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
                else // It's a box
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
            }
        }

        var result = 0L;
        foreach (var (k,v) in state)
        {
            if (v == 'O')
            {
                result += 100 * k.Y + k.X;
            }
        }
        return result;
    }

    private Point2d ParseInput(List<string> input, Dictionary<Point2d, char> map, List<Point2d> moves)
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
                    switch (ch)
                    {
                        case '^':
                            moves.Add(new(0,-1));
                            break;
                        case '>':
                            moves.Add(new(1,0));
                            break;
                        case 'v':
                            moves.Add(new(0,1));
                            break;
                        case '<':
                            moves.Add(new(-1,0));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (ch == '@')
                        result = new(x, y);
                    if (ch == '.')
                        continue; // Don't store space
                    map.Add(new(x,y), ch);
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
}
