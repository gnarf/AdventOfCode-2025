using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
namespace AoC2025;

class Day5 : Puzzle
{
    List<Range> Ranges = new();    
    List<long> Ingredients = new();

    public override void Parse(string filename)
    {
        base.Parse(filename);
        bool ranges = true;
        foreach (var line in lines)
        {
            if (line.Length == 0)
            {
                ranges = false;
                continue;
            }
            if (ranges)
            {
                var split = line.Split('-');
                Ranges.Add(new Range(long.Parse(split[0]), long.Parse(split[1])));
            }
            else
            {
                Ingredients.Add(long.Parse(line));
            }
        }
    }

    public override void Part1()
    {
        Console.WriteLine(Ingredients.Where(i => Ranges.Any( r => i>=r.min & i<=r.max )).Count());
        foreach (var id in Ingredients)
        {

        }
    }

    public class Range
    {
        public long min;
        public long max;
        public Range(long min, long max)
        {
            this.min = Math.Min(min, max);
            this.max = Math.Max(max, min);
        }

        public bool IncludesAny(Range r2)
        {
            Console.WriteLine($" test {this} {r2} ");
            if (max < r2.min || min > r2.max) return false;
            return true;
        }

        public void Extend(Range r2)
        {
            min = Math.Min(min, r2.min);
            max = Math.Max(max, r2.max);
        }

        public long Size => max - min + 1;

        public override string ToString()
        {
            return $"<{min}-{max}> {Size}";
        }
    }

    public List<Range> ReduceRanges(List<Range> Input)
    {
        List<Range> ProcessedRanges = new();
        foreach (var range in Input)
        {
            foreach (var r2 in ProcessedRanges)
            {
                if (range.IncludesAny(r2))
                {
                    // Console.WriteLine($"Intersect: {range} {r2}");
                    r2.Extend(range);
                    // Console.WriteLine($"Extended: {r2}");
                    goto done;
                }
            }
            // Console.WriteLine($"No Intersect: {range}");
            ProcessedRanges.Add(range);

            done:;
        }
        return ProcessedRanges;
        
    }

    public override void Part2()
    {
        var collection = ReduceRanges(Ranges);
        for(int lastCount = 0;lastCount != collection.Count;)
        {
            lastCount = collection.Count;
            // Console.WriteLine($"Reduce again: {lastCount}");
            collection = ReduceRanges(collection);
            Console.WriteLine($"Collection now: {collection.Count}");
        }

        Console.WriteLine(collection.Aggregate( 0L, (long Sum, Range X) => Sum + X.Size));
        
        
    }
}