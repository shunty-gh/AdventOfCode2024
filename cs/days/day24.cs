namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/24 - Crossed Wires

public class Day24 : AocDaySolver
{
    public int DayNumber => 24;
    public string Title => "Crossed Wires";

    private record Gate(string L, string R, string Op, string Out);
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
            var gate = new Gate(sp[0], sp[2], sp[1], sp[4]);
            gates.Add(gate.Out, gate);
        }

        // Find and solve all z wires
        Dictionary<string, bool> zwires = [];
        foreach (var wire in gates.Keys.Where(k => k[0] == 'z'))
        {
            var g = SolveGate(gates[wire], gates, wires);
            zwires.Add(wire, g);
        }

        var part1 = Convert.ToInt64(
            string.Join("", zwires.OrderByDescending(kvp => kvp.Key)
                                .Select(kvp => kvp.Value ? "1" : "0")), 2);
        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, gates.Count);
    }

    private bool SolveGate(Gate gate, IReadOnlyDictionary<string, Gate> gates, Dictionary<string, bool> wires)
    {
        if (wires.TryGetValue(gate.Out, out var result))
            return result;

        if (!wires.TryGetValue(gate.L, out var gl))
            gl = SolveGate(gates[gate.L], gates, wires);
        if (!wires.TryGetValue(gate.R, out var gr))
            gr = SolveGate(gates[gate.R], gates, wires);
        result = gate.Op switch {
            "AND" => gl && gr,
            "OR"  => gl || gr,
            "XOR" => gl ^ gr,
            _ => throw new InvalidOperationException($"Unknown operation: {gate.Op}"),
        };
        wires.Add(gate.Out, result);
        return result;
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
