
using System.Text.RegularExpressions;

namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/3 - Mull It Over

public class Day03 : AocDaySolver
{
    public int DayNumber => 3;
    public string Title => "Mull It Over";

    private const string patternP1 = @"mul\(\d+,\d+\)";
   	private const string patternP2 = @"mul\(\d+,\d+\)|do\(\)|don\'t\(\)";
    private const string patternInner = @"mul\((\d+),(\d+)\)";

    private readonly Regex reP1 = new(patternP1, RegexOptions.Compiled);
    private readonly Regex reP2 = new(patternP2, RegexOptions.Compiled);
    private readonly Regex reInner = new(patternInner, RegexOptions.Compiled);

    public async Task Solve()
    {
        var input = string.Join("", await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber)));
        var matchesP1 = reP1.Matches(input);

        var part1 = 0L;
        foreach (var m in matchesP1.ToList())
        {
            var mm = reInner.Matches(m.Value).ToList();
            part1 += int.Parse(mm[0].Groups[1].Value) * int.Parse(mm[0].Groups[2].Value);
        }
        this.ShowDayResult(1, part1);

        var ok = true;
        var part2 = 0L;
        var matchesP2 = reP2.Matches(input);
        foreach (var m in matchesP2.ToList())
        {
            if (m.Value == "don't()")
            {
                ok = false;
            }
            else if (m.Value == "do()")
            {
                ok = true;
            }
            else if (ok)
            {
                var mm = reInner.Matches(m.Value).ToList();
                part2 += int.Parse(mm[0].Groups[1].Value) * int.Parse(mm[0].Groups[2].Value);
            }
        }
        this.ShowDayResult(2, part2);
    }
}
