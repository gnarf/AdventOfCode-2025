using NUnit.Framework;
using NUnit.Framework.Internal;
namespace AoC2025;

class Day2 : Puzzle
{
    public bool IsValidID(long ID)
    {
        var str = ID.ToString();
        if (str.Length % 2 == 1) return true;
        if (str[0..(str.Length/2)] == str[(str.Length / 2)..]) return false;
        return true;
    }

    public override void Part1()
    {

        var ranges = lines[0].Split(",");
        long total = 0;
        foreach (var range in ranges)
        {
            var test = range.Split('-');
            var start = long.Parse(test[0]);
            var end = long.Parse(test[1]);
            Console.WriteLine($"Testing {start} - {end}");
            for (long x = start; x<= end; x++ )
            {
                if (!IsValidID(x))
                {
                    total += x;
                    Console.WriteLine($"Invalid ID {x}");
                }
            }
        }
        Console.WriteLine($"Total: {total}");
    }

    public bool IsValidID2(long ID)
    {
        var str = ID.ToString();
        for (int test = 1; test <= str.Length / 2; test++)
        {
            if (str.Length % test != 0) continue;
            var times = str.Length / test;
            var repeat = str[0..test];
            var onlyRepeats = true;
            for (int x=1; x<times && onlyRepeats; x++)
            {
                // Console.WriteLine($"Testing {str[(x*test)..(test*(x+1))]} vs {repeat} ({x} {times})");
                onlyRepeats = str[(x*test)..(test*(x+1))] == repeat;
            }
            if (onlyRepeats) return false;
        }
        return true;
    }


    public override void Part2()
    {
        var ranges = lines[0].Split(",");
        long total = 0;
        foreach (var range in ranges)
        {
            var test = range.Split('-');
            var start = long.Parse(test[0]);
            var end = long.Parse(test[1]);
            Console.WriteLine($"Testing {start} - {end}");
            for (long x = start; x<= end; x++ )
            {
                if (!IsValidID2(x))
                {
                    total += x;
                    Console.WriteLine($"Invalid ID {x}");
                }
            }
        }
        Console.WriteLine($"Total: {total}");
   }
}