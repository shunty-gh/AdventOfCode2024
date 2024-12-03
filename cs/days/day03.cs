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
        var part1 = matchesP1.Sum(GetMulValue);
        this.ShowDayResult(1, part1);

        var ok = true;
        var part2 = 0L;
        var matchesP2 = reP2.Matches(input);
        foreach (Match m in matchesP2)
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
                part2 += GetMulValue(m);
            }
        }
        this.ShowDayResult(2, part2);
    }

    private int GetMulValue(Match match)
    {
        var mm = reInner.Matches(match.Value);
        return int.Parse(mm[0].Groups[1].Value) * int.Parse(mm[0].Groups[2].Value);
    }
}
