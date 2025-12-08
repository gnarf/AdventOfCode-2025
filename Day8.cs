using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework.Internal;
namespace AoC2025;

class Day8 : Puzzle
{

    public List<Point3D> JunctionBoxes = new();

    public override void Parse(string filename)
    {
        base.Parse(filename);
        foreach (var l in lines)
        {
            var parts = l.Split(',').Select(s => long.Parse(s)).ToArray();
            JunctionBoxes.Add(new(parts[0], parts[1], parts[2]));
        }
    }

    public class Connection
    {
        public Point3D a;
        public Point3D b;
        public Connection(Point3D a, Point3D b) { this.a = a; this.b = b; }
        public double Distance() => (a-b).Magnitude();
        public override string ToString() => $"{a} {b} {(a-b).Magnitude():0.###}";
    }

    public override void Part1()
    {
        int ConnectionsToMake = extra == "example" ? 10 : 1000;

        List<Connection> Combinations = new();
        List<List<Connection>> Circuits = new(); 
        List<HashSet<Point3D>> CircuitMembers = new();
        for (int x=0; x<JunctionBoxes.Count-1; x++)
        {
            for (int y=x+1; y<JunctionBoxes.Count; y++)
            {
                Combinations.Add(new (JunctionBoxes[x], JunctionBoxes[y]));                
            }
        }

        Combinations.Sort( (a, b) => a.Distance().CompareTo(b.Distance()) );

        int nextTest = 0;
        while (ConnectionsToMake > 0)
        {
            // Console.WriteLine($"{ConnectionsToMake} connections left; {Circuits.Count} circuits; sizes {string.Join(", ", CircuitMembers.Select(m => m.Count))}");
            var test = Combinations[nextTest++];

            var circ1 = CircuitMembers.FindIndex(circ => circ.Contains(test.a));
            var circ2 = CircuitMembers.FindIndex(circ => circ.Contains(test.b));

            if (circ2 == -1)
            {
                if (circ1 == -1)
                {
                    // Console.WriteLine($"New Circuit: Connecting {test.a}#{circ1} and {test.b}#{circ2}");
                    Circuits.Add(new() { test });
                    CircuitMembers.Add(new () { test.a, test.b });
                }
                else
                {
                    // Console.WriteLine($"Existing Circuit: Connecting {test.a}#{circ1} and {test.b}#{circ2}");
                    Circuits[circ1].Add(test);
                    CircuitMembers[circ1].Add(test.b);
                }
            }
            else if (circ1 == -1)
            {
                // Console.WriteLine($"Existing Circuit: Connecting {test.a}#{circ1} and {test.b}#{circ2}");
                Circuits[circ2].Add(test);
                CircuitMembers[circ2].Add(test.a);
            }
            else if (circ1 == circ2)
            {
                // Console.WriteLine($"Already Connected: {test.a}#{circ1} and {test.b}#{circ2}");
            }
            else
            {
                // Console.WriteLine($"Connecting Circuits: {test.a}#{circ1} and {test.b}#{circ2}");
                Circuits[circ1].Add(test);
                Circuits[circ1].AddRange(Circuits[circ2]);
                CircuitMembers[circ1].AddRange(CircuitMembers[circ2]);
                Circuits.RemoveAt(circ2);
                CircuitMembers.RemoveAt(circ2);
            }
            ConnectionsToMake--;
        }
        // Console.WriteLine($"{ConnectionsToMake} connections left; {Circuits.Count} circuits; sizes {string.Join(", ", CircuitMembers.Select(m => m.Count))}");

        int revcomp(int a, int b) => b.CompareTo(a);
        var memCounts = CircuitMembers.Select(m=>m.Count).ToList();
        memCounts.Sort(revcomp);
        var result = memCounts.Take(3).Aggregate(1, (a, b) => a*b);
        Console.WriteLine(result);
    }

    public override void Part2()
    {
        List<Connection> Combinations = new();
        List<List<Connection>> Circuits = new(JunctionBoxes.Select(J => new List<Connection>())); 
        List<HashSet<Point3D>> CircuitMembers = new(JunctionBoxes.Select(J => new HashSet<Point3D>() { J }));
        for (int x=0; x<JunctionBoxes.Count-1; x++)
        {
            for (int y=x+1; y<JunctionBoxes.Count; y++)
            {
                Combinations.Add(new (JunctionBoxes[x], JunctionBoxes[y]));                
            }
        }

        Combinations.Sort( (a, b) => a.Distance().CompareTo(b.Distance()) );
        Connection last = null;
        foreach (var test in Combinations)
        {
            Console.WriteLine($"{Circuits.Count} circuits; sizes {string.Join(", ", CircuitMembers.Select(m => m.Count))}");

            var circ1 = CircuitMembers.FindIndex(circ => circ.Contains(test.a));
            var circ2 = CircuitMembers.FindIndex(circ => circ.Contains(test.b));

            if (circ2 == -1)
            {
                if (circ1 == -1)
                {
                    Console.WriteLine($"New Circuit: Connecting {test.a}#{circ1} and {test.b}#{circ2}");
                    Circuits.Add(new() { test });
                    CircuitMembers.Add(new () { test.a, test.b });
                }
                else
                {
                    Console.WriteLine($"Existing Circuit: Connecting {test.a}#{circ1} and {test.b}#{circ2}");
                    Circuits[circ1].Add(test);
                    CircuitMembers[circ1].Add(test.b);
                }
            }
            else if (circ1 == -1)
            {
                Console.WriteLine($"Existing Circuit: Connecting {test.a}#{circ1} and {test.b}#{circ2}");
                Circuits[circ2].Add(test);
                CircuitMembers[circ2].Add(test.a);
            }
            else if (circ1 == circ2)
            {
                Console.WriteLine($"Already Connected: {test.a}#{circ1} and {test.b}#{circ2}");
            }
            else
            {
                Console.WriteLine($"Connecting Circuits: {test.a}#{circ1} and {test.b}#{circ2}");
                Circuits[circ1].Add(test);
                Circuits[circ1].AddRange(Circuits[circ2]);
                CircuitMembers[circ1].AddRange(CircuitMembers[circ2]);
                Circuits.RemoveAt(circ2);
                CircuitMembers.RemoveAt(circ2);
            }
            if (Circuits.Count == 1)
            {
                last = test;
                break;
            }
        }
        Console.WriteLine($"{Circuits.Count} circuits; sizes {string.Join(", ", CircuitMembers.Select(m => m.Count))}");

        Console.WriteLine(last.a.x * last.b.x);
        
    }
}