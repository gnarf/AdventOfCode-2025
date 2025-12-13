using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Google.OrTools.LinearSolver;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Google.OrTools.ConstraintSolver;
namespace AoC2025;

class Day12 : Puzzle
{

    public class Shape
    {
        public Point2D Size = new(0, 0);
        public List<Point2D> Points = new();
        public Shape() {}
        public Shape(IEnumerable<Point2D> points)
        {
            Points.AddRange(points);
            Size = Points.Aggregate(Point2D.Max);
        }
        public void Add(Point2D point)
        {
            Points.Add(point);
            Size = Points.Aggregate(Point2D.Max);
        }

        public override string ToString()
        {
            return Point2D.StringGrid(Points, c => Points.Contains(c) ? '#' : '.');
        }

        public Dictionary<Turn2D, Shape> Turns = new();
        public IEnumerable<Shape> AllTurns()
        {
            yield return this;
            yield return Turn(Turn2D.Left);
            yield return Turn(Turn2D.Around);
            yield return Turn(Turn2D.Right);            
        }
        public Shape Turn(Turn2D turn)
        {
            if (Turns.TryGetValue(turn, out var s))
            {
                return s;
            }
            if (turn == Turn2D.None)
            {
                Turns.Add(turn, this);
                return this;
            }
            var turnedPoints = Points.Select( p => p.Turn(turn) );
            var newOrigin = turnedPoints.Aggregate(Point2D.Min);
            s = new(turnedPoints.Select(p => p - newOrigin));
            Turns.Add(turn, s);
            return s;
        }

        public bool Fits(Dictionary<Point2D, char> grid, Point2D at)
        {
            foreach (var p in Points)
            {
                if (grid.ContainsKey(p + at)) return false;
            }
            return true;
        }

    }

    public class TreeRequest
    {
        public int Width;
        public int Height;
        public List<int> PackageRequests = new();
        public TreeRequest(int w, int h, IEnumerable<int> packages)
        {
            Width = w;
            Height = h;
            PackageRequests.AddRange(packages);
        }

        public override string ToString()
        {
            return $"{Width}x{Height}: {string.Join(" ", PackageRequests)}";
        }

        public bool TryFit(ref Dictionary<Point2D, char>? grid, ref int iterations, List<Shape>? shapes = null, int depth = 0)
        {
            if (shapes == null)
            {
                shapes = PackageRequests
                    .SelectMany((n, i) => Enumerable.Range(0, n)
                    .Select( n => Packages[i]))
                    .ToList();
                var packagePixels = shapes.Select(s => s.Points.Count).Sum();
                TimeCheck($"Checking {packagePixels} vs {Width * Height}");
                if (packagePixels > Width * Height) return false;
            }
            if (shapes.Count == 0)
            {
                return true;
            }
            var myGrid = new Dictionary<Point2D, char>();
            if (grid != null)
            {
                foreach (var p in grid)
                {
                    myGrid[p.Key] = p.Value;
                }
            }
            var max = new Point2D(Width - 1, Height -1);
            // if (depth == shapes.Count - 1)
            // {
            //     TimeCheck($"{shapes.Count} shapes into {Width}x{Height}");
            //     Point2D.PrintGrid(Point2D.Zero, max, point => myGrid.TryGetValue(point, out var c) ? c : '.');
            // }
            var shape = shapes[depth];
            var shapeMax = shape.Points.Aggregate(Point2D.Max);
            // the off by ones cancel
            iterations++;
            for(int x=0; x<=max.x - shapeMax.x; x++)
            {
                for (int y=0; y<=max.y - shapeMax.y; y++)
                {
                    var offset = new Point2D(x, y);
                    foreach (var turn in shape.AllTurns())
                    {
                        if (turn.Fits(myGrid, offset))
                        {
                            var copy = myGrid.ToDictionary(k => k.Key, v=>v.Value);

                            foreach (var p in turn.Points)
                            {
                                copy[p+offset] = '#';
                            }
                            if (shapes.Count > depth + 1)
                            {
                                if (TryFit(ref copy, ref iterations, shapes, depth + 1))
                                {
                                    grid = copy;
                                    return true;
                                }
                            }
                            else
                            {
                                grid = copy;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }

    public static List<Shape> Packages = new();
    public static List<TreeRequest> Requests = new();

    public override void Parse(string filename)
    {
        base.Parse(filename);
        int parseState = -1;
        var shape = new Shape();
        foreach (var line in lines)
        {
            if (parseState == -1)
            {
                if (line.IndexOf('x') != -1) // tree request
                {
                    parseState = -2;
                }
                else if (line.IndexOf(':') != -1)
                {
                    parseState = 0;
                    continue;                    
                }
            }
            if (parseState == -2)
            {
                var splits = line.Split(':');
                var size = splits[0].Split('x').Select(int.Parse).ToArray();
                Requests.Add(new(size[0], size[1], splits[1].Trim().Split(' ').Select(int.Parse)));
            }
            if (parseState >= 0)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    parseState = -1;
                    Packages.Add(shape);
                    shape = new();
                    continue;
                }
                for (int x=0; x<line.Length; x++)
                {
                    if (line[x] == '#')
                    {
                        shape.Add(new(x, parseState));
                    }
                }
                parseState++;
            }
        }
    }

    public override void Part1()
    {
        Dictionary<Point2D, char>? grid = null;
        int fit = 0;
        int l = 0;
        int iterations = 0;
        TimeCheck($"starting");
        foreach (var r in Requests)
        {
            TimeCheck($"Starting #{++l} {r}");
            iterations = 0;
            if (r.TryFit(ref grid, ref iterations))
            {
                fit++;
                TimeCheck($"Finished {l} {r} {iterations}");
                // Point2D.PrintGrid(grid.Keys, f => grid.TryGetValue(f, out var c) ? c : '.');
                grid.Clear();
            }
        }
        Console.WriteLine(fit);
    }

    // public override void Part2()
    // {
    // }
}