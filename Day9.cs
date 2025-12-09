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
        public Point2D UL;
        public Point2D LR;
        public long Area;

        public Box2D(Point2D a, Point2D b)
        {
            this.UL = Point2D.Min(a, b);
            this.LR = Point2D.Max(a, b);
            var diff = (b-a);
            Area = (Math.Abs(diff.x) + 1) * (Math.Abs(diff.y) + 1);
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
        Boxes.Sort( (a, b) => b.Area.CompareTo(a.Area) );

        Console.WriteLine(Boxes[0].Area);
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
        Boxes.Sort( (a, b) => b.Area.CompareTo(a.Area) );

        // int boxes = 0;
        // TimeCheck("Starting Box Check");
        foreach (var box in Boxes)
        {
            // if ((++boxes % 100) == 0)
            // {
            //     TimeCheck($"Parsed {boxes} of {Boxes.Count} boxes so far, still looking.");
            // }
            // Console.WriteLine($"Test: {box.a} {box.b} {box.Area()}");
            if (TestBox(box))
            {
                Console.WriteLine(box.Area);
                break;
            }
        }
    }

    bool TestBox(Box2D box)
    {
        // check each segment of the polygon to see if it crosses into our box
        for (int x=0; x<RedTiles.Count; x++)
        {
            var start = RedTiles[x];
            var end = RedTiles[(x+1) % RedTiles.Count];
            var diff = end-start;
            // this segment of the poly moves left<->right
            if (diff.x != 0 )
            {
                // fully outside box (or on our "edge")
                if (start.y <= box.UL.y || start.y >= box.LR.y) continue;
                if (start.x >= box.LR.x && end.x >= box.LR.x) continue;
                if (start.x <= box.UL.x && end.x <= box.UL.x) continue;
                // starts or ends in the heart of the box 
                if (start.x > box.UL.x && start.x < box.LR.x) return false;
                if (end.x > box.UL.x && end.x < box.LR.x) return false;
                // spans across our box
                if (start.x <= box.UL.x && end.x >= box.LR.x) return false;
                if (end.x <= box.UL.x && start.x >= box.LR.x) return false;
            }
            // this segment of the poly moves up/down
            else
            {
                // fully outside box (or on our "edge")
                if (start.x <= box.UL.x || start.x >= box.LR.x) continue;
                if (start.y >= box.LR.y && end.y >= box.LR.y) continue;
                if (start.y <= box.UL.y && end.y <= box.UL.y) continue;
                // starts or ends in the heart of the box 
                if (start.y > box.UL.y && start.y < box.LR.y) return false;
                if (end.y > box.UL.y && end.y < box.LR.y) return false;
                // spans across our box
                if (start.y <= box.UL.y && end.y >= box.LR.y) return false;
                if (end.y <= box.UL.y && start.y >= box.LR.y) return false;
            }
            throw new Exception($"Didn't quick exit for line from {start} to {end} and box {box.UL} {box.LR}");
        }
        return true;            
    }

}