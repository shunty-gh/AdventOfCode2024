namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/24 - Crossed Wires

public class Day24 : AocDaySolver
{
    public int DayNumber => 24;
    public string Title => "Crossed Wires";

    private record Gate(string L, string R, string Op);
    public async Task Solve()
    {
        var input = (await File.ReadAllTextAsync(AocUtils.FindInputFile(DayNumber)))
        //var input = (await Task.FromResult(TestInput.Trim())).Replace("\r", "")
            .Trim()
            .Split("\n\n");

        // Wires
        Dictionary<string, bool> wires = [];
        foreach (var w in input[0].Split('\n').Select(s => s.Trim()))
        {
            var sp = w.Split(": ");
            wires[sp[0]] = sp[1] == "1" ? true : false;
        }
        // Gates
        Dictionary<string, Gate> gates = [];
        foreach (var g in input[1].Split('\n').Select(s => s.Trim()))
        {
            var sp = g.Split(' ');
            var gate = new Gate(sp[0], sp[2], sp[1]);
            gates.Add(sp[4], gate);
        }

        this.ShowDayResult(1, CalculateZ(new(gates), new(wires)));
        var part2 = Part2(gates, wires);
        this.ShowDayResult(2, part2);
    }

    private static long CalculateZ(Dictionary<string, Gate> gates, Dictionary<string, bool> wires)
    {
        // Find and solve all z wires
        Dictionary<string, bool> zwires = [];
        foreach (var wire in gates.Keys.Where(k => k[0] == 'z'))
        {
            var g = SolveWire(wire, gates, wires);
            zwires.Add(wire, g);
        }
        return BitsValue(zwires);
    }

    private static bool SolveWire(string wire, IReadOnlyDictionary<string, Gate> gates, Dictionary<string, bool> wires)
    {
        if (wires.TryGetValue(wire, out var result))
            return result;

        var gate = gates[wire];
        if (!wires.TryGetValue(gate.L, out var gl))
            gl = SolveWire(gate.L, gates, wires);
        if (!wires.TryGetValue(gate.R, out var gr))
            gr = SolveWire(gate.R, gates, wires);
        result = gate.Op switch {
            "AND" => gl && gr,
            "OR"  => gl || gr,
            "XOR" => gl ^ gr,
            _ => throw new InvalidOperationException($"Unknown operation: {gate.Op}"),
        };
        wires.Add(wire, result);
        return result;
    }

    private string Part2(Dictionary<string, Gate> gates, Dictionary<string, bool> wires)
    {
        long xx = BitsValue(wires, 'x');
        long yy = BitsValue(wires, 'y');
        long expectedz = xx + yy;

        // Nearly: (string,string)[] swaps = [("z13","mks"), ("z19","dhf"), ("z25","mps"), ("vjv","cqm")];
        // Nearly: (string,string)[] swaps = [("z13","vcv"), ("z19","dhf"), ("z25","mps"), ("vjv","cqm")];
        (string,string)[] swaps = [("z13","vcv"), ("z19","vwp"), ("z25","mps"), ("vjv","cqm")];

        // Originals
        Dictionary<string, Gate> ggg = new (gates);
        Dictionary<string, bool> www = new (wires);
        foreach (var (sa, sb) in swaps)
        {
            var tmp = ggg[sa];
            ggg[sa] = ggg[sb];
            ggg[sb] = tmp;
        }
        var zzz = CalculateZ(ggg,www);
        string zzs = string.Join("", www.Where(kvp => kvp.Key[0] == 'z').OrderByDescending(kvp => kvp.Key)
            .Select(kvp => kvp.Value ? "1" : "0"));
        AnsiConsole.WriteLine($"Expected  : {expectedz} : {Convert.ToString(expectedz, 2)}");
        AnsiConsole.WriteLine($"Calculated: {zzz} : {zzs}");

        ggg = new (gates);
        www = new (wires);
        for (var j = 0; j < 45; j +=2)
        {
            www[$"x{j:D2}"] = true;
            www[$"x{(j+1):D2}"] = false;
        }
        xx = BitsValue(www, 'x');
        yy = BitsValue(www, 'y');
        expectedz = xx + yy;
        foreach (var (sa, sb) in swaps)
        {
            var tmp = ggg[sa];
            ggg[sa] = ggg[sb];
            ggg[sb] = tmp;
        }
        zzz = CalculateZ(ggg,www);
        zzs = string.Join("", www.Where(kvp => kvp.Key[0] == 'z').OrderByDescending(kvp => kvp.Key)
            .Select(kvp => kvp.Value ? "1" : "0"));
        AnsiConsole.WriteLine($"Expected  : {expectedz} : {Convert.ToString(expectedz, 2)}");
        AnsiConsole.WriteLine($"Calculated: {zzz} : {zzs}");


        ggg = new (gates);
        www = new (wires);
        for (var j = 0; j < 44; j +=2)
        {
            www[$"x{j:D2}"] = false;
            www[$"x{(j+1):D2}"] = true;
        }
        xx = BitsValue(www, 'x');
        yy = BitsValue(www, 'y');
        expectedz = xx + yy;
        foreach (var (sa, sb) in swaps)
        {
            var tmp = ggg[sa];
            ggg[sa] = ggg[sb];
            ggg[sb] = tmp;
        }
        zzz = CalculateZ(ggg,www);
        zzs = string.Join("", www.Where(kvp => kvp.Key[0] == 'z').OrderByDescending(kvp => kvp.Key)
            .Select(kvp => kvp.Value ? "1" : "0"));
        AnsiConsole.WriteLine($"Expected  : {expectedz} : {Convert.ToString(expectedz, 2)}");
        AnsiConsole.WriteLine($"Calculated: {zzz} : {zzs}");


        // Single 1 or 0
        for (var i = 0; i < 45; i++)
        {
            Dictionary<string, Gate> gg = new (gates);
            Dictionary<string, bool> ww = new (wires);
            foreach (var (w, _) in ww)
                ww[w] = false;
            foreach (var (w, _) in ww.Where(x => x.Key[0] == 'x'))
                ww[w] = true;
            ww[$"x{i:D2}"] = false;

            foreach (var (sa, sb) in swaps)
            {
                var tmp = gg[sa];
                gg[sa] = gg[sb];
                gg[sb] = tmp;
            }
            var zz = CalculateZ(gg,ww);
            string zs = string.Join("", ww.Where(kvp => kvp.Key[0] == 'z').OrderByDescending(kvp => kvp.Key)
                .Select(kvp => kvp.Value ? "1" : "0"));
            AnsiConsole.WriteLine($"{i:D2} => {zs}; {zz}");
        }

        // For my data - This leads to failures for x (or y) = 13,19,25,33
        // For each value find all gates involved and try swapping them
        // There is a way to do this in code but today we're going to do it
        // by manual inspection. Also look at the mermaid diagram generated by
        // the LINQPad script in this directory.

        // Get the answer string
        List<string> swaps2 = [];
        foreach (var (s1,s2) in swaps)
        {
            swaps2.Add(s1);
            swaps2.Add(s2);
        }
        swaps2.Sort();
        return string.Join(",", swaps2);
    }

    // Fail: cqm,mks,ngr,nmn,pqn,vbw,vjv,z13
    // Fail: cqm,dhf,mks,mps,vjv,z13,z19,z25
    // Fail: cqm,dhf,mps,vcv,vjv,z13,z19,z25
    // z13, mks
    // z19, dhf
    // z25, mps
    // vjv, cqm

    /* inspection of the data

    x33 XOR y33 -> vjv
    x33 AND y33 -> cqm
    vjv OR djh -> qnw
    cqm AND hgj -> djh
    khb OR snc -> hgj
    hgj XOR cqm -> z33
    vgm XOR qnw -> z34



    y25 & x25 -> vbw
    x25 ^ y25 -> pqn
    mqj XOR pqn -> mps
    mqj AND pqn -> qkk

    jnb AND mps -> hsw
    hjs OR fnw -> mqj
    vbw | qkk -> z25  <- should be xor?
    jnb XOR mps -> z26


    x19 & y19 -> ngr
    y19 ^ x19 -> nmn

    vwp | ngr -> dhf
    dhf & svm -> cfc
    csn ^ nmn -> vwp
    csn & nmn -> z19 <- should be xor?

    x20 & y20 -> pmc
    x20 ^ y20 -> svm
    svm ^ dhf -> z20
    dhf & svm -> cfc

    x13 AND y13 -> z13  <-  dubious
    y13 XOR x13 -> mks
    mks XOR bhr -> vcv
    bhr AND mks -> jpk
    jpk OR vcv -> ngh
    ngh AND kpj -> snn
    ngh XOR kpj -> z14
     */

    private static long BitsValue(Dictionary<string, bool> wires)
    {
        return Convert.ToInt64(
            string.Join("", wires.OrderByDescending(kvp => kvp.Key)
                                .Select(kvp => kvp.Value ? "1" : "0")), 2);
    }

    private static long BitsValue(Dictionary<string, bool> wires, char beginsWith)
    {
        return Convert.ToInt64(
            string.Join("", wires.Where(kvp => kvp.Key[0] == beginsWith).OrderByDescending(kvp => kvp.Key)
                                .Select(kvp => kvp.Value ? "1" : "0")), 2);
    }

    private const string TestInput = """
    x00: 1
    x01: 0
    x02: 1
    x03: 1
    x04: 0
    y00: 1
    y01: 1
    y02: 1
    y03: 1
    y04: 1

    ntg XOR fgs -> mjb
    y02 OR x01 -> tnw
    kwq OR kpj -> z05
    x00 OR x03 -> fst
    tgd XOR rvg -> z01
    vdt OR tnw -> bfw
    bfw AND frj -> z10
    ffh OR nrd -> bqk
    y00 AND y03 -> djm
    y03 OR y00 -> psh
    bqk OR frj -> z08
    tnw OR fst -> frj
    gnj AND tgd -> z11
    bfw XOR mjb -> z00
    x03 OR x00 -> vdt
    gnj AND wpb -> z02
    x04 AND y00 -> kjc
    djm OR pbm -> qhw
    nrd AND vdt -> hwm
    kjc AND fst -> rvg
    y04 OR y02 -> fgs
    y01 AND x02 -> pbm
    ntg OR kjc -> kwq
    psh XOR fgs -> tgd
    qhw XOR tgd -> z09
    pbm OR djm -> kpj
    x03 XOR y03 -> ffh
    x00 XOR y04 -> ntg
    bfw OR bqk -> z06
    nrd XOR fgs -> wpb
    frj XOR qhw -> z04
    bqk OR frj -> z07
    y03 OR x01 -> nrd
    hwm AND bqk -> z03
    tgd XOR rvg -> z12
    tnw OR pbm -> gnj
    """;
}
