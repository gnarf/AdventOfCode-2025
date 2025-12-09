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
        // TimeCheck("Building Boxes");
        List<Box2D> Boxes = new();
        for (int x=0; x<RedTiles.Count; x++)
        {
            for (int y2=x+1; y2<RedTiles.Count; y2++)
            {
                Boxes.Add(new (RedTiles[x], RedTiles[y2]));
            }
        }
        // TimeCheck("Sorting Boxes");
        Boxes.Sort( (a, b) => b.Area().CompareTo(a.Area()) );

        bool TestBox(Box2D box)
        {
            for (int x=0; x<RedTiles.Count; x++)
            {
                var s = RedTiles[x];
                var e = RedTiles[(x+1) % RedTiles.Count];
                var d = (e-s).Sign();
                // this segment of the poly moves left<->right
                if (d.x == 1 || d.x == -1)
                {
                    // fully outside box
                    if (s.y <= box.a.y || s.y >= box.b.y) continue;
                    if (s.x >= box.b.x && e.x >= box.b.x) continue;
                    if (s.x <= box.a.x && e.x <= box.a.x) continue;
                    // crosses into box 
                    if (s.x > box.a.x && s.x < box.b.x) return false;
                    if (e.x > box.a.x && e.x < box.b.x) return false;
                    if (s.x <= box.a.x && e.x >= box.b.x) return false;
                    if (e.x <= box.a.x && s.x >= box.b.x) return false;
                }
                // this segment of the poly moves up/down
                else
                {
                    // fully outside box
                    if (s.x <= box.a.x || s.x >= box.b.x) continue;
                    if (s.y >= box.b.y && e.y >= box.b.y) continue;
                    if (s.y <= box.a.y && e.y <= box.a.y) continue;
                    // crosses into box 
                    if (s.y > box.a.y && s.y < box.b.y) return false;
                    if (e.y > box.a.y && e.y < box.b.y) return false;
                    if (s.y <= box.a.y && e.y >= box.b.y) return false;
                    if (e.y <= box.a.y && s.y >= box.b.y) return false;
                }
                throw new Exception($"Didn't quick exit for line from {s} to {e} and box {box.a} {box.b}");
            }
            return true;            
        }

        int boxes = 0;
        // TimeCheck("Starting Box Check");
        foreach (var box in Boxes)
        {
            if ((++boxes % 1000) == 0)
            {
                // TimeCheck($"Parsed {boxes} of {Boxes.Count} boxes so far, still looking.");
            }
            // Console.WriteLine($"Test: {box.a} {box.b} {box.Area()}");
            if (TestBox(box))
            {
                Console.WriteLine(box.Area());
                break;
            }
        }
    }
}