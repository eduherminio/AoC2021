//namespace AoC_2021;

//public class Day_18 : BaseDay
//{
//    record Element()
//    {
//        public long? Value { get; set; }

//        public List<Element>? Pair { get; set; }
//    }

//    private readonly List<Element> _input;
//    //private readonly List<List<int>> _input;

//    public Day_18()
//    {
//        _input = ParseInput();
//        //_input = ParseInput2();
//    }

//    public override ValueTask<string> Solve_1()
//    {
//        var solution = string.Empty;

//        return new(solution);
//    }

//    public override ValueTask<string> Solve_2()
//    {
//        var solution = string.Empty;

//        return new(solution);
//    }

//    private List<Element> ParseInput()
//    {
//        var file = new ParsedFile(InputFilePath);

//        var result = new List<Element>();

//        while (!file.Empty)
//        {
//            var line = file.NextLine();
//            var currentElement = new Element();

//            var pairStack = new Stack<Pair>();
//            Pair? currentPair = null;

//            while (!line.Empty)
//            {
//                var ch = line.NextElement<char>();
//                switch (ch)
//                {
//                    case '[':
//                        currentPair = new Pair();
//                        pairStack.Push(currentPair);
//                        break;
//                    case ',':
//                        break;
//                    case ']':
//                        var lastStack = pairStack.Pop();
//                        if (pairStack.TryPeek(out var pair))
//                        {
//                            pair.NextElement.Pair = lastStack;
//                        }
//                        else
//                        {
//                            result.Add(lastStack);
//                        }
//                        break;
//                    default:
//                        var nextElement = currentPair!.NextElement;
//                        nextElement.Value = int.Parse(ch.ToString());
//                        break;
//                }
//            }
//        }

//        return result;
//    }
//}
