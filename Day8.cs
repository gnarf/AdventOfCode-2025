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
    public List<Connection> Combinations = new();

    public override void Parse(string filename)
    {
        base.Parse(filename);
        foreach (var l in lines)
        {
            var parts = l.Split(',').Select(s => long.Parse(s)).ToArray();
            JunctionBoxes.Add(new(parts[0], parts[1], parts[2]));
        }

        for (int x=0; x<JunctionBoxes.Count-1; x++)
        {
            for (int y=x+1; y<JunctionBoxes.Count; y++)
            {
                Combinations.Add(new (JunctionBoxes[x], JunctionBoxes[y]));                
            }
        }

        Combinations.Sort( (a, b) => a.Distance().CompareTo(b.Distance()) );
    }

    public class Connection
    {
        public Point3D a;
        public Point3D b;
        public Connection(Point3D a, Point3D b) { this.a = a; this.b = b; }
        public double Distance() => (a-b).Magnitude();
        public override string ToString() => $"{a} {b} {(a-b).Magnitude():0.###}";
    }

    public class Circuit
    {
        public HashSet<Point3D> Members = new();
        public List<Connection> Connections = new();
        public Circuit(Point3D JunctionBox)
        {
            Members.Add(JunctionBox);
        }

        public bool Contains(Point3D Box) => Members.Contains(Box);
        public void AddCircuit(Circuit other, Connection cable)
        {
            Members.AddRange(other.Members);
            Connections.Add(cable);
            Connections.AddRange(other.Connections);
        }
    }

    public override void Part1()
    {
        int ConnectionsToMake = extra == "example" ? 10 : 1000;

        List<Circuit> Circuits = new(JunctionBoxes.Select(box => new Circuit(box)));

        int nextTest = 0;
        while (ConnectionsToMake > 0)
        {
            // Console.WriteLine($"{Circuits.Count} circuits; sizes {string.Join(", ", Circuits.Select(m => m.Members.Count))}");
            var test = Combinations[nextTest++];

            var circ1 = Circuits.FindIndex(circ => circ.Contains(test.a));
            var circ2 = Circuits.FindIndex(circ => circ.Contains(test.b));

            if (circ1 == circ2)
            {
                // Console.WriteLine($"Already Connected: {test.a}#{circ1} and {test.b}#{circ2}");
            }
            else
            {
                // Console.WriteLine($"Connecting Circuits: {test.a}#{circ1} and {test.b}#{circ2}");
                Circuits[circ1].AddCircuit(Circuits[circ2], test);
                Circuits.RemoveAt(circ2);
            }
            ConnectionsToMake--;
        }
        // Console.WriteLine($"{Circuits.Count} circuits; sizes {string.Join(", ", Circuits.Select(m => m.Members.Count))}");

        int revcomp(int a, int b) => b.CompareTo(a);
        var memCounts = Circuits.Select(m=>m.Members.Count).ToList();
        memCounts.Sort(revcomp);
        var result = memCounts.Take(3).Aggregate(1, (a, b) => a*b);
        Console.WriteLine(result);
    }

    public override void Part2()
    {
        Connection last = null;
        List<Circuit> Circuits = new(JunctionBoxes.Select(J => new Circuit(J)));
        foreach (var test in Combinations)
        {
            // Console.WriteLine($"{Circuits.Count} circuits; sizes {string.Join(", ", Circuits.Select(m => m.Members.Count))}");

            var circ1 = Circuits.FindIndex(circ => circ.Contains(test.a));
            var circ2 = Circuits.FindIndex(circ => circ.Contains(test.b));

            if (circ1 == circ2)
            {
                // Console.WriteLine($"Already Connected: {test.a}#{circ1} and {test.b}#{circ2}");
            }
            else
            {
                // Console.WriteLine($"Connecting Circuits: {test.a}#{circ1} and {test.b}#{circ2}");
                Circuits[circ1].AddCircuit(Circuits[circ2], test);
                Circuits.RemoveAt(circ2);
            }
            if (Circuits.Count == 1)
            {
                last = test;
                break;
            }
        }
        // Console.WriteLine($"{Circuits.Count} circuits; sizes {string.Join(", ", Circuits.Select(m => m.Members.Count))}");

        Console.WriteLine(last.a.x * last.b.x);
        
    }
}