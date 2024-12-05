namespace Shunty.AoC.Days;

// A basic day solver template

// https://adventofcode.com/2024/day/ -

public class DayXX : AocDaySolver
{
    public int DayNumber => 0;
    public string Title => "";

    public async Task Solve()
    {
        //var input = (await File.ReadAllLinesAsync(AocUtils.FindInputFile(DayNumber))).ToImmutableList();
        //var input = (await File.ReadAllTextAsync(AocUtils.FindInputFile(DayNumber))).Split("\n\n");
        //var input = TestData.Split('\n').ToList();

        int part1 = 0, part2 = 0;
        this.ShowDayResult(1, part1);
        this.ShowDayResult(2, part2);
    }

    private const string TestInput = """
    12345
    """;
}
