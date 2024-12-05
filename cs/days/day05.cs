namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/5 - Print Queue

public class Day05 : AocDaySolver
{
    public int DayNumber => 5;
    public string Title => "Print Queue";

    public async Task Solve()
    {
        var input = (await File.ReadAllTextAsync(AocUtils.FindInputFile(DayNumber))).Split("\n\n");
        //var input = TestInput.Split("\n\n");

        Dictionary<int, HashSet<int>> rules = new();
        input[0].Split('\n')
            .Select(r => new { Key = int.Parse(r.Split("|")[0]), Value = int.Parse(r.Split("|")[1]) })
            .ToList()
            .ForEach(r =>
            {
                if (!rules.ContainsKey(r.Key))
                    rules[r.Key] = [];
                rules[r.Key].Add(r.Value);

                if (!rules.ContainsKey(r.Value))
                    rules[r.Value] = [];
            });

        var printruns = input[1].Split('\n');

        int part1 = 0, part2 = 0;
        foreach (var run in printruns.Where(r => !string.IsNullOrWhiteSpace(r)))
        {
            var nums = run.Split(",").Select(int.Parse).ToList();
            var pass = true;
            foreach (var i in Enumerable.Range(0, nums.Count-1))
            {
                if (!rules.ContainsKey(nums[i]) || !rules[nums[i]].Contains(nums[i+1]))
                {
                    pass = false;
                    break;
                }
            }
            if (pass)
            {
                part1 += nums[(nums.Count-1) / 2];
            }
            else
            {
                var resorted = new List<int>(nums);
                resorted.Sort((a,b) => rules[a].Contains(b) ? -1 : 1);
                part2 += resorted[(resorted.Count-1) / 2];
            }
        }

        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
    }

    private const string TestInput = """
    47|53
    97|13
    97|61
    97|47
    75|29
    61|13
    75|53
    29|13
    97|29
    53|29
    61|53
    97|53
    61|29
    47|13
    75|47
    97|75
    47|61
    75|61
    47|29
    75|13
    53|13

    75,47,61,53,29
    97,61,53,29,13
    75,29,13
    75,97,47,61,53
    61,13,29
    97,13,75,29,47
    """;
}
