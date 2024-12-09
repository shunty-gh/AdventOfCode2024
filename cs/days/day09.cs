namespace Shunty.AoC.Days;

// https://adventofcode.com/2024/day/9 - Disk Fragmenter

public class Day09 : AocDaySolver
{
    public int DayNumber => 9;
    public string Title => "Disk Fragmenter";

    private record FsInfo(int Id, int Len, int Space);

    public async Task Solve()
    {
        var input = (await File.ReadAllTextAsync(AocUtils.FindInputFile(DayNumber))).Trim();
        //var input = TestInput.Trim();

        var slen = input.Length;
        var fs = new List<FsInfo>();
        var fid = 0;
        var sp = 0;
        while (sp < slen)
        {
            var flen = (int)(input[sp] - '0');
            var space = sp < slen-1 ? (int)(input[sp+1] - '0') : 0;
            fs.Add(new(fid, flen, space));

            fid += 1;
            sp += 2;
        }

        var fsP1 = Defrag1(fs);
        this.ShowDayResult(1, Checksum(fsP1));

        var fsP2 = Defrag2(fs);
        this.ShowDayResult(2, Checksum(fsP2));
    }

    private static List<FsInfo> Defrag1(IReadOnlyList<FsInfo> fs)
    {
        var rp = 0;
        var ep = fs.Count - 1;
        var result = new List<FsInfo>();
        FsInfo tomove = fs[ep];
        while (rp < ep)
        {
            var curr = fs[rp];
            result.Add(new(curr.Id, curr.Len, 0));
            var tofill = curr.Space;

            while (tofill > 0)
            {
                if (tomove.Len > tofill)
                {
                    result.Add(new(tomove.Id, tofill, 0));
                    tomove = new(tomove.Id, tomove.Len - tofill, 0);
                    tofill = 0;
                }
                else
                {
                    result.Add(new(tomove.Id, tomove.Len, 0));
                    tofill -= tomove.Len;
                    ep -= 1;
                    tomove = fs[ep];
                }
            }
            rp += 1;
        }
        if (tomove.Len > 0)
        {
            result.Add(new(tomove.Id, tomove.Len, 0));
        }
        return result;
    }

    /// <summary>
    /// 'defrag' in-place by moving lower placed files into space
    /// available 'higher' up the disk - but only if there's at least
    /// enough space for the whole file.
    /// </summary>
    private static List<FsInfo> Defrag2(IReadOnlyList<FsInfo> fs)
    {
        var ep = fs.Count - 1;
        var result = new List<FsInfo>(fs);

        while (ep > 0)
        {
            var moved = false;
            var tomove = result[ep];
            // Find free space for it
            for (var i = 0; i < ep; i++)
            {
                var curr = result[i];
                if (curr.Space >= tomove.Len)
                {
                    // Insert a copy in the free space. Update the available free space for both the existing and added items.
                    // The existing item will have 0 free space as the newly inserted item will be right next to it.
                    result.Insert(i+1, tomove with { Space = curr.Space - tomove.Len });
                    result[i] = curr with { Space = 0 };

                    // We're going to create a hole at ep+1 so add the extra space this will create to the previous file info
                    result[ep] = result[ep] with { Space = result[ep].Space + tomove.Len + tomove.Space };
                    result.RemoveAt(ep+1); // +1 'cos we've, temporarily, added a new one
                    moved = true;
                    break;
                }
            }
            if (!moved)
                ep -= 1;
        }
        //result.ForEach(f => Console.WriteLine($"{f.Id}, {f.Len}, {f.Space}"));
        return result;
    }

    private static long Checksum(IReadOnlyList<FsInfo> fs)
    {
        long result = 0;
        var idx = 0;
        foreach (var f in fs)
        {
            for (var i = 0; i < f.Len; i++, idx++)
            {
                result += f.Id * idx;
            }
            // Add the space (0 in part 1 but significant in part 2)
            idx += f.Space;
        }
        return result;
    }

    // Test data. Expect P1: 1928; P2: 2858
    private const string TestInput = """
    2333133121414131402
    """;
}
