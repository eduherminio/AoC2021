namespace AoC_2021;

sealed record class Node(string Id) : SheepTools.Model.GenericNode<string>(Id)
{
    private const string StartNodeId = "start";
    private const string EndNodeId = "end";

    public bool IsMajor() => char.IsUpper(Id[0]);
    public bool IsStart() => Id == StartNodeId;
    public bool IsEnd() => Id == EndNodeId;

    public HashSet<Node> Connections { get; set; } = new HashSet<Node>();

    #region Equals override, to avoid taking into account Connections for equality

    public override int GetHashCode() => base.GetHashCode();

    public bool Equals(Node? other) => base.Equals(other);

    #endregion
}

public class Day_12 : BaseDay
{
    private readonly HashSet<Node> _input;

    public Day_12()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        var start = _input.First(n => n.IsStart());
        var end = _input.First(n => n.IsEnd());

        List<(string Path, Node LastNode)> pathList = new() { (start.Id, start) };

        while (pathList.Any(p => !p.LastNode.IsEnd()))
        {
            List<(string Path, Node LastNode)> newPathList = new(pathList.Where(p => p.LastNode.IsEnd()));
            foreach (var path in pathList)
            {
                if (path.LastNode != end)
                {
                    foreach (var connection in path.LastNode.Connections)
                    {
                        if (connection.IsMajor() || (!path.Path.Contains($",{connection.Id},") && !connection.IsStart()))
                        {
                            var newPath = ($"{path.Path},{connection.Id}", connection);
                            newPathList.Add(newPath);
                        }
                    }
                }
            }

            pathList = newPathList;
        }

        return new($"{pathList.Count}");
    }

    public override ValueTask<string> Solve_2()
    {
        var start = _input.First(n => n.IsStart());
        var end = _input.First(n => n.IsEnd());

        List<(string Path, Node LastNode)> pathList = new() { (start.Id, start) };

        while (pathList.Any(p => !p.LastNode.IsEnd()))
        {
            List<(string Path, Node LastNode)> newPathList = new(pathList.Where(p => p.LastNode.IsEnd()));
            foreach (var path in pathList)
            {
                if (path.LastNode != end)
                {
                    foreach (var connection in path.LastNode.Connections)
                    {
                        var caves = path.Path.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        var minorCaves = caves.Select(c => _input.First(i => i.Id == c)).Where(c => !c.IsMajor());

                        var isMinorCaveRepeated = minorCaves.Distinct().Count() != minorCaves.Count();

                        if (connection.IsMajor() || (!isMinorCaveRepeated || !path.Path.Contains($",{connection.Id},")) && !connection.IsStart())
                        {
                            var newPath = ($"{path.Path},{connection.Id}", connection);
                            newPathList.Add(newPath);
                        }
                    }
                }
            }

            pathList = newPathList;
        }

        return new($"{pathList.Count}");
    }

    private HashSet<Node> ParseInput()
    {
        var nodeList = new HashSet<Node>(100);

        foreach (var line in new ParsedFile(InputFilePath, new[] { '-' }).ToList())
        {
            var origin = new Node(line.NextElement<string>());
            var destination = new Node(line.NextElement<string>());

            if (!nodeList.Add(origin))
            {
                origin = nodeList.First(n => n.Id == origin.Id);
            }
            if (!nodeList.Add(destination))
            {
                destination = nodeList.First(n => n.Id == destination.Id);
            }

            origin.Connections.Add(destination);
            destination.Connections.Add(origin);
        }

        return nodeList;
    }
}