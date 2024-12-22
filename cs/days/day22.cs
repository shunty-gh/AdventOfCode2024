namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/22 -

public class Day22 : AocDaySolver
{
    public int DayNumber => 22;
    public string Title => "";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber)))
            .Select(x => long.Parse(x))
            .ToList();
        //var input = (await File.ReadAllTextAsync(AocUtils.FindInputFile(DayNumber))).Split("\n\n");
        //var input = (await Task.FromResult(TestInput.Trim())).Split('\n').ToImmutableList();

        long part1 = 0, part2 = 0;

        foreach (var n in input)
        {
            part1 += NextSecret(n, 2000);
        }
        this.ShowDayResult(1, part1);
        //this.ShowDayResult(2, NextSecret(15887950));
    }

    private static Dictionary<(long, int), long> cache = [];
    private long NextSecret(long start, int count)
    {
        if (cache.TryGetValue((start, count), out var result))
            return result;

        if (count == 1)
            return NextSecret(start);

        result = NextSecret(NextSecret(start), count - 1);
        cache[(start, count)] = result;
        return result;
    }

    private long NextSecret(long n)
    {
        long r1 = n << 6;
        r1 ^= n;
        r1 %= 16777216;

        long r2 = r1 >> 5;
        r2 ^= r1;
        r2 %= 16777216;

        long result = r2 << 11;
        result ^= r2;
        result %= 16777216;

        return result;
    }

    private const string TestInput = """
    12345
    """;
}
