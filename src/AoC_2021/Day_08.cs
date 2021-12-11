using SheepTools.Model;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace AoC_2021
{
    public class Day_08 : BaseDay
    {
        private List<(List<string> SignalPatterns, List<string> Output)> _input;

        ///   0:      1:      2:      3:      4:
        ///  aaaa    ....    aaaa    aaaa    ....
        /// b    c  .    c  .    c  .    c  b    c
        /// b    c  .    c  .    c  .    c  b    c
        ///  ....    ....    dddd    dddd    dddd
        /// e    f  .    f  e    .  .    f  .    f
        /// e    f  .    f  e    .  .    f  .    f
        ///  gggg    ....    gggg    gggg    ....
        ///
        ///   5:      6:      7:      8:      9:
        ///  aaaa    aaaa    aaaa    aaaa    aaaa
        /// b    .  b    .  .    c  b    c  b    c
        /// b    .  b    .  .    c  b    c  b    c
        ///  dddd    dddd    ....    dddd    dddd
        /// .    f  e    f  .    f  e    f  .    f
        /// .    f  e    f  .    f  e    f  .    f
        ///  gggg    gggg    ....    gggg    gggg
        private static readonly IReadOnlyDictionary<string, int> _chars = new Dictionary<string, int>()
        {
            ["abcefg"] = 0,
            ["cf"] = 1,
            ["acdeg"] = 2,
            ["acdfg"] = 3,
            ["bcdf"] = 4,
            ["abdfg"] = 5,
            ["abdefg"] = 6,
            ["acf"] = 7,
            ["abcdefg"] = 8,
            ["abcdfg"] = 9,
        };

        private static readonly List<IGrouping<int, KeyValuePair<string, int>>> _numbersByCharLength = _chars.GroupBy(ch => ch.Key.Length).ToList();

        public Day_08()
        {
            _input = ParseInput().ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            return new(_input
                .SelectMany(pair => pair.Output.Select(output => output.Length))
                .Count(nChars => nChars == 2 || nChars == 3 || nChars == 4 || nChars == 7)
                .ToString());
        }

        private static readonly IReadOnlyDictionary<short, HashSet<int>> _positionsByDigitDict = new Dictionary<short, HashSet<int>>()
        {
            [1] = new HashSet<int> { 3, 6 },                // Unique length
            [4] = new HashSet<int> { 2, 3, 4, 6, },         // Unique length
            [7] = new HashSet<int> { 1, 3, 6, },            // Unique length
            [2] = new HashSet<int> { 1, 3, 4, 5, 7 },
            [3] = new HashSet<int> { 1, 3, 4, 6, 7 },
            [5] = new HashSet<int> { 1, 2, 4, 6, 7 },
            [6] = new HashSet<int> { 1, 2, 4, 5, 6, 7 },
            [0] = new HashSet<int> { 1, 2, 3, 5, 6, 7 },
            [9] = new HashSet<int> { 1, 2, 3, 4, 6, 7 },
            [8] = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7 } // Unique length
        };

        public override ValueTask<string> Solve_2()
        {
            ImmutableArray<char> constantChars = ImmutableArray.Create(new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' });
            ImmutableArray<int> constantPositions = ImmutableArray.Create(new[] { 1, 2, 3, 4, 5, 6, 7 });

            var finalResult = 0;
            foreach (var (SignalPatterns, Output) in _input)
            {
                Dictionary<char, HashSet<int>> positionsByCharDict = new(constantChars.Select(c =>
                    new KeyValuePair<char, HashSet<int>>(c, new HashSet<int>(constantPositions))));

                List<Dictionary<char, int>> finalPositionsByCharDictList = new() { new() };
                var finalPositionsByCharDict = finalPositionsByCharDictList.Last();
                // {

                //     var combinations = new List<Dictionary<int, char>>() { new() };
                //     foreach (var candidate in positionsByCharDict)
                //     {
                //         var newCombinations = new List<Dictionary<int, char>>(2 * combinations.Count);
                //         foreach (var position in candidate.Value)
                //         {
                //             foreach (var combination in combinations)
                //             {
                //                 var newCombination = new Dictionary<int, char>(combination);
                //                 if (!newCombination.TryAdd(position, candidate.Key))
                //                 {
                //                     continue;
                //                 }
                //                 newCombinations.Add(newCombination);
                //             }
                //         }

                //         combinations = newCombinations;
                //     }

                //     if (combinations.Count == 1 && combinations[0].Count == positionsByCharDict.Count)
                //     {
                //         foreach (var pair in combinations[0])
                //         {
                //             finalPositionsByCharDict.Add(pair.Value, pair.Key);
                //         }
                //     }

                //     var solutioDictionary = new Dictionary<string, int>(_positionsByDigitDict.Select(pair => new KeyValuePair<string, int>(string.Join("", pair.Value), pair.Key)));
                //     var outpuResult = Output.Aggregate("", (acc, n) => acc + solutioDictionary[string.Join("", n.Select(ch => finalPositionsByCharDict[ch]).OrderBy(pos => pos))]);

                //     ;
                // }


                Dictionary<short, HashSet<char>> knownCharsByDigitDict = GetCharsByDigit(SignalPatterns);

                var charsWeCareAbout = Output.SelectMany(o => o.ToArray()).ToHashSet();
                while (charsWeCareAbout.Any())
                {
                    var previousFinalPositionsByCharDictLenght = finalPositionsByCharDict.Count;

                    // Positions only considered by one char
                    foreach (var position in constantPositions)
                    {
                        var charMatches = positionsByCharDict.Where(pair => pair.Value.Contains(position));
                        if (charMatches.Count() == 1 && !finalPositionsByCharDict.ContainsValue(position))
                        {
                            positionsByCharDict[charMatches.First().Key] = new HashSet<int> { position };
                        }
                    }

                    // Matches by digit
                    foreach (var knownCharsByDigit in knownCharsByDigitDict)
                    {
                        var charsByDigit = knownCharsByDigit.Value.Where(ch => !finalPositionsByCharDict.ContainsKey(ch));
                        var positionsByDigit = _positionsByDigitDict[knownCharsByDigit.Key].Where(pos => !finalPositionsByCharDict.ContainsValue(pos));

                        // If one of the positions is only possible by one of the chars
                        foreach (var position in positionsByDigit)
                        {
                            var charsThatCanHaveThatPosition = charsByDigit.Where(ch => positionsByCharDict[ch].Contains(position));

                            if (charsThatCanHaveThatPosition.Count() == 1)
                            {
                                positionsByCharDict[charsThatCanHaveThatPosition.First()] = new() { position };
                            }
                        }

                        // If one of these chars, cannot have other positions
                        foreach (var ch in charsByDigit)
                        {
                            var n = positionsByCharDict[ch].Where(pos => !positionsByDigit.Contains(pos)).ToList();
                            positionsByCharDict[ch].RemoveWhere(pos => !positionsByDigit.Contains(pos));
                        }

                        // If not one of these chars, cannot have these positions
                        for (int i = 0; i < positionsByCharDict.Count; ++i)
                        {
                            var item = positionsByCharDict.ElementAt(i);

                            if (!charsByDigit.Contains(item.Key))
                            {
                                item.Value.RemoveWhere(pos => positionsByDigit.Contains(pos));
                            }
                        }
                    }

                    // Verify which chars only have one possible solution
                    for (int i = 0; i < positionsByCharDict.Count; ++i)
                    {
                        var item = positionsByCharDict.ElementAt(i);
                        if (item.Value.Count == 1 && !finalPositionsByCharDict.ContainsKey(item.Key))
                        {
                            for (int j = 0; j < positionsByCharDict.Count; ++j)
                            {
                                if (i != j)
                                {
                                    positionsByCharDict.ElementAt(j).Value.Remove(item.Value.First());
                                }
                            }

                            finalPositionsByCharDict[item.Key] = item.Value.First();
                            positionsByCharDict.Remove(item.Key);
                            charsWeCareAbout.Remove(item.Key);
                            // updated = true;
                        }
                    }

                    // No progress deducting
                    if (finalPositionsByCharDict.Count == previousFinalPositionsByCharDictLenght)
                    {
                        var combinations = new List<Dictionary<int, char>>() { new(finalPositionsByCharDict.Select(pair => new KeyValuePair<int, char>(pair.Value, pair.Key))) };
                        foreach (var candidate in positionsByCharDict)
                        {
                            var newCombinations = new List<Dictionary<int, char>>(2 * combinations.Count);
                            foreach (var position in candidate.Value)
                            {
                                foreach (var combination in combinations)
                                {
                                    var newCombination = new Dictionary<int, char>(combination);
                                    if (!newCombination.TryAdd(position, candidate.Key))
                                    {
                                        continue;
                                    }
                                    newCombinations.Add(newCombination);
                                }
                            }

                            combinations = newCombinations;
                        }

                        var goodCombinations = combinations.Where(combination =>
                        {
                            foreach (var known in knownCharsByDigitDict)
                            {
                                var positions = known.Value.Select(val => combination.Single(c => c.Value == val).Key);
                                if (positions.Any(pos => !_positionsByDigitDict[known.Key].Contains(pos)))
                                {
                                    return false;
                                }
                            }

                            return true;
                        }).ToList();

                        //if (goodCombinations.Count == 1 && goodCombinations[0].Count == positionsByCharDict.Count)
                        foreach (var goodCombination in goodCombinations)
                        {
                            finalPositionsByCharDictList.Add(new(finalPositionsByCharDictList.First()));

                            foreach (var pair in goodCombination)
                            {
                                finalPositionsByCharDictList.Last()[pair.Value] = pair.Key;
                            }
                        }

                        finalPositionsByCharDictList = finalPositionsByCharDictList.Skip(1).ToList();
                        break;
                    }
                }

                var solutionDictionary = new Dictionary<string, int>(_positionsByDigitDict.Select(pair => new KeyValuePair<string, int>(string.Join("", pair.Value), pair.Key)));

                foreach (var allegedSol in finalPositionsByCharDictList)
                {
                    try
                    {
                        string outputResult = Output.Aggregate("", (acc, n) => acc + solutionDictionary[string.Join("", n.Select(ch => allegedSol[ch]).OrderBy(pos => pos))]);
                        finalResult += int.Parse(outputResult);
                        break;
                    }
                    catch (Exception) { }
                }
            }

            return new($"{finalResult}");
        }

        private Dictionary<short, HashSet<char>> GetCharsByDigit(List<string> signalPatters)
        {
            Dictionary<short, HashSet<char>> charByDigit = new();

            foreach (var pattern in signalPatters)
            {
                var candidates = _positionsByDigitDict.GroupBy(n => n.Value.Count)
                    .Where(n => n.Key == pattern.Length)
                    .SelectMany(group => group.Select(pair => pair.Key));

                if (candidates.Count() == 1)
                {
                    charByDigit[candidates.First()] = pattern.ToHashSet();
                }
            }

            return charByDigit;
        }

        public ValueTask<string> Solve__2()
        {
            var numbers = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };

            foreach (var input in _input)
            {
                var dict = new Dictionary<char, List<char>>()
                {
                    ['a'] = numbers.ToList(),
                    ['b'] = numbers.ToList(),
                    ['c'] = numbers.ToList(),
                    ['d'] = numbers.ToList(),
                    ['e'] = numbers.ToList(),
                    ['f'] = numbers.ToList(),
                    ['g'] = numbers.ToList()
                };

                var charByNumber = new Dictionary<char, List<int>>(
                    numbers
                        .Select(ch => new KeyValuePair<char, List<int>>(
                            ch,
                            _chars
                                .Where(pair => pair.Key.Contains(ch))
                                .Select(pair => pair.Value)
                                .ToList())
                    ));

                var segments = InitializeSegments(input.SignalPatterns);

                var solvedOutput = input.Output.Select(p =>
                {
                    var index = segments.IndexOf(p);

                    return index != -1
                    ? $"{index}"
                    : p;
                }).ToList();

                static bool IsSolved(int n, List<string> solvedOutput) => solvedOutput[n].Length == 1;

                while (solvedOutput.Any(s => s.Length != 1))
                {
                    var knowns = segments
                        .Select((value, index) => (value, index))
                        .Where(s => s.value != null)
                        .ToList();

                    foreach (var pair in charByNumber)
                    {
                        if (knowns.All(known => pair.Value.Contains(known.index)))
                        {
                            var intersection = dict[pair.Key];
                            foreach (var known in knowns)
                            {
                                intersection = intersection.Intersect(known.value).ToList();
                            }

                            dict[pair.Key] = intersection;
                        }
                    }

                    foreach (var pair in dict)
                    {
                        if (pair.Value.Count == 1)
                        {
                            charByNumber.Remove(pair.Key);
                        }
                    }

                    var knownValues = dict.Where(pair => pair.Value.Count == 1);

                    foreach (var unsolvedOutput in solvedOutput.Where(s => s.Length != 1))
                    {
                        if (unsolvedOutput.All(ch => knownValues.Select(p => p.Value[0]).Contains(ch)))
                        {
                            var str = "";
                            foreach (var ch in unsolvedOutput)
                            {
                                str.Append(knownValues.Single(pair => pair.Value[0] == ch).Key);
                            }

                            str = new string(str.OrderBy(s => s).ToArray());

                            var n = _chars[str];
                        }
                    }
                }
            }
            return new("");
        }

        private static List<string> InitializeSegments(List<string> SignalPatters)
        {
            string[] segments = new string[10];

            foreach (var pattern in SignalPatters)
            {
                var candidates = _numbersByCharLength
                    .Where(n => n.Key == pattern.Length)
                    .SelectMany(group => group.Select(pair => pair.Value));

                if (candidates.Count() == 1)
                {
                    segments[candidates.First()] = pattern;
                }
            }

            return segments.ToList();
        }

        private IEnumerable<(List<string> Input, List<string> Output)> ParseInput()
        {
            var file = new ParsedFile(InputFilePath);

            while (!file.Empty)
            {
                var line = file.NextLine();
                var input = new List<string>();
                var output = new List<string>();

                bool isOutput = false;
                while (!line.Empty)
                {
                    var item = new string(line.NextElement<string>().OrderBy(s => s).ToArray());
                    if (item == "|")
                    {
                        isOutput = true;
                    }
                    else if (isOutput)
                    {
                        output.Add(item);
                    }
                    else
                    {
                        input.Add(item);
                    }
                }

                yield return (input, output);
            }
        }
    }
}