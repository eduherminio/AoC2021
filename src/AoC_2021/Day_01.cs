namespace AoC_2021;

public class Day_01 : BaseDay
{
    private readonly List<int> _input;
    //private readonly List<List<int>> _input;

    public Day_01()
    {
        _input = ParseInput();
        //_input = ParseInput2();
    }

    public override ValueTask<string> Solve_1()
    {
        var result = 0;
        var lastDepth = int.MaxValue;
        foreach (var depth in _input)
        {
            if (depth > lastDepth)
            {
                ++result;
            }
            lastDepth = depth;
        }

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var result = 0;
        var lastDepth = _input[0] + _input[1] + _input[2];
        for (int i = 3; i < _input.Count; ++i)
        {
            var depth = lastDepth - _input[i - 3] + _input[i];
            if (depth > lastDepth)
            {
                ++result;
            }
            lastDepth = depth;
        }

        return new(result.ToString());
    }

    private List<int> ParseInput()
    {
        return File.ReadAllLines(InputFilePath).Select(int.Parse).ToList();
    }

    private List<List<int>> ParseInput2()
    {
        var file = new ParsedFile(InputFilePath);

        var result = new List<List<int>>(file.Count);

        while (!file.Empty)
        {
            result.Add(new());
            var line = file.NextLine();

            while (!line.Empty)
            {
                var n = line.NextElement<int>();
                result.Last().Add(n);
            }
        }

        return result;
    }
}