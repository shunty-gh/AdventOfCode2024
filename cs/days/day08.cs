namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/8 - Resonant Collinearity

public class Day08 : AocDaySolver
{
    public int DayNumber => 8;
    public string Title => "Resonant Collinearity";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToImmutableList();
        //var input = TestInput.Split('\n').Select(s => s.Trim()).ToList();

        var antennas = new Dictionary<char, List<(int x, int y)>>();
        for (var y = 0; y < input.Count; y++)
            for (var x = 0; x < input[y].Length; x++)
                if (input[y][x] != '.')
                {
                    var k = input[y][x];
                    var v = (x,y);
                    if (!antennas.ContainsKey(k))
                        antennas[k] = [];
                    antennas[k].Add(v);
                }

        var xlen = input[0].Length;
        var ylen = input.Count;
        Func<int,int,bool> inGrid = (x,y) => x >= 0 && x < xlen && y >= 0 && y < ylen;

        HashSet<(int x, int y)> antinodesP1 = [];
        HashSet<(int x, int y)> antinodesP2 = [];
        foreach (var ant in antennas.Values)
        {
            for (var i = 0; i < ant.Count-1; i++) // Pair them up
                for (var j = i+1; j < ant.Count; j++)
                {
                    var (ax,ay) = ant[i];
                    var (bx,by) = ant[j];
                    var (dx, dy) = (ax - bx, ay - by);

                    var mul = 1;
                    bool okH = true, okL = true;
                    while (okH || okL) // Keep going until both + and - distances are out of range
                    {
                        // Add antenna at multiples of dx,dy
                        var (antHx, antHy) = (ax + mul * dx, ay + mul * dy);
                        var (antLx, antLy) = (bx - mul * dx, by - mul * dy);

                        // Are they still in range?
                        if (!inGrid(antHx, antHy))
                            okH = false;
                        if (!inGrid(antLx, antLy))
                            okL = false;

                        if (mul == 1) // Part 1
                        {
                            if (okH) antinodesP1.Add((antHx, antHy));
                            if (okL) antinodesP1.Add((antLx, antLy));
                        }

                        if (okH) antinodesP2.Add((antHx, antHy));
                        if (okL) antinodesP2.Add((antLx, antLy));

                        mul += 1;
                    }
                    // Add each antenna for part 2
                    antinodesP2.Add((ax, ay));
                    antinodesP2.Add((bx, by));
                }
        }
        this.ShowDayResult(1, antinodesP1.Count);
        this.ShowDayResult(2, antinodesP2.Count);
    }

    private const string TestInput = """
    ............
    ........0...
    .....0......
    .......0....
    ....0.......
    ......A.....
    ............
    ............
    ........A...
    .........A..
    ............
    ............
    """;
}
