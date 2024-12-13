using System.Text.RegularExpressions;

namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/13 - Claw Contraption

public partial class Day13 : AocDaySolver
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

            part1 += FindCheapest(btna, btnb, prize);
            part2 += FindCheapest(btna, btnb, prize, 10_000_000_000_000);
        }

        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
    }

    private long FindCheapest(Button btnA, Button btnB, Prize prize, long offset = 0)
    {
        /*
           We have 2 simultaneous equations
           A * btnA.x + B * btnB.x = prize.x
           A * btnA.y + B * btnB.y = prize.y

           We need to solve for A and B

           | btnA.x  btnB.x | | A | = | prize.x |
           | btnA.y  btnB.y | | B |   | prize.y |

           Using Cramers rule https://en.wikipedia.org/wiki/Cramer%27s_rule
           we get

           det(org) = btnA.x * btnB.y - btnB.x * btnA.y
           det(A)   = prize.x * btnB.y - btnB.x * prize.y
           det(B)   = btnA.x * prize.y - prize.x * btnA.y

           A = det(A) / det(org)
           B = det(B) / det(org)
        */

        Prize prz = new(prize.X + offset, prize.Y + offset);
        long det0 = btnA.X * btnB.Y - btnB.X * btnA.Y;
        long detA = prz.X * btnB.Y - btnB.X * prz.Y;
        long detB = btnA.X * prz.Y - prz.X * btnA.Y;

        // We can only have +ve, whole numbers of presses
        if ((det0 > 0 && (detA < 0 || detB < 0))
          || (det0 < 0 && (detA > 0 || detB > 0))
          || detA % det0 != 0 || detB % det0 != 0)
            return 0;

        long a = detA / det0;
        long b = detB / det0;

        return 3 * a + b;
    }

    // The brute force approach for part 1, which was never going to work for part 2.
    // private long FindCheapest(Button btnA, Button btnB, Prize prize)
    // {
    //     Prize prz = new(prize.X + offset, prize.Y + offset);
    //     var amax = Math.Max(prz.X / btnA.X, prz.Y / btnA.Y);
    //     var apresses = amax;
    //     long cheapest = 0;
    //     while (apresses >= 0)
    //     {
    //         var remx = prz.X - (apresses * btnA.X);
    //         var remy = prz.Y - (apresses * btnA.Y);
    //         if (remx < 0 || remy < 0)
    //         {
    //             apresses -= 1;
    //             continue;
    //         }

    //         if (remx % btnB.X == 0)
    //         {
    //             var bpresses = remx / btnB.X;
    //             if (bpresses * btnB.Y == remy)
    //             {
    //                 var cost = apresses * 3 + bpresses;
    //                 if (cheapest <= 0 || cost < cheapest)
    //                     cheapest = cost;
    //             }
    //         }
    //         apresses -= 1;
    //     }
    //     return cheapest;
    // }

    private const string ButtonPattern = @"Button [AB]: X\+(?<xinc>\d+), Y\+(?<yinc>\d+)";
    private const string PrizePattern = @"Prize: X=(?<xprize>\d+), Y=(?<yprize>\d+)";

    [GeneratedRegex(ButtonPattern)]
    private static partial Regex ButtonRe();

    [GeneratedRegex(PrizePattern)]
    private static partial Regex PrizeRe();

    private static Button ReadButtonLine(string line)
    {
        var match = ButtonRe().Match(line);
        var x = int.Parse(match.Groups["xinc"].Value);
        var y = int.Parse(match.Groups["yinc"].Value);
        return new Button(x, y);
    }

    private static Prize ReadPrizeLine(string line)
    {
        var match = PrizeRe().Match(line);
        var x = int.Parse(match.Groups["xprize"].Value);
        var y = int.Parse(match.Groups["yprize"].Value);
        return new Prize(x, y);
    }

    // Expect P1: 480
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
