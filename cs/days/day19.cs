namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/19 - Linen Layout

public class Day19 : AocDaySolver
{
    public int DayNumber => 19;
    public string Title => "Linen Layout";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToList();
        //var input = (await Task.FromResult(TestInput.Trim())).Split('\n').ToList();
        var towels = input[0].Trim().Split(", ").AsReadOnly();
        var patterns = input[2 ..].Select(s => s.Trim());

        var part1 = patterns.Sum(p => CanMakePattern(towels, p));
        this.ShowDayResult(1, part1);

        Dictionary<string, long> cache = [];
        long part2 = patterns.Sum(p => PatternCombinations(towels, p, cache));
        this.ShowDayResult(2, part2);
    }

    /// <summary>
    /// This is the original fn for Part 1. Turns out not to be suitable for part 2 (surprise)
    /// and part 2 was so much more straightforward with a bit of recursion and a cache anyway.
    /// I could substitute the part 2 solution for this but why bother now.
    private int CanMakePattern(IReadOnlyCollection<string> towels, string pattern)
    {
        Queue<int> q = [];
        HashSet<int> matchedlengths = [];
        var ps = pattern.AsSpan();
        foreach (var towel in towels)
        {
            if (ps.StartsWith(towel))
                q.Enqueue(towel.Length);
        }
        var maxlen = towels.Max(t => t.Length);
        var plen = ps.Length;
        while (q.Count > 0)
        {
            var len = q.Dequeue();
            foreach (var towel in towels)
            {
                var newlen = len + towel.Length;
                if (newlen > plen || matchedlengths.Contains(newlen))
                    continue;
                if (ps[len..].StartsWith(towel))
                {
                    if (newlen == plen)
                        return 1;
                    q.Enqueue(newlen);
                    matchedlengths.Add(newlen);
                }
            }
        }
        return 0;
    }

    private long PatternCombinations(IReadOnlyCollection<string> towels, string pattern, Dictionary<string, long> cache)
    {
        if (cache.TryGetValue(pattern, out var result))
            return result;

        if (string.IsNullOrWhiteSpace(pattern))
            return 1;

        result = 0;
        foreach (var towel in towels)
        {
            if (pattern.StartsWith(towel))
            {
                result += PatternCombinations(towels, pattern[towel.Length..], cache);
            }
        }
        cache[pattern] = result;
        return result;
    }

    private const string TestInput = """
    r, wr, b, g, bwu, rb, gb, br

    brwrr
    bggr
    gbbr
    rrbgbr
    ubwu
    bwurrg
    brgr
    bbrgwb
    """;
}
