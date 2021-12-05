//using SheepTools.Model;

namespace AoC_2021
{
    record IntPoint(int X, int Y) : SheepTools.Model.IntPoint(X, Y)
    {
        #region Equals override

        public virtual bool Equals(IntPoint? other)
        {
            if (other is null)
            {
                return false;
            }

            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        #endregion
    }

    public class Day_05 : BaseDay
    {
        private readonly List<(IntPoint Start, IntPoint End)> _input;

        public Day_05()
        {
            _input = ParseInput().ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            var pointCount = 0;

            var horizontalVerticalLines = _input.Where(pair => pair.Start.X == pair.End.X || pair.Start.Y == pair.End.Y).ToList();
            var linePoints = new Dictionary<IntPoint, int>();

            for (int i = 0; i < horizontalVerticalLines.Count; ++i)
            {
                var start = horizontalVerticalLines[i].Start;
                var end = horizontalVerticalLines[i].End;
                if (start.X == end.X)
                {
                    if (start.Y > end.Y)
                    {
                        (start, end) = (end, start);
                    }

                    for (int y = 0; y <= (end.Y - start.Y); ++y)
                    {
                        ++pointCount;
                        var point = start with { Y = start.Y + y };
                        if (!linePoints.TryAdd(point, 1))
                        {
                            ++linePoints[point];
                        }
                    }
                }
                else
                {
                    var m = (end.Y - start.Y) / ((double)end.X - start.X);
                    if (start.X > end.X)
                    {
                        (start, end) = (end, start);
                    }

                    for (int x = 0; x <= (end.X - start.X); ++x)
                    {
                        ++pointCount;
                        var point = new IntPoint(start.X + x, Convert.ToInt32(start.Y + (m * x)));
                        if (!linePoints.TryAdd(point, 1))
                        {
                            ++linePoints[point];
                        }
                    }
                }
            }

            return new($"{linePoints.Count(pair => pair.Value >= 2)}");
        }

        public override ValueTask<string> Solve_2()
        {
            var pointCount = 0;

            var linePoints = new Dictionary<IntPoint, int>();

            for (int i = 0; i < _input.Count; ++i)
            {
                var start = _input[i].Start;
                var end = _input[i].End;
                if (start.X == end.X)
                {
                    if (start.Y > end.Y)
                    {
                        (start, end) = (end, start);
                    }

                    for (int y = 0; y <= (end.Y - start.Y); ++y)
                    {
                        ++pointCount;
                        var point = start with { Y = start.Y + y };
                        if (!linePoints.TryAdd(point, 1))
                        {
                            ++linePoints[point];
                        }
                    }
                }
                else
                {
                    var m = (end.Y - start.Y) / ((double)end.X - start.X);
                    if (start.Y == end.Y || Math.Abs(m) == 1)
                    {
                        if (start.X > end.X)
                        {
                            (start, end) = (end, start);
                        }

                        for (int x = 0; x <= (end.X - start.X); ++x)
                        {
                            ++pointCount;
                            var point = new IntPoint(start.X + x, Convert.ToInt32(start.Y + (m * x)));
                            if (!linePoints.TryAdd(point, 1))
                            {
                                ++linePoints[point];
                            }
                        }
                    }
                }
            }

            return new($"{linePoints.Count(pair => pair.Value >= 2)}");
        }

        private IEnumerable<(IntPoint Start, IntPoint End)> ParseInput()
        {
            var file = new ParsedFile(InputFilePath, new[] { ' ', ',' });

            while (!file.Empty)
            {
                var line = file.NextLine();

                var start = new IntPoint(line.NextElement<int>(), line.NextElement<int>());
                line.NextElement<string>();
                var end = new IntPoint(line.NextElement<int>(), line.NextElement<int>());

                yield return (start, end);
            }
        }
    }
}