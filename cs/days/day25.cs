namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/25 - Code Chronicle

public class Day25 : AocDaySolver
{
    public int DayNumber => 25;
    public string Title => "Code Chronicle";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToList();
        //var input = (await Task.FromResult(TestInput.Trim())).Split('\n').ToList();

        List<int[]> keys = [];
        List<int[]> locks = [];
        for (var i = 0; i < input.Count; i += 8)
        {
            int[] k = [0,0,0,0,0];
            for (var j = 0; j < 5; j++)
            {
                k[j] = input.Skip(i).Take(7).Sum(x => x[j] == '#' ? 1 : 0);
            }
            if (input[i][0] == '#')
                locks.Add(k);
            else
                keys.Add(k);
        }

        HashSet<(int[],int[])> pairs = [];
        foreach (var lk in locks)
        {
            foreach (var k in keys)
            {
                var overlap = false;
                for (var ki = 0; ki < 5; ki++)
                {
                    if (lk[ki] + k[ki] > 7)
                    {
                        overlap = true;
                        break;
                    }
                }
                if (!overlap)
                    pairs.Add((lk, k));
            }
        }

        this.ShowDayResult(1, pairs.Count);
    }

    private const string TestInput = """
    #####
    .####
    .####
    .####
    .#.#.
    .#...
    .....

    #####
    ##.##
    .#.##
    ...##
    ...#.
    ...#.
    .....

    .....
    #....
    #....
    #...#
    #.#.#
    #.###
    #####

    .....
    .....
    #.#..
    ###..
    ###.#
    ###.#
    #####

    .....
    .....
    .....
    #....
    #.#..
    #.#.#
    #####
    """;
}
