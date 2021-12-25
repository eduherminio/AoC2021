using SheepTools.Model;
using System;
using System.Linq;

namespace AoC_2021;

public sealed record Distance(double Value, IntPoint3D A, IntPoint3D B)
{
    #region Equals override, to avoid taking into account Connections for equality

    public override int GetHashCode() => A.GetHashCode() * B.GetHashCode();

    public bool Equals(Distance? other)
    {
        return other is not null
            && Value.Equals(other.Value)
            && ((other.A == A && other.B == B)
                || (other.A == B && other.B == A));
    }

    #endregion
}

/// <summary>
/// Lightweight <see cref="3DPoint"/> record class, based on <see cref="int"/> and without Id.
/// </summary>
public record IntPoint3D(int X, int Y, int Z)
{
    public List<IntPoint3D> Rotations() => new()
    {
        // XYZ
        this,
        new IntPoint3D(X, Y, -Z),

        new IntPoint3D(-X, Y, Z),
        new IntPoint3D(-X, Y, -Z),

        new IntPoint3D(X, -Y, Z),
        new IntPoint3D(X, -Y, -Z),

        new IntPoint3D(-X, -Y, Z),
        new IntPoint3D(-X, -Y, -Z),

        // XZY
        new IntPoint3D(X, Z, Y),
        new IntPoint3D(X, Z, -Y),

        new IntPoint3D(-X, Z, Y),
        new IntPoint3D(-X, Z, -Y),

        new IntPoint3D(X, -Z, Y),
        new IntPoint3D(X, -Z, -Y),

        new IntPoint3D(-X, -Z, Y),
        new IntPoint3D(-X, -Z, -Y),

        //YZX
        new IntPoint3D(Y, Z, X),
        new IntPoint3D(Y, Z, -X),

        new IntPoint3D(-Y, Z, X),
        new IntPoint3D(-Y, Z, -X),

        new IntPoint3D(Y, -Z, X),
        new IntPoint3D(Y, -Z, -X),

        new IntPoint3D(-Y, -Z, X),
        new IntPoint3D(-Y, -Z, -X),
    };

    public double DistanceTo(IntPoint3D otherPoint)
    {
        return Math.Sqrt(
            Math.Pow(otherPoint.X - X, 2)
            + Math.Pow(otherPoint.Y - Y, 2)
            + Math.Pow(otherPoint.Z - Z, 2));
    }

    public List<Distance> Distances(List<IntPoint3D> otherPoints)
    {
        return otherPoints
            .Except(new[] { this })
            .Select(p => new Distance(DistanceTo(p), this, p))
            .ToList();
    }

    public static IntPoint3D operator +(IntPoint3D a, IntPoint3D b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static IntPoint3D operator -(IntPoint3D a, IntPoint3D b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public override string ToString()
    {
        return $"[{X}, {Y}, {Z}]";
    }
}

public class Day_19 : BaseDay
{
    record Scanner(List<IntPoint3D> Points)
    {
        public IntPoint3D? Origin { get; set; } = null;

        public IEnumerable<List<IntPoint3D>> Positions()
        {
            var rotations = Points.Select(p => p.Rotations());

            for (int i = 0; i < 24; ++i)
            {
                yield return rotations.Select(r => r[i]).ToList();
            }
        }

        public List<Distance> Distances => Points.SelectMany(p => p.Distances(Points)).Distinct().ToList();
    }

    private readonly List<Scanner> _input;

    public Day_19()
    {
        _input = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        _input[0].Origin = new(0, 0, 0);
        var unvisitedScanners = new List<Scanner> { _input[0] };
        var alreadyVisitedScanners = new List<Scanner>(_input.Count);

        var beaconsFromOrigin = new HashSet<IntPoint3D>();

        while (unvisitedScanners.Count > 0)
        {
            var unvisitedScanner = unvisitedScanners[0];
            if (alreadyVisitedScanners.Contains(unvisitedScanner))
            {
                unvisitedScanners.Remove(unvisitedScanner);
                continue;
            }

            foreach (var scanner in _input)
            {
                if (scanner.Origin == unvisitedScanner.Origin)
                {
                    continue;
                }

                var sharedBeaconLocations = unvisitedScanner.Distances.Join(scanner.Distances, d => 0.01 * Math.Round(100 * d.Value), d => 0.01 * Math.Round(100 * d.Value), (d1, d2) => (d1.A, d1.B, d1.Value, d2.A, d2.B)).ToList();

                var detectedPoints = new HashSet<IntPoint3D>();
                //var detectedPoints2 = new HashSet<IntPoint3D>();

                foreach (var pair in sharedBeaconLocations)
                {
                    detectedPoints.Add(pair.Item1);
                    detectedPoints.Add(pair.Item2);

                    //detectedPoints2.Add(pair.Item4);
                    //detectedPoints2.Add(pair.Item5);
                }

                if (detectedPoints.Count >= 12)
                {
                    var possibleOrigins = new List<(IntPoint3D Origin, IntPoint3D Source)>();
                    foreach (var pair in sharedBeaconLocations)
                    {
                        foreach (var p in pair.Item4.Rotations())
                        {
                            possibleOrigins.Add((new(pair.Item1.X + p.X, pair.Item1.Y + p.Y, pair.Item1.Z + p.Z), p));
                            possibleOrigins.Add((new(pair.Item2.X + p.X, pair.Item2.Y + p.Y, pair.Item2.Z + p.Z), p));
                        }

                        foreach (var p in pair.Item5.Rotations())
                        {
                            possibleOrigins.Add((new(pair.Item1.X + p.X, pair.Item1.Y + p.Y, pair.Item1.Z + p.Z), p));
                            possibleOrigins.Add((new(pair.Item2.X + p.X, pair.Item2.Y + p.Y, pair.Item2.Z + p.Z), p));
                        }
                    }

                    var validGroup = possibleOrigins.GroupBy(o => o.Origin).OrderByDescending(g => g.Count()).First();
                    var sourcePoints = validGroup.Select(g => g.Source).Distinct(); // scanner.Origin - sourcePoints.ElementAt(1)

                    //scanner.Points.Clear();
                    //scanner.Points.AddRange(sourcePoints);

                    if (scanner.Origin is null)
                    {
                        scanner.Origin = unvisitedScanner.Origin! + validGroup.Key;
                        unvisitedScanners.Add(scanner);
                    }

                    foreach (var detectedPoint in detectedPoints)
                    //foreach (var detectedPoint in sourcePoints)
                    {
                        beaconsFromOrigin.Add(detectedPoint);
                        //beaconsFromOrigin.Add(detectedPoint + unvisitedScanner.Origin!);
                        //beaconsFromOrigin.Add(scanner.Origin - detectedPoint);
                        //beaconsFromOrigin.Add(scanner.Origin + detectedPoint);
                        //beaconsFromOrigin.Add(detectedPoint - scanner.Origin);
                    }
                }
            }
            alreadyVisitedScanners.Add(unvisitedScanner);
        }

        return new($"{beaconsFromOrigin.Count}");
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private IEnumerable<Scanner> ParseInput()
    {
        foreach (var scanner in ParsedFile.ReadAllGroupsOfLines(InputFilePath))
        {
            var points = new List<IntPoint3D>(scanner.Count - 1);

            for (int i = 1; i < scanner.Count; ++i)
            {
                var coordinates = scanner[i].Split(',', StringSplitOptions.TrimEntries);
                points.Add(new(int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2])));
            }

            yield return new Scanner(points);
        }
    }
}