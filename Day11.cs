using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
namespace AoC2025;

class Day11 : Puzzle
{

    public class Connection
    {
        public string From = "";
        public List<string> To = new();

        public Connection(string From, IEnumerable<string> To)
        {
            this.From = From;
            this.To.AddRange(To);
        }
    }

    public Dictionary<string, Connection> ServerRack = new();

    public override void Parse(string filename)
    {
        base.Parse(filename);
        foreach (var line in lines)
        {
            var name = line[0..3];
            ServerRack.Add(name, new Connection(name, line[5..].Split(' ')));
        }
        ServerRack.Add("out", new("out", Enumerable.Empty<string>()));
    }

    public override void Part1()
    {
        Queue<(string node, string path, int depth)> bfs = new();
        bfs.Enqueue(("you", "you", 0));

        HashSet<string> FullPath = new();
        while (bfs.Count > 0)
        {
            var t = bfs.Dequeue();
            foreach (var next in ServerRack[t.node].To)
            {
                var path = t.path + " " + next;
                if (next == "out")
                {
                    FullPath.Add(path);
                }
                else
                {
                    bfs.Enqueue((next, path, t.depth + 1));
                }
            }
        }

        Console.WriteLine(FullPath.Count);
    }

    public class SwitchQueue<T> : List<T>
    {
        public void Enqueue(T item) => Add(item);
        public T Dequeue()
        {
            var item = this[0];
            RemoveAt(0);
            return item;
        }
        public void RemoveWhere(Predicate<T> f)
        {
            for (int x=0; x<Count - 1;)
            {
                if (f(this[x])) RemoveAt(x); else x++;
            }
        }
    }

    public Dictionary<string, long> PathsThrough = new();

    public long CalcPathsThrough(Connection c, bool dac = false, bool fft = false)
    {
        var hash = $"{c.From} {dac} {fft}";
        if (PathsThrough.TryGetValue(hash, out var count))
        {
            return count;
        }
        // TimeCheck($"Traversing {hash}");
        var result = c.To.Sum(n => {
            if (n =="out") return fft && dac ? 1 : 0;
            return CalcPathsThrough(ServerRack[n], dac || c.From == "dac", fft || c.From == "fft");
        });
        PathsThrough.Add(hash, result);
        return result;
    }

    public override void Part2()
    {
        Console.WriteLine(CalcPathsThrough(ServerRack["svr"]));
    }
}