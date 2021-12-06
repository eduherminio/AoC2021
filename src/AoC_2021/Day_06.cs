using System.Numerics;

namespace AoC_2021
{
    public class Day_06 : BaseDay
    {
        private readonly List<int> _input;

        public Day_06()
        {
            _input = ParseInput();
        }

        public override ValueTask<string> Solve_1()
        {
            var fish = new List<int>(_input);

            for (int dayIndex = 0; dayIndex < 80; dayIndex++)
            {
                int fishToAdd = 0;
                for (int fishIndex = 0; fishIndex < fish.Count; fishIndex++)
                {
                    --fish[fishIndex];
                    if (fish[fishIndex] == -1)
                    {
                        fishToAdd++;
                        fish[fishIndex] = 6;
                    }
                }

                for (int i = 0; i < fishToAdd; ++i)
                {
                    fish.Add(8);
                }
            }

            return new($"{fish.Count}");
        }

        public override ValueTask<string> Solve_2()
        {
            var fishSet = _input.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            object totalFishCountLock = new object();
            BigInteger totalFishCount = 0;

            Parallel.ForEach(fishSet, pair =>
            {
                var fishList = new List<List<short>> { new() { (short)pair.Key } };

                for (int dayIndex = 0; dayIndex < 256; dayIndex++)
                {
                    int fishToAdd = 0;
                    for (int fishListIndex = 0; fishListIndex < fishList.Count; ++fishListIndex)
                    {
                        var fish = fishList[fishListIndex];
                        for (int fishIndex = 0; fishIndex < fish.Count; fishIndex++)
                        {
                            --fish[fishIndex];
                            if (fish[fishIndex] == -1)
                            {
                                fishToAdd++;
                                fish[fishIndex] = 6;
                            }
                        }
                    }

                    if (int.MaxValue - fishToAdd < fishList.Last().Count)
                    {
                        fishList.Add(new());
                    }
                    for (int i = 0; i < fishToAdd; ++i)
                    {
                        fishList.Last().Add(8);
                    }

                    //Console.WriteLine($"Item {pair.Key}, Day {dayIndex + 1}, Fish [{fishList.Count}, {fishList.Last().Count}]");
                }

                lock (totalFishCountLock)
                {
                    for (int i = 0; i < pair.Value; ++i)
                    {
                        for (int listIndex = 0; listIndex < fishList.Count; ++listIndex)
                        {
                            totalFishCount += fishList[listIndex].Count;
                        }
                    }
                }
            });

            return new($"{totalFishCount}");
        }

        private List<int> ParseInput()
        {
            var file = new ParsedFile(InputFilePath, new[] { ',' });

            return file.ToList<int>();
        }
    }
}