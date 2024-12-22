namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/22 - Monkey Market

public class Day22 : AocDaySolver
{
    public int DayNumber => 22;
    public string Title => "Monkey Market";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber)))
        //var input = (await Task.FromResult(TestInput.Trim())).Split('\n')
            .Select(x => long.Parse(x))
            .ToList();

        long part1 = input.Sum(n => Part1(n, 2000));
        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, Part2(input));
    }

    private long Part2(List<long> nums)
    {
        List<Dictionary<(int,int,int,int), int>> allseqs = [];
        HashSet<(int,int,int,int)> seqstotest = [];
        foreach (var (i,n) in nums.Select((x,i) => (i,x)))
        {
            List<(int, int)> diffs = [];
            Dictionary<(int,int,int,int), int> seqs = [];
            int prev = 0;
            long ns = n;
            for (var c = 1; c <= 2000; c++)
            {
                ns = NextSecret(ns);
                var d = (int)(ns % 10);
                diffs.Add((d, d - prev));
                prev = d;

                // Collect all the sequences of 4 diffs
                if (c > 3)
                {
                    var k = (diffs[c-4].Item2, diffs[c-3].Item2, diffs[c-2].Item2, diffs[c-1].Item2);
                    seqs.TryAdd(k, diffs[c-1].Item1);
                    seqstotest.Add(k);
                }
            }

            allseqs.Add(seqs);
        }

        var topbananas = 0;
        foreach (var seq in seqstotest)
        {
            var bananas = 0;
            foreach (var s in allseqs)
            {
                if (s.TryGetValue(seq, out var b))
                    bananas += b;
            }
            if (bananas > topbananas)
                topbananas = bananas;
        }

        return topbananas;
    }

    private static long Part1(long num, int count)
    {
        long result;
        if (count == 1)
            result = NextSecret(num);
        else
        {
            result = Part1(NextSecret(num), count - 1);
        }
        return result;
    }

    private static long NextSecret(long n)
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
    1
    2
    3
    2024
    """;
}
