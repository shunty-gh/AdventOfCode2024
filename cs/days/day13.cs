using System.Text.RegularExpressions;

namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/13 - Claw Contraption

public class Day13 : AocDaySolver
{
    private record Button(int X, int Y);
    private record Prize(long X, long Y);

    public int DayNumber => 13;
    public string Title => "Claw Contraption";

    public async Task Solve()
    {
        var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToImmutableList();
        //var input = (await Task.FromResult(TestInput)).Split('\n').Select(s => s.Trim()).ToImmutableList();

        long part1 = 0, part2 = 0;
        for (var i = 0; i < input.Count; i++)
        {
            var btna = ReadButtonLine(input[i]);
            var btnb = ReadButtonLine(input[i+1]);
            var prize = ReadPrizeLine(input[i+2]);
            i += 3;

            var p1 = FindCheapest(btna, btnb, prize);
            if (p1 > 0)
                part1 += p1;
        }

        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
    }

    private long FindCheapest(Button btnA, Button btnB, Prize prize)
    {
        var amax = Math.Max(prize.X / btnA.X, prize.Y / btnA.Y);
        var amin = Math.Min(prize.X / btnA.X, prize.X / btnB.X);
        var apresses = amax;
        long cheapest = -1;
        while (apresses >= 0)
        {
            var remx = prize.X - (apresses * btnA.X);
            var remy = prize.Y - (apresses * btnA.Y);
            if (remx < 0 || remy < 0)
            {
                apresses -= 1;
                continue;
            }

            if (remx % btnB.X == 0)
            {
                var bpresses = remx / btnB.X;
                if (bpresses * btnB.Y == remy)
                {
                    var cost = apresses * 3 + bpresses;
                    if (cheapest < 0 || cost < cheapest)
                        cheapest = cost;
                }
            }
            apresses -= 1;
        }
        return cheapest;
    }

    private static string ButtonPattern = @"Button [AB]: X\+(?<xinc>\d+), Y\+(?<yinc>\d+)";
    private static string PrizePattern = @"Prize: X=(?<xprize>\d+), Y=(?<yprize>\d+)";
    private static Regex ButtonRe = new Regex(ButtonPattern);
    private static Regex PrizeRe = new Regex(PrizePattern);
    private Button ReadButtonLine(string line)
    {
        var match = ButtonRe.Match(line);
        var x = int.Parse(match.Groups["xinc"].Value);
        var y = int.Parse(match.Groups["yinc"].Value);
        return new Button(x, y);
    }

    private Prize ReadPrizeLine(string line)
    {
        var match = PrizeRe.Match(line);
        var x = int.Parse(match.Groups["xprize"].Value);
        var y = int.Parse(match.Groups["yprize"].Value);
        return new Prize(x, y);
    }

    // Expect P1 = 1930; P2 = 1206
    private const string TestInput = """
    Button A: X+94, Y+34
    Button B: X+22, Y+67
    Prize: X=8400, Y=5400

    Button A: X+26, Y+66
    Button B: X+67, Y+21
    Prize: X=12748, Y=12176

    Button A: X+17, Y+86
    Button B: X+84, Y+37
    Prize: X=7870, Y=6450

    Button A: X+69, Y+23
    Button B: X+27, Y+71
    Prize: X=18641, Y=10279
    """;
}
