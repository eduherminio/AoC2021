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

        public override ValueTask<string> Solve_1() => SolveSimple(80);

        public override ValueTask<string> Solve_2() => SolveReusingCalculations(256);

        private List<int> ParseInput()
        {
            var file = new ParsedFile(InputFilePath, new[] { ',' });

            return file.ToList<int>();
        }

        private ValueTask<string> SolveSimple(int days)
        {
            var fish = new List<int>(_input);

            for (int dayIndex = 0; dayIndex < days; dayIndex++)
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

        private ValueTask<string> Solve_Bruteforce_ParallelForeach(int days)
        {
            var fishSet = _input.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            object totalFishCountLock = new object();
            BigInteger totalFishCount = 0;

            Parallel.ForEach(fishSet, pair =>
            {
                var fishList = new List<List<short>> { new() { (short)pair.Key } };

                for (int dayIndex = 0; dayIndex < days; dayIndex++)
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

                    // Console.WriteLine($"Item {pair.Key}, Day {dayIndex + 1}, Fish [{fishList.Count}, {fishList.Last().Count}]");
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

        private ValueTask<string> SolveReusingCalculations(int days)
        {
            var fishSet = _input.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

            var min = fishSet.Min(pair => pair.Key);
            var max = fishSet.Max(pair => pair.Key);

            var fishList = new List<List<short>> { new() { (short)min } };

            Dictionary<int, BigInteger> totalFishCount = new();
            for (int dayIndex = 0; dayIndex < days; dayIndex++)
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

                if (dayIndex >= days - max)
                {
                    var key = days - dayIndex;
                    totalFishCount[key] = fishList[0].Count;
                    for (int listIndex = 1; listIndex < fishList.Count; ++listIndex)
                    {
                        totalFishCount[key] += fishList[listIndex].Count;
                    }
                }

                // Console.WriteLine($"Item {min}, Day {dayIndex + 1}, Fish [{fishList.Count}, {fishList.Last().Count}]");
            }

            BigInteger finalCount = 0;
            foreach (var pair in fishSet)
            {
                for (int i = 0; i < pair.Value; ++i)
                {
                    finalCount += totalFishCount[pair.Key];
                }
            }

            return new($"{finalCount}");
        }
    }
}