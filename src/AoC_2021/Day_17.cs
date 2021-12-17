using SheepTools;
using SheepTools.Model;

namespace AoC_2021;

public class Day_17 : BaseDay
{
    private record TargetArea(List<IntPoint> AreaPoint, int MinX, int MaxX, int MinY, int MaxY);

    private readonly Dictionary<IntPoint, int> _validVelocities;

    public Day_17()
    {
        _validVelocities = CalculateValidVelocities(ParseInput());
    }

    public override ValueTask<string> Solve_1()
    {
        return new($"{_validVelocities.Max(p => p.Value)}");
    }

    public override ValueTask<string> Solve_2()
    {
        return new($"{_validVelocities.Count()}");
    }

    private TargetArea ParseInput()
    {
        var targetArea = new List<IntPoint>();

        var rawInput = File.ReadAllText(InputFilePath);

        var parts = rawInput.Split(',');

        var xStr = parts[0].Split(' ').Last().Replace("x=", string.Empty);
        var yStr = parts[1].Trim().Split(' ').First().Replace("y=", string.Empty);

        var xParts = xStr.Split("..");
        var yParts = yStr.Split("..");

        var xMin = int.Parse(xParts[0]);
        var xMax = int.Parse(xParts[1]);
        var yMin = int.Parse(yParts[0]);
        var yMax = int.Parse(yParts[1]);

        for (int y = yMin; y <= yMax; ++y)
        {
            for (int x = xMin; x <= xMax; ++x)
            {
                targetArea.Add(new IntPoint(x, y));
            }
        }

        return new TargetArea(targetArea, xMin, xMax, yMin, yMax);
    }

    private static Dictionary<IntPoint, int> CalculateValidVelocities(TargetArea input)
    {
        Dictionary<IntPoint, int> validVelocities = new();

        var origin = new IntPoint(0, 0);

        int maxVx = input.MaxX;
        int minVx = CalculateMinVx(input);

        var xValues = RangeHelpers.GenerateRange(minVx, maxVx);

        foreach (var x in xValues)
        {
            int y = input.MinY;

            while (true)
            {
                int maxY = 0;
                bool breakY = false;
                bool validVelocity = false;
                var currentPoint = origin;
                var currentVelocity = new IntPoint(x, y);

                int steps = 0;
                while (true)
                {
                    (currentPoint, currentVelocity) = Move(currentPoint, currentVelocity);
                    if (currentPoint.Y > maxY)
                    {
                        maxY = currentPoint.Y;
                    }

                    if (currentPoint.Y <= input.MaxY && currentPoint.X >= input.MinX)
                    {
                        if (input.AreaPoint.Contains(currentPoint))
                        {
                            validVelocity = true;
                            break;
                        }
                    }

                    // Too fast to stop in the target area.
                    if (currentPoint.Y == 0 && currentVelocity.Y < 0 && currentVelocity.Y < input.MinY)
                    {
                        breakY = true;
                        break;
                    }

                    if (currentPoint.Y < input.MinY || currentPoint.X > input.MaxX)
                    {
                        // Still going up when x > maxX.
                        // If currentVelocity.X == 0, this will never be hit, therefore we need the other condition above
                        if (currentPoint.X > input.MaxX && currentVelocity.Y > 0)
                        {
                            breakY = true;
                        }
                        break;
                    }
                    ++steps;
                }

                if (validVelocity)
                {
                    validVelocities.Add(new(x, y), maxY);
                }

                if (breakY)
                {
                    break;
                }

                ++y;
            }
        }

        return validVelocities;
    }

    private static int CalculateMinVx(TargetArea input)
    {
        int minVx = 0;

        for (int vx = input.MinX; vx > 0; --vx)
        {
            bool valid = true;
            var position = new IntPoint(0, 0);
            var velocity = new IntPoint(vx, 0);
            while (true)
            {
                (position, velocity) = Move(position, velocity);
                if (position.X >= input.MinX)
                {
                    break;
                }

                if (velocity.X <= 0)
                {
                    valid = false;
                    break;
                }
            }

            if (!valid)
            {
                minVx = vx + 1;
                break;
            }
        }

        return minVx;
    }

    private static (IntPoint NewPoint, IntPoint NewVelocity) Move(IntPoint currentPoint, IntPoint velocity)
    {
        var newXVelocity = velocity.X switch
        {
            > 0 => velocity.X - 1,
            < 0 => velocity.X + 1,
            0 => 0
        };
        var newYVelocity = velocity.Y - 1;

        var newPosition = currentPoint with { X = currentPoint.X + velocity.X, Y = currentPoint.Y + velocity.Y };

        return (
            newPosition,
            new IntPoint(newXVelocity, newYVelocity)
        );
    }
}