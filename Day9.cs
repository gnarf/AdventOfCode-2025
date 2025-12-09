using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Data.Common;
namespace AoC2025;

class Day9 : Puzzle
{

    public List<Point2D> RedTiles = new();
    public override void Parse(string filename)
    {
        base.Parse(filename);
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            RedTiles.Add(new(long.Parse(parts[0]),long.Parse(parts[1])));
        }
    }

    public class Box2D
    {
        public Point2D a;
        public Point2D b;

        public Box2D(Point2D a, Point2D b)
        {
            this.a = Point2D.Min(a, b);
            this.b = Point2D.Max(a, b);
        }

        public long Area()
        {
            var diff = (b-a);
            return (Math.Abs(diff.x) + 1) * (Math.Abs(diff.y) + 1);
        }

        public bool AnyInside(Predicate<Point2D> f)
        {
            for (long x=a.x; x<=b.x; x++) for (long y=a.y; y<=b.y; y++)
                {
                    if (f(new(x, y))) return true;
                }
            return false;
        }

        public bool Inside(Point2D p)
        {
            return (p.x > a.x && p.x < b.x) && (p.y > a.y && p.y < b.y);
        }
    }

    public override void Part1()
    {

        List<Box2D> Boxes = new();
        for (int x=0; x<RedTiles.Count; x++)
        {
            for (int y=x+1; y<RedTiles.Count; y++)
            {
                Boxes.Add(new (RedTiles[x], RedTiles[y]));
            }
        }
        Boxes.Sort( (a, b) => b.Area().CompareTo(a.Area()) );

        Console.WriteLine(Boxes[0].Area());
    }

    public enum TileType
    {
        Red, Green
    }

    public override void Part2()
    {
        Dictionary<Point2D, TileType> Tiles = new();
        HashSet<Point2D> flood = new();
        for (int x=0; x<RedTiles.Count; x++)
        {
            var start = RedTiles[x];
            var end = x+1 == RedTiles.Count ? RedTiles[0] : RedTiles[x+1];
            var move = (end-start).Sign();
            for (var p = start+move; p!=end; p+=move)
            {
                Tiles[p] = TileType.Green;
            }
            Tiles[start] = TileType.Red;
        }

        var domainLower = Tiles.Keys.Aggregate(Point2D.Min);
        var domainUpper = Tiles.Keys.Aggregate(Point2D.Max);

        // flood.Add(new Point2D(domainUpper.x - domainLower.x / 2, domainUpper.y - domainLower.y / 2) + domainLower);
        // int floodCount = 0;
        // while (flood.Count > 0)
        // {
        //     var f = flood.First();
        //     flood.Remove(f);
        //     Tiles.Add(f, TileType.Green);
        //     foreach (var f2 in f.CardinalCells())
        //     {
        //         if (!Tiles.ContainsKey(f2)) flood.Add(f2);
        //     }
        //     if (floodCount++ % 1000 == 0) { TimeCheck($"Flood Count {flood.Count}"); }
        // }

        // for (long y=domainLower.y; y<= domainUpper.y; y++)
        // {
        //     bool parity = true;
        //     bool lastInside = false;
        //     if (y%100 == 0) Console.WriteLine($"Line {y}");
        //     for (long x=domainLower.x; x<= domainUpper.x; x++)
        //     {
        //         var inside = Tiles.ContainsKey(new (x, y));
        //         if (inside != lastInside)
        //         {
        //             parity = !parity;
        //         }
        //         lastInside = inside;
        //         if (parity && !Tiles.ContainsKey(new (x, y)))
        //         {
        //             flood.Add(new (x,y));
        //         }
        //     }
        // }

        List<Box2D> Boxes = new();
        for (int x=0; x<RedTiles.Count; x++)
        {
            for (int y2=x+1; y2<RedTiles.Count; y2++)
            {
                Boxes.Add(new (RedTiles[x], RedTiles[y2]));
            }
        }
        Boxes.Sort( (a, b) => b.Area().CompareTo(a.Area()) );

        bool TestBox(Box2D box)
        {
            for (int x=0; x<RedTiles.Count; x++)
            {
                var s = RedTiles[x];
                var e = RedTiles[(x+1) % RedTiles.Count];
                var d = (e-s).Sign();
                if (d.x == 1 || d.x == -1)
                {
                    if (s.y <= box.a.y || s.y >= box.b.y) continue;
                    while (s != e + d)
                    {
                        if (s.x > box.a.x && s.x < box.b.x) return false;
                        s+=d;
                    }
                }
                else
                {
                    if (s.x <= box.a.x || s.x >= box.b.x) continue;
                    while (s != e + d)
                    {
                        if (s.y > box.a.y && s.y < box.b.y) return false;
                        s+=d;
                    }
                }
            }
            return true;            
        }

        foreach (var box in Boxes)
        {
            // Console.WriteLine($"Test: {box.a} {box.b} {box.Area()}");
            if (TestBox(box))
            {
                Console.WriteLine($"Test: {box.a} {box.b} {box.Area()} WINNER!");
                return;
            }
        }

        // Point2D.PrintGrid(Tiles.Keys, p => Tiles.TryGetValue(p, out var type) ? type switch { TileType.Red => '#', TileType.Green => 'X' } : flood.Contains(p) ? 'f' : '.');

    }
}