namespace AoC_2021;

public class Day_01 : BaseDay
{
    private readonly int[] _input;

    public Day_01()
    {
        _input = ParseInput();
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
        const int n = 3;

        var result = 0;
        var lastDepth = _input[..n].Sum();

        for (int i = n; i < _input.Length; ++i)
        {
            var depth = lastDepth - _input[i - n] + _input[i];
            if (depth > lastDepth)
            {
                ++result;
            }
            lastDepth = depth;
        }

        return new(result.ToString());
    }

    private int[] ParseInput()
    {
        return File.ReadAllLines(InputFilePath).Select(int.Parse).ToArray();
    }
}