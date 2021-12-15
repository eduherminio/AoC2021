namespace AoC_2021;

public class Day_15 : BaseDay
{
    sealed record Node(SheepTools.Model.IntPoint Id, int Risk)
        : SheepTools.Model.GenericNode<SheepTools.Model.IntPoint>(Id)
    {
        public List<Node> Neighbours { get; private set; }

        public Node Parent { get; set; }

        public int Depth => Id.Y;

        public void CalculateNeighbours(List<List<Node>> grid)
        {
            if (Id.Y != grid.Count)
            {
                var points = new List<SheepTools.Model.IntPoint>
                {
                    Id.Move(SheepTools.Model.Direction.Down),
                    Id.Move(SheepTools.Model.Direction.Up),
                    Id.Move(SheepTools.Model.Direction.Left),
                    Id.Move(SheepTools.Model.Direction.Right)
                }
                .Where(p => p.Y >= 0 && p.Y < grid.Count && p.X >= 0 && p.X < grid[p.Y].Count)
                .ToList();

                Neighbours = points.Select(node => grid[node.Y][node.X]).ToList();
            }
        }

        #region Equals override, to avoid taking into account Connections for equality

        public override int GetHashCode() => base.GetHashCode();

        public bool Equals(Node? other) => base.Equals(other);

        #endregion
    }

    private readonly List<List<Node>> _input;

    public Day_15()
    {
        _input = ParseInput();
        foreach (var list in _input)
        {
            foreach (var node in list)
            {
                node.CalculateNeighbours(_input);
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var result = AStarBacktracking(_input);

        return new($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {

        const int n = 3;
        var result = 0;

        return new("");
    }

    private List<List<Node>> ParseInput()
    {
        return File.ReadAllLines(InputFilePath).Select((str, y) => str.Select((ch, x) => new Node(new SheepTools.Model.IntPoint(x, y), int.Parse(ch.ToString()))).ToList()).ToList();
    }

    /// <summary>
    /// A node needs all the previous used nodes to check success, so it cannot be deducted afterwards
    /// </summary>
    /// <param name="allNodes"></param>
    /// <param name="isSuccess"></param>
    private static int AStarBacktracking(List<List<Node>> allNodes)
    {
        var start = allNodes[0][0];
        var end = allNodes.Last().Last();

        var priorityQueue = new PriorityQueue<Node, int>();
        priorityQueue.Enqueue(start, 0);

        var scores = new Dictionary<Node, int>(allNodes.SelectMany(n => n).Select(n => new KeyValuePair<Node, int>(n, int.MaxValue)))
        {
            [start] = 0 // Overridin initial one
        };

        var finalScores = new Dictionary<Node, int>(allNodes.SelectMany(n => n).Select(n => new KeyValuePair<Node, int>(n, int.MaxValue)))
        {
            [start] = 0 // Overridin initial one
        };

        var cameFrom = new Dictionary<Node, Node>();

        while (priorityQueue.TryDequeue(out var current, out _))
        {
            if (current == end)
            {
                return scores[current];
            }

            foreach (var neighbour in current.Neighbours)
            {
                var tentativeScore = scores[current] + neighbour.Risk;
                if (tentativeScore < scores[neighbour])
                {
                    cameFrom[neighbour] = current;
                    neighbour.Parent = current;
                    scores[neighbour] = tentativeScore;
                    finalScores[neighbour] = tentativeScore;

                    if (!priorityQueue.UnorderedItems.Any(pair => pair.Element == neighbour))
                    {
                        priorityQueue.Enqueue(neighbour, finalScores[neighbour]);
                    }
                }
            }
        }

        throw new SolvingException();
    }

    private static ICollection<Node> ReconstructPath(Node end)
    {
        throw new NotImplementedException();
    }
}