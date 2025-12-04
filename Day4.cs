using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
namespace AoC2025;

class Day4 : Puzzle
{

    public HashSet<Point2D> grid = new();

    public override void Parse(string filename)
    {
        base.Parse(filename);

        for (int y=0; y<lines.Length; y++)
        {
            for (int x=0; x<lines[y].Length; x++)
            {
                if (lines[y][x] == '@')
                    grid.Add(new Point2D(x, y));
            }
        }
    }

    public override void Part1()
    {
        Console.WriteLine(grid.Where(point => point.NearbyCells().Count(p2 => grid.Contains(p2)) < 4).Count());
    }

    public override void Part2()
    {
        var grid2 = grid.ToHashSet();
        long count = 0;
        List<Point2D> remove;
        do
        {
            remove = grid2.Where(point => point.NearbyCells().Count(p2 => grid2.Contains(p2)) < 4).ToList();
            count += remove.Count;
            foreach (var r in remove)
            {
                grid2.Remove(r);
            }

        }
        while (remove.Count > 0);

        Console.WriteLine(count);
    }
}