namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/23 - LAN Party

public class Day23 : AocDaySolver
{
    public int DayNumber => 23;
    public string Title => "LAN Party";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber)))
        //var input = (await Task.FromResult(TestInput.Trim())).Split('\n').Select(s => s.Trim())
            .Select(ln => ln.Split('-'))
            .Select(s => (s[0], s[1]))
            .ToList();

        Dictionary<string, HashSet<string>> lan = [];
        foreach (var (n1, n2) in input)
        {
            if (lan.TryGetValue(n1, out var nn1))
                nn1.Add(n2);
            else
                lan.Add(n1, [n2]);

            if (lan.TryGetValue(n2, out var nn2))
                nn2.Add(n1);
            else
                lan.Add(n2, [n1]);
        }


        HashSet<(string,string,string)> tgroups = [];
        Dictionary<string, int> lanparty = [];
        foreach (var (k1,v) in lan)
        {
            foreach (var k2 in v)
            {
                var common = v.Intersect(lan[k2]).ToList();
                if (common.Count > 0)
                {
                    // Part 2
                    if (common.Count > 1)
                    {
                        List<string> lp = [k1, k2, .. common];
                        lp.Sort();
                        var lpkey = string.Join(",", lp);
                        if (lanparty.TryGetValue(lpkey, out var lpc))
                            lanparty[lpkey] = lpc+1;
                        else
                            lanparty[lpkey] = 1;
                    }

                    // Part 1
                    foreach (var k3 in common)
                    {
                        List<string> sorted = [k1, k2, k3];
                        sorted.Sort();
                        if (sorted.Any(x => x[0] == 't'))
                            tgroups.Add((sorted[0], sorted[1], sorted[2]));

                    }
                }
            }
        }

        this.ShowDayResult(1, tgroups.Count);
        var p2max = lanparty.Max(x => x.Value);
        this.ShowDayResult(2, lanparty.First(kvp => kvp.Value == p2max).Key);
    }

    private const string TestInput = """
    kh-tc
    qp-kh
    de-cg
    ka-co
    yn-aq
    qp-ub
    cg-tb
    vc-aq
    tb-ka
    wh-tc
    yn-cg
    kh-ub
    ta-co
    de-co
    tc-td
    tb-wq
    wh-td
    ta-ka
    td-qp
    aq-cg
    wq-ub
    ub-vc
    de-ta
    wq-aq
    wq-vc
    wh-yn
    ka-de
    kh-ta
    co-tc
    wh-qp
    tb-vc
    td-yn
    """;
}
