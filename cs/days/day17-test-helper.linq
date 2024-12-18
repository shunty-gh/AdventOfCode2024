<Query Kind="Program">
  <Namespace>System.Numerics</Namespace>
</Query>

void Main()
{
	long prev = 0;
	List<(long, long)> loops = [];
	for (var a = 10_000_000; a < 30_000_000; a++)
	{
		long b1 = RunLoop(a);
		var a2 = a / 8;
		long b2 = RunLoop(a2);
		var a3 = a2 / 8;
		long b3 = RunLoop(a3);
		var a4 = a3 / 8;
		long b4 = RunLoop(a4);
		var a5 = a4 / 8;
		long b5 = RunLoop(a5);
		var a6 = a5 / 8;
		long b6 = RunLoop(a6);
		if (b1 == 2 && b2 == 4 && b3 == 1 && b4 == 2 && b5 == 7 && b6 == 5)
		{
			loops.Add((a, a-prev));
			//Console.WriteLine($"{a:D2} => {b1},{b2},{b3},{b4},{b5},{b6};  Diff: {a - prev}");
			prev = a;
		}		
	}
	
	var looplen = 13;
	loops.DumpTell();
	loops[^(looplen+1)].Item1.DumpTell();
	loops[^looplen..].Select(ll => ll.Item2).DumpTell();
	
	var rep = FindRepeatingSequence(loops.Select(ll => ll.Item2).Reverse().ToList());
	rep.DumpTell();
}

public long RunLoop(long a)
{
	long b = a % 8;
	b = b ^ 2;
	long c = a / (long)Math.Pow(2, b);
	b = b ^ c;
	b = b ^ 3;
	return b % 8;
}

public static (int StartIndex, int SequenceLength) FindRepeatingSequence<T>(List<T> source, int minRequiredSeqLength = 2) where T : INumber<T>
{
	for (var i = 0; i < source.Count; i++)
	{
		for (var j = i + minRequiredSeqLength; j < source.Count; j++)
		{
			// Find a matching load value
			if (source[i] == source[j])
			{
				// Is the sequence to get there the same?
				var rlen = j - i;
				if (j + rlen >= source.Count)
					break;
				if (source[i..j].SequenceEqual(source[j..(j + rlen)]))
				{
					// If we can, then make sure the next sequence also matches just for good measure
					if (j + rlen + rlen >= source.Count || source[i..j].SequenceEqual(source[(j + rlen)..(j + rlen + rlen)]))
					{
						//Console.WriteLine($"Found range starting at i={i} for {j - i} elements");
						return (i, j - i);
					}
				}
			}
		}
	}
	return (-1, -1);
}
