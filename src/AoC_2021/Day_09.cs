using SheepTools.Model;

namespace AoC_2021;

public class Day_09 : BaseDay
{
    private readonly List<List<int>> _input;

    public Day_09()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        var result = 0;
        List<(int X, int Y)> lowPoints = new();

        for (int y = 0; y < _input.Count; ++y)
        {
            for (int x = 0; x < _input[y].Count; ++x)
            {
                var value = _input[y][x];

                for (int y1 = y - 1; y1 <= y + 1; ++y1)
                {
                    if (y1 < 0 || y1 > _input.Count - 1)
                    {
                        continue;
                    }
                    for (int x1 = x - 1; x1 <= x + 1; ++x1)
                    {
                        if (x1 < 0 || x1 > _input[y].Count - 1 || (x1 == x && y1 == y))
                        {
                            continue;
                        }

                        if (_input[y1][x1] < value)
                        {
                            goto endOfDoubleLoop;
                        }
                    }
                }

                lowPoints.Add((x, y));
            endOfDoubleLoop:;
            }
        }

        return new($"{lowPoints.Sum(point => _input[point.Y][point.X]) + lowPoints.Count}");
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private List<List<int>> ParseInput()
    {
        return File.ReadAllLines(InputFilePath).Select(str => str.Select(ch => int.Parse(ch.ToString())).ToList()).ToList();
    }
}