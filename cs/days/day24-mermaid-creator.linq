<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <RuntimeVersion>9.0</RuntimeVersion>
</Query>

async Task Main()
{
	var fn = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "../../input/day24-input");
	var input = (await File.ReadAllTextAsync(fn))
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

	var i = 0;
	StringBuilder sb = new();
	sb.AppendLine("graph TB");
	foreach (var (w,g) in gates)
	{
		var id = $"_{i:D3}";
		if (g.L[0] == 'y' || g.L[0] == 'x')
			sb.AppendLine($"    {g.L}(({g.L})) --> {g.Op}{id}");
		else
			sb.AppendLine($"    {g.L}{{{g.L}}} --> {g.Op}{id}");
		if (g.R[0] == 'y' || g.R[0] == 'x')
			sb.AppendLine($"    {g.R}(({g.R})) --> {g.Op}{id}");
		else
			sb.AppendLine($"    {g.R}{{{g.R}}} --> {g.Op}{id}");
		sb.AppendLine($"    {g.Op}{id} --> {w}");
		i++;
	}
	var merm = sb.ToString();
	merm.Dump();
}

private record Gate(string L, string R, string Op);
