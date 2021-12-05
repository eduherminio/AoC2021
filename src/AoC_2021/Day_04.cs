using SheepTools;
using SheepTools.Model;

namespace AoC_2021
{
    public record PointWithValue(/*int X, int Y, */int Value)/* : IntPoint(X, Y)*/
    {
        public bool Completed { get; set; } = false;
    }

    public record BingoBoard(List<PointWithValue> Numbers)
    {
        public bool ForSureCompleted { get; set; } = false;

        public const int BoardSize = 5;

        public bool Completed(int numberIndex)
        {
            var complete = true;

            var startY = numberIndex % BoardSize;
            for (int y = 0; y < BoardSize; ++y)
            {
                var n = Numbers[startY + (BoardSize * y)];
                if (!n.Completed)
                {
                    complete = false;
                    break;
                }
            }

            if (complete)
            {
                return true;
            }

            complete = true;

            var startX = numberIndex / BoardSize;
            for (int x = 0; x < BoardSize; ++x)
            {
                var n = Numbers[(startX * BoardSize) + x];
                if (!n.Completed)
                {
                    complete = false;
                    break;
                }
            }

            return complete;
        }
    }

    public class Day_04 : BaseDay
    {
        private readonly List<BingoBoard> _inputBoards;
        private readonly List<int> _inputNumbers;

        public Day_04()
        {
            (_inputNumbers, _inputBoards) = ParseInput();
        }

        public override ValueTask<string> Solve_1()
        {
            for (int numberIndex = 0; numberIndex < _inputNumbers.Count; ++numberIndex)
            {
                var number = _inputNumbers[numberIndex];
                for (int boardIndex = 0; boardIndex < _inputBoards.Count; ++boardIndex)
                {
                    var board = _inputBoards[boardIndex];

                    var foundNumberIndex = board.Numbers.FindIndex(n => n.Value == number);
                    if (foundNumberIndex != -1)
                    {
                        board.Numbers[foundNumberIndex].Completed = true;

                        if (board.Completed(foundNumberIndex))
                        {
                            return new($"{board.Numbers.Where(n => !n.Completed).Sum(n => n.Value) * number}");
                        }
                    }
                }
            }

            throw new SolvingException($"Error solving {nameof(Day_04)} part 1");
        }

        public override ValueTask<string> Solve_2()
        {
            for (int numberIndex = 0; numberIndex < _inputNumbers.Count; ++numberIndex)
            {
                var number = _inputNumbers[numberIndex];
                for (int boardIndex = 0; boardIndex < _inputBoards.Count; ++boardIndex)
                {
                    var board = _inputBoards[boardIndex];
                    if (board.ForSureCompleted)
                    {
                        continue;
                    }

                    var foundNumberIndex = board.Numbers.FindIndex(n => n.Value == number);
                    if (foundNumberIndex != -1)
                    {
                        board.Numbers[foundNumberIndex].Completed = true;

                        if (board.Completed(foundNumberIndex))
                        {
                            board.ForSureCompleted = true;
                            if (_inputBoards.All(b => b.ForSureCompleted))
                            {
                                return new($"{board.Numbers.Where(n => !n.Completed).Sum(n => n.Value) * number}");
                            }
                        }
                    }
                }
            }

            throw new SolvingException($"Error solving {nameof(Day_04)} part 2");
        }

        private (List<int>, List<BingoBoard>) ParseInput()
        {
            var file = new ParsedFile(InputFilePath, existingSeparator: new[] { ' ', ',' });

            var numbers = file.NextLine().ToList<int>();

            var boards = new List<BingoBoard>();
            var boardNumbers = new List<PointWithValue>();

            var y = 0;
            while (!file.Empty)
            {
                if (y == 5)
                {
                    y = 0;
                    boards.Add(new BingoBoard(boardNumbers));
                    boardNumbers = new List<PointWithValue>();
                }

                boardNumbers.AddRange(file.NextLine().ToList<int>().Select((value, index) => new PointWithValue(value)));
                ++y;
            }

            boards.Add(new BingoBoard(boardNumbers));

            return (numbers, boards);
        }
    }
}