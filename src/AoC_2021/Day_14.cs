using System.Text;
using SheepTools;
using SheepTools.Model;

namespace AoC_2021;

public class Day_14 : BaseDay
{
    private readonly (string Template, List<(string pattern, string Value)> Instructions) _input;

    public Day_14()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        const int steps = 15;

        return new($"{Mutate(steps, _input.Template, _input.Instructions)}");
    }

    public override ValueTask<string> Solve_2()
    {
        var result = Mutate(10, _input.Template, _input.Instructions);


        // var other = _input.Instructions.SelectMany(i => i.Value).ToHashSet();
        var filteredInstructions = _input.Instructions.Where(i =>
            i.pattern.Contains('B')
            || i.Value.Contains('B'))
            // || i.pattern.Contains(result.Min.Key)
            // || i.Value.Contains(result.Min.Key))
            .ToList();

        var producers = new HashSet<char>(filteredInstructions.SelectMany(i => i.pattern))
        {
            'B',
        };
        // var moreFilteredInstructions = _input.Instructions.Where(i => i.Value.Intersect(producers).Count() > 0).ToList();

        _input.Instructions.RemoveAll(i => new string(i.Value.Intersect(producers).ToArray()) == i.Value);
        const int steps = 15;

        var x = Mutate(steps, _input.Template, _input.Instructions);

        return new($"{MutateOptimized(steps, _input.Template, _input.Instructions)}");
    }

    private (string Template, List<(string pattern, string Value)> Instructions) ParseInput()
    {
        var groupsOfLines = ParsedFile.ReadAllGroupsOfLines(InputFilePath);
        if (groupsOfLines.Count != 2) throw new SolvingException("2 groups of lines expected in input file");

        return (
            groupsOfLines[0][0],
            groupsOfLines[1].Select(g => { var parts = g.Split(" -> "); return (parts[0], $"{parts[0][0]}{parts[1]}{parts[0][1]}"); }).ToList()
        );
    }


    private static long Mutate(int stepCount, string template, List<(string Pattern, string Value)> instructions)
    {
        var result = template;
        for (int step = 0; step < stepCount; ++step)
        {
            // Console.WriteLine($"Step {step + 1}, length {result.Length}");
            var sb = new StringBuilder();

            var prevChar = '\n';
            foreach (var ch in result)
            {
                var patt = $"{prevChar}{ch}";

                var matching = instructions.FirstOrDefault(i => i.Pattern == patt);
                if (matching != default)
                {
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(matching.Value);
                }
                else
                {
                    sb.Append(ch);
                }
                prevChar = ch;
            }

            result = sb.ToString();
        }

        var groups = result.GroupBy(i => i).OrderByDescending(g => g.Count());

        return groups.First().Count() - groups.Last().Count();
    }

    private static long MutateOptimized(int stepCount, string template, List<(string Pattern, string Value)> instructions)
    {
        var generatorInstructions = instructions.Where(instruction => instruction.Value[1] == instruction.Value[2]).ToList();
        var nonGeneratorInstructions = instructions.Except(generatorInstructions).ToList();

        var allChars = instructions.SelectMany(ins => ins.Value).ToHashSet();
        var extraCharacterCount = new Dictionary<char, int>(allChars.Select(ch => new KeyValuePair<char, int>(ch, 0)));

        var result = new StringBuilder(template);
        for (int step = 0; step < stepCount; ++step)
        {
            Console.WriteLine($"Step {step + 1}, length {result.Length}");
            var sb = new StringBuilder();

            var prevChar = '\n';
            for (int i = 0; i < result.Length; ++i)
            {
                var ch = result[i];
                var patt = $"{prevChar}{ch}";

                var matching = nonGeneratorInstructions.FirstOrDefault(i => i.Pattern == patt);
                if (matching != default)
                {
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(matching.Value);
                }
                else
                {
                    var generatorMatching = generatorInstructions.FirstOrDefault(i => i.Pattern == patt);
                    if (generatorMatching == default)
                    {
                        sb.Append(ch);
                    }
                    else
                    {
                        if (i == result.Length - 1 || result[i + 1] != ch)
                        {
                            sb.Append(ch);
                            sb.Append(ch);
                        }
                        else
                        {
                            sb.Append(ch);
                            extraCharacterCount[ch]++;
                        }
                    }
                }
                prevChar = ch;
            }

            result = sb;
            if (step < 5)
                Console.WriteLine(result.ToString());
            Console.WriteLine(result.Length + extraCharacterCount.Sum(pair => pair.Value));
        }

        var groups = result.ToString().GroupBy(i => i);

        var min = long.MaxValue;
        var max = long.MinValue;

        foreach (var ch in allChars)
        {
            var total = groups.First(g => g.Key == ch).Count() + extraCharacterCount[ch];
            if (total < min)
            {
                min = total;
            }
            if (total > max)
            {
                max = total;
            }
        }

        return max - min;
    }
}
