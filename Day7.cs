using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
namespace AoC2025;

class Day7 : Puzzle
{

    public enum BoardState
    {
        Empty,
        Start,
        Splitter,
        Beam
    };
    public Point2D Start = new(0, 0);
    public int lastRow;
    public Dictionary<Point2D, BoardState> InitialBoard = new();
    public override void Parse(string filename)
    {
        base.Parse(filename);
        for(int y=0; y<lines.Length; y++ ) for (int x=0; x<lines[y].Length; x++)
        {                
            InitialBoard[new(x, y)] = lines[y][x] switch {
                'S' => BoardState.Start,
                '^' => BoardState.Splitter,
                _ => BoardState.Empty,
            };
            if (lines[y][x] == 'S') Start = new (x, y);
            lastRow = y;
        }
    }

    public override void Part1()
    {
        long splits = 0;
        Dictionary<Point2D, BoardState> Board = new(InitialBoard);
        HashSet<Point2D> points = new() { Start };
        HashSet<Point2D> next = new();

        while (points.Count > 0)
        {
            // Console.WriteLine($"Beams on {string.Join(",", points)}");
            next.Clear();
            foreach (var p in points)
            {
                // Console.WriteLine($"{p} {p + Point2D.Down}");
                if (Board.TryGetValue(p + Point2D.Down, out var state))
                {
                    if (state == BoardState.Splitter)
                    {
                        next.Add(p + Point2D.Down + Point2D.Left);
                        next.Add(p + Point2D.Down + Point2D.Right);
                        splits++;
                    }
                    else if (state == BoardState.Empty)
                    {
                        // Console.WriteLine($"whiff {p+Point2D.Down}");
                        next.Add(p + Point2D.Down);
                        
                    }
                }
                else
                {
                }
            }
            points.Clear();
            // Console.WriteLine($"next {string.Join(",", points)}");
            points.AddRange(next);
        }

        Console.WriteLine($"{splits}");
    }

    private void AddBeam(Dictionary<Point2D, long> Beams, Point2D Point, long Timelines)
    {
        if (Beams.TryGetValue(Point, out var already))
        {
            Timelines += already;
        }
        Beams[Point] = Timelines;
    }

    public override void Part2()
    {
        Dictionary<Point2D, BoardState> Board = new(InitialBoard);
        Dictionary<Point2D, long> Beams = new() { {Start, 1} };

        for (long y=Start.y; y<=lastRow; y++)
        {
            var iter = Beams.Keys.Where(k => k.y == y).ToList();
            foreach (var beam in iter)
            {
                var n = beam + Point2D.Down;
                if (Board.TryGetValue(n, out var boardState) && boardState == BoardState.Splitter)
                {
                    AddBeam(Beams, n + Point2D.Left, Beams[beam]);
                    AddBeam(Beams, n + Point2D.Right, Beams[beam]);
                }
                else
                {
                    AddBeam(Beams, n, Beams[beam]);
                }
            }
        }
        Console.WriteLine(Beams.Where(kv => kv.Key.y == lastRow).Select(kv => kv.Value).Sum());
    }
}