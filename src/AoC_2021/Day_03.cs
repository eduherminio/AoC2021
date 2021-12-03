using System.Text;

namespace AoC_2021
{
    public class Day_03 : BaseDay
    {
        private readonly string[] _input;

        public Day_03()
        {
            _input = ParseInput();
        }

        public override ValueTask<string> Solve_1()
        {
            var gammaSb = new StringBuilder(_input[0].Length);
            var epsilonSb = new StringBuilder(_input[0].Length);

            for (int i = 0; i < _input[0].Length; ++i)
            {
                var slice = _input.Select(number => int.Parse($"{number[i]}"));
                if (slice.Sum() > _input.Length / 2)
                {
                    gammaSb.Append('0');
                    epsilonSb.Append('1');
                }
                else
                {
                    gammaSb.Append('1');
                    epsilonSb.Append('0');
                }
            }

            var gamma = Convert.ToInt32(gammaSb.ToString(), 2);
            var epsilon = Convert.ToInt32(epsilonSb.ToString(), 2);

            return new($"{gamma * epsilon}");
        }

        public override ValueTask<string> Solve_2()
        {
            var oxygenGeneratorRating = FindOxygenGeneratorRating(_input);
            var co2ScrubberRating = FindCO2ScrubberRating(_input);

            return new($"{oxygenGeneratorRating * co2ScrubberRating}");
        }

        private string[] ParseInput()
        {
            return File.ReadAllLines(InputFilePath);
        }

        public static int FindOxygenGeneratorRating(string[] input, int bitPosition = 0)
        {
            if (input.Length == 1)
            {
                return Convert.ToInt32(input[0], 2);
            }

            var slice = input.Select(number => int.Parse($"{number[bitPosition]}"));

            var mostCommonChar = slice.Sum() >= input.Length / 2d
                ? '1'
                : '0';

            return FindOxygenGeneratorRating(input.Where(number => number[bitPosition] == mostCommonChar).ToArray(), bitPosition + 1);
        }

        public static int FindCO2ScrubberRating(string[] input, int bitPosition = 0)
        {
            if (input.Length == 1)
            {
                return Convert.ToInt32(input[0], 2);
            }

            var slice = input.Select(number => int.Parse($"{number[bitPosition]}"));

            var leastCommonChar = slice.Sum() >= input.Length / 2d
                ? '0'
                : '1';

            return FindCO2ScrubberRating(input.Where(number => number[bitPosition] == leastCommonChar).ToArray(), bitPosition + 1);
        }
    }
}