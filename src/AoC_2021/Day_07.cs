using SheepTools;

namespace AoC_2021
{
    public class Day_07 : BaseDay
    {
        private readonly List<int> _input;

        public Day_07()
        {
            _input = ParseInput();
        }

        public override ValueTask<string> Solve_1()
        {
            var range = RangeHelpers.GenerateRange(_input.Min(), _input.Max());
            var fuel = new int[range.Count()];

            foreach (var alignPosition in range)
            {
                foreach (var crabPosition in _input)
                {
                    fuel[alignPosition] += Math.Abs(crabPosition - alignPosition);
                }
            }

            return new($"{fuel.Min()}");
        }

        public override ValueTask<string> Solve_2()
        {
            var range = RangeHelpers.GenerateRange(_input.Min(), _input.Max());
            var fuel = new int[range.Count()];

            foreach (var alignPosition in range)
            {
                foreach (var crabPosition in _input)
                {
                    for (int i = 1; i <= Math.Abs(crabPosition - alignPosition); ++i)
                    {
                        fuel[alignPosition] += i;
                    }
                }
            }

            return new($"{fuel.Min()}");
        }

        private List<int> ParseInput()
        {
            var file = new ParsedFile(InputFilePath, new[] { ',' });

            return file.ToList<int>();
        }
    }
}