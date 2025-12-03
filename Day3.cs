using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
namespace AoC2025;

class Day3 : Puzzle
{

    // public override void Parse(string filename)
    // {
    //     base.Parse(filename);
    // }

    public override void Part1()
    {
        long Total = 0;
        foreach (var line in lines)
        {
            int highest = 0;
            for (int x=0; x<line.Length; x++)
            {
                var c1 = line[x];
                for (int y=x+1; y<line.Length; y++)
                {
                    var c2 = line[y];
                    int test = int.Parse(line[x] + "" + line[y]);
                    if (test > highest)
                    {
                        // Console.WriteLine($"new high {highest} {x}/{y}");
                        highest = test;
                    }
                }
            }
            Total += highest;
        }
        Console.WriteLine(Total);
    }

    public (int index, char c) IndexOfHighestCharInRange(ReadOnlySpan<char> Span)
    {
        char highest = '\0';
        int Index = -1;
        for (int x=0; x<Span.Length; x++)
        {
            if (Span[x] > highest)
            {
                highest = Span[x];
                Index = x;
            }
        }
        // Console.WriteLine($"Highest in range {Span} is {highest}@{Index}");
        return (Index, highest);
    }

    public override void Part2()
    {
        long Total = 0;
        foreach (var line in lines)
        {
            var span = line.AsSpan();
            string collectedHighest = "";
            int start = 0;
            for (int x = 0; x<12; x++)
            {
                var result = IndexOfHighestCharInRange(span[start..(line.Length - 11 + x)]);
                start = start + result.index + 1;
                collectedHighest += result.c;
            }
            Console.WriteLine($"Overall {line} {collectedHighest}");
            Total += long.Parse(collectedHighest);
        }
        Console.WriteLine(Total);

    }
}