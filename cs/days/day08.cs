namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/8 -

public class Day08 : AocDaySolver
{
    public int DayNumber => 8;
    public string Title => "";

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
        int part1 = 0, part2 = 0;
        HashSet<(int x, int y)> antinodes = new();
        var xlen = input[0].Length;
        var ylen = input.Count;
        foreach (var (k,v) in antennas)
        {
            for (var i = 0; i < v.Count; i++)
                for (var j = i+1; j < v.Count; j++)
                {
                    var (ax,ay) = v[i];
                    var (bx,by) = v[j];
                    var (dx, dy) = (ax - bx, ay - by);
                    var (ant1x, ant1y) = (ax + dx, ay + dy);
                    var (ant2x, ant2y) = (bx - dx, by - dy);
                    if (ant1x >= 0 && ant1x < xlen && ant1y >= 0 && ant1y < ylen)
                        antinodes.Add((ant1x, ant1y));
                    if (ant2x >= 0 && ant2x < xlen && ant2y >= 0 && ant2y < ylen)
                        antinodes.Add((ant2x, ant2y));
                }
        }
        part1 = antinodes.Count;
        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
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
