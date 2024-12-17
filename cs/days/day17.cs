namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/17 - Chronospatial Computer

public class Day17 : AocDaySolver
{
    public int DayNumber => 17;
    public string Title => "Chronospatial Computer";

    public async Task Solve()
    {
        var input = (await File.ReadAllTextAsync(AocUtils.FindInputFile(DayNumber))).Split("\n\n");
        //var input = (await Task.FromResult(TestInput2.Trim())).Replace("\r", "").Split("\n\n").ToImmutableList();

        List<long> registers = [];
        foreach (var line in input[0].Trim().Split('\n'))
            registers.Add(long.Parse(line[12..]));
        var program = input[1].Trim()[9..].Split(',').Select(s => int.Parse(s)).ToImmutableList();


        var (_, part1) = RunProgram(program, registers);
        this.ShowDayResult(1, part1);

        /* Part 2
           Brute forcing this is never going to work. Finding a sequence of 8, 9 or 10
           is doable but getting 16 is a looong stretch. Therefore find a shorter sequence
           then use that as the basis for finding the full result.

           Run the 'program' and try to match the first 6 numbers from the target using
           increasing values of A. Keep going for a lot of different values of A.
           This will eventually yield a collection of start values for A that all give
           the correct first 6 digits. If we have tried enough values then a pattern will
           emerge of a repeating set of increments between each starting value.
           In order to match the full target string our starting value for A must at least
           be our starting guess plus a multiple of one of the repeating differences

           Finding the repeating loop is a bit of trial and error. But, for the first 6
           elements, checking a range of 10_000_000 to 30_000_000 turns out to be acceptable.
         */

        List<(long, long)> loopcheck = [];
        long prev = 0;
        string target = input[1].Trim()[9..20]; // First 6 elements
        for (var a = 10_000_000 ; a < 30_000_000; a++)
        {
            var (res, _) = RunProgram(program, registers, true, a, target);
            if (res)
            {
                loopcheck.Add((a, a - prev));
                prev = a;
            }
        }
        // Debug: Look at this list to find the repeating sequence of differences. In my case the loop length is 13.
        //loopcheck.ForEach(d => Console.WriteLine($"Start: {d.Item1,16:D};   Diff: {d.Item2,10:D}"));


        // Now use those diffs to find the real solution

        // The loop length is found by going back through the list and finding the
        // point at which it repeats.
        var (_, looplen) = loopcheck
            .Select(ll => ll.Item2)
            .Reverse()
            .ToList()
            .FindRepeatingSequence();
        long part2 = loopcheck[^(looplen+1)].Item1; // The value of A we should start from
        var diffs = loopcheck[^looplen..].Select(ll => ll.Item2); // The amounts to increment A by
        // From analysis of the code it is apparent that we will need to set register A
        // to at least 8^15 in order to generate a 16 element result. Therefore don't bother
        // checking until we get to that minimum.
        long p2min = (long)Math.Pow(8, 15);

        bool found = false;
        target = input[1].Trim()[9..];
        while (!found)
        {
            foreach (var diff in diffs)
            {
                part2 += diff;
                if (part2 < p2min)
                    continue;

                var (res, _) = RunProgram(program, registers, true, part2, target);
                if (res)
                {
                    found = true;
                    break;
                }
            }
        }
        this.ShowDayResult(2, part2);
    }

    private static (bool, string) RunProgram(IReadOnlyList<int> program, IReadOnlyList<long> registers, bool part2 = false, long registerA = 0, string target = "")
    {
        List<long> reg = [.. registers];
        if (part2) reg[0] = registerA;

        long GetOperandValue(int op) => op <= 3 ? op : reg[op - 4];
        long Pow(long op) => 1 << (int)op;

        int ip = 0;
        List<int> output = [];
        while (true)
        {
            if (ip >= program.Count)
                break;

            var opcode = program[ip];
            var oper = program[ip+1];

            var jnz = false;
            switch (opcode)
            {
                case 0:
                    reg[0] = reg[0] / Pow(GetOperandValue(oper));
                    break;
                case 1:
                    reg[1] = reg[1] ^ oper;
                    break;
                case 2:
                    reg[1] = GetOperandValue(oper) % 8;
                    break;
                case 3:
                    jnz = reg[0] != 0;
                    break;
                case 4:
                    reg[1] = reg[1] ^ reg[2];
                    break;
                case 5:
                    output.Add((int)(GetOperandValue(oper) % 8));
                    // Exit early! Check it matches the target so far.
                    var s = string.Join(',', output);
                    if (part2 && !target.StartsWith(s))
                        return (false, "");
                    else if (s == target)
                        return (true, ""); // We don't need to pass the target back. The caller will already know what it is.
                    //else if (output.Count > 9)
                    //    Console.WriteLine($"M Len: {output.Count} ; Start: {registerA}");
                    break;
                case 6:
                    reg[1] = reg[0] / Pow(GetOperandValue(oper));
                    break;
                case 7:
                    reg[2] = reg[0] / Pow(GetOperandValue(oper));
                    break;
                default:
                    throw new InvalidOperationException($"Unknown opcode {opcode}");
            }
            ip = jnz ? oper : ip + 2;
        }

        return (true, string.Join(',', output));
    }

    private const string TestInput = """
    Register A: 729
    Register B: 0
    Register C: 0

    Program: 0,1,5,4,3,0
    """;

    private const string TestInput2 = """
    Register A: 2024
    Register B: 0
    Register C: 0

    Program: 0,3,5,4,3,0
    """;
}
