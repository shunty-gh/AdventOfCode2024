namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/4 - Ceres Search

public class Day04 : AocDaySolver
{
    public int DayNumber => 4;
    public string Title => "Ceres Search";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToImmutableList();
        //var input = TestData.Split('\n').ToList();

        var part1 = 0;
        for (int y = 0; y < input.Count; y++)
        {
            var row = input[y];
            for (int x = 0; x < row.Length; x++)
            {
                if ('X' == row[x])
                {
                    // Find MAS
                    var mas = FindMAS(x, y, input);
                    part1 += mas;
                }
            }
        }
        this.ShowDayResult(1, part1);

        var part2 = 0;
        for (int y = 0; y < input.Count; y++)
        {
            var row = input[y];
            for (int x = 0; x < row.Length; x++)
            {
                if ('A' == row[x])
                {
                    if (FindX_MAS(x, y, input))
                        part2 += 1;
                }
            }
        }
        this.ShowDayResult(2, part2);
    }

    private (int dx, int dy)[] Directions = [(0,-1), (1,-1), (1,0), (1,1), (0,1), (-1,1), (-1,0), (-1,-1)];
    private int FindMAS(int X, int Y, IReadOnlyList<string> source)
    {
        var result = 0;
        foreach (var (dx, dy) in Directions)
        {
            int px = X + dx;
            int py = Y + dy;
            if (source.CharAt(px, py) == 'M'
              && source.CharAt(px+dx, py+dy) == 'A'
              && source.CharAt(px+dx+dx, py+dy+dy) == 'S')
            {
                result += 1;
            }
        }
        return result;
    }

    private bool FindX_MAS(int X,int Y, IReadOnlyList<string> source)
    {
        // pt is an 'A'. We need to find M and S in both diagonals
        // ie ((above-left == S && below-right == M) || (above-left == M && below-right == S))
        //    && ((above-right == S && below-left == M) || (above-right == M && below-left == S))

        var upL = source.CharAt(X-1, Y-1);
        var upR = source.CharAt(X+1, Y-1);
        var dnL = source.CharAt(X-1, Y+1);
        var dnR = source.CharAt(X+1, Y+1);

        return ((upL == 'S' && dnR == 'M') || (upL == 'M' && dnR == 'S'))
            && ((upR == 'S' && dnL == 'M') || (upR == 'M' && dnL == 'S'));
    }

    // Test data: P1 expect 18; P2 expect 9;
    private const string TestData = """ 
    MMMSXXMASM
    MSAMXMSMSA
    AMXSXMAAMM
    MSAMASMSMX
    XMASAMXAMM
    XXAMMXXAMA
    SMSMSASXSS
    SAXAMASAAA
    MAMMMXMMMM
    MXMXAXMASX
    """;
}
