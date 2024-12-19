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
        var towels = input[0].Trim().Split(", ").ToHashSet();
        var patterns = input[2 ..].Select(s => s.Trim());

        int part1 = 0, part2 = 0;
        foreach (var pattern in patterns)
        {
            part1 += CanMakePattern(towels, pattern) ? 1 : 0;
        }
        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
    }

    private bool CanMakePattern(IReadOnlySet<string> towels, string pattern)
    {
        Queue<int> q = [];
        HashSet<int> matchedlengths = [];
        foreach (var towel in towels)
        {
            if (pattern.StartsWith(towel))
            {
                q.Enqueue(towel.Length);
            }
        }
        var maxlen = towels.Max(t => t.Length);
        var plen = pattern.Length;
        while (q.Count > 0)
        {
            var len = q.Dequeue();
            var tomatch = len+maxlen >= plen
                ? pattern[len..].AsSpan()
                : pattern[len..(len+maxlen)].AsSpan();
            foreach (var towel in towels)
            {
                var newlen = len + towel.Length;
                if (newlen > plen || matchedlengths.Contains(newlen))
                    continue;
                if (tomatch.StartsWith(towel))
                {
                    if (newlen == plen)
                        return true;
                    q.Enqueue(len + towel.Length);
                    matchedlengths.Add(newlen);
                }
            }
        }
        return false;
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
