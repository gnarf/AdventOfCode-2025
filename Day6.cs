using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
namespace AoC2025;

class Day6 : Puzzle
{

    public class WorkSheetColumn
    {
        public List<long> Numbers = new();
        public string Operator = "";
        public override string ToString()
        {
            return $"{string.Join(Operator, Numbers)} = {Total()}";
        }
        public long Total()
        {
            if (Operator == "*")
            {
                return Numbers.Aggregate(1L, (memo, number) => memo * number);
            }
            else
            {
                return Numbers.Aggregate(0L, (memo, number) => memo + number);
            }
            
        }
    }

    public List<WorkSheetColumn> Columns = new();

    public override void Parse(string filename)
    {
        base.Parse(filename);
        for (int x=0; x<lines.Length; x++)
        {
            var entries = lines[x].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int y=0; y<entries.Length; y++)
            {
                WorkSheetColumn C = x == 0 ? new() : Columns[y];
                if (x==0) Columns.Add(C);
                if (x<lines.Length - 1) C.Numbers.Add(long.Parse(entries[y]));
                else C.Operator = entries[y];
            }
        }
    }

    public override void Part1()
    {
        long total = 0;
        foreach (var C in Columns)
        {
            // Console.WriteLine(C);
            if (C.Operator == "*")
            {
                total += C.Numbers.Aggregate(1L, (memo, number) => memo * number);
            }
            else
            {
                total += C.Numbers.Aggregate(0L, (memo, number) => memo + number);
            }
        }
        Console.WriteLine(total);
    }

    
    public override void Part2()
    {
        Dictionary<Point2D, char> chars = new();
        int width = 0;
        int height = 0;
        for(int x=0;x<lines.Length; x++)
        {
            for (int y=0; y<lines[x].Length;y++)
            {
                chars[new(y, x)] = lines[x][y];
                width = Math.Max(width, y);
            }
            height = x;
        }

        // Console.WriteLine($"{width}x{height}");
        List<WorkSheetColumn> P2 = new();
        void ParseBox(int x1, int x2)
        {
            string n;
            var C = new WorkSheetColumn { Operator = chars[new(x1, height)].ToString() };
            for (int x = x2; x>=x1; x--)
            {
                n = "";
                for (int y = height - 1; y>=0; y--)
                {
                    var c = chars[new(x, y)];
                    if (c != ' ')
                    {
                        n = c + n;
                    }
                }
                C.Numbers.Add(long.Parse(n));
            }
            P2.Add(C);
            // Console.WriteLine($"ParseBox {x1} {x2} {C}");
        }
        int lastX = -1;
        for (int x = 0; x<=width;x++)
        {
            if (chars[new(x, height)] != ' ')
            {
                if (lastX != -1)
                {
                    ParseBox(lastX, x-2);
                }
                lastX = x;
            }
        }
        ParseBox(lastX, width);
        long total = 0;
        foreach (var C in P2)
        {
            total += C.Total();
        }
        Console.WriteLine(total);
    }
}