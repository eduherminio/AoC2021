using SheepTools.Model;

namespace AoC_2021;

public record DirectionCommnad(Direction Direction, int Value);

public class Day_02 : BaseDay
{
    private readonly List<DirectionCommnad> _input;

    public Day_02()
    {
        _input = ParseInput();
        //_input = ParseInput2();
    }

    public override ValueTask<string> Solve_1()
    {
        var position = new Point(0, 0);

        foreach (var direction in _input)
        {
            position = position.Move(direction.Direction, direction.Value);
        }

        return new(Math.Abs(position.X * position.Y).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var aim = 0;
        var position = new Point(0, 0);

        foreach (var direction in _input)
        {
            switch (direction.Direction)
            {
                case Direction.Down:
                    aim += direction.Value;
                    break;
                case Direction.Up:
                    aim -= direction.Value;
                    break;
                case Direction.Right:
                    position = position.Move(direction.Direction, direction.Value);
                    position.Y += (direction.Value * aim);
                    break;
                default:
                    throw new();
            }
        }

        return new((position.X * position.Y).ToString());
    }

    private List<DirectionCommnad> ParseInput()
    {
        static Direction ParseDirection(string str)
        {
            return str switch
            {
                "forward" => Direction.Right,
                "backward" => Direction.Left,
                "up" => Direction.Up,
                "down" => Direction.Down,
                _ => throw new()
            };
        }

        var file = new ParsedFile(InputFilePath);

        var result = new List<DirectionCommnad>(2 * file.Count);

        while (!file.Empty)
        {
            var line = file.NextLine();
            result.Add(new(ParseDirection(line.NextElement<string>()), line.NextElement<int>()));
        }

        return result;
    }
}