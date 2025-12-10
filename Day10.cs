using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using NUnit.Framework.Internal;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
namespace AoC2025;

class Day10 : Puzzle
{

    public class Machine
    {
        public List<bool> Lights = new();
        public List<bool> RequiredPattern = new();
        public List<List<int>> Buttons = new();
        public List<int> Joltage = new();

        public Machine(string inputLine)
        {
            var parts = inputLine.Split(' ');
            RequiredPattern.AddRange(parts[0].Trim('[',']').Select(c => c=='#'));
            Joltage.AddRange(parts[^1].Trim('{','}').Split(',').Select(int.Parse));
            Buttons.AddRange(
                parts[1..^1].Select(s => s.Trim('(',')').Split(',').Select(int.Parse).ToList())
            );

            // Buttons.Sort( (a, b) => GetButtonDesirability(a).CompareTo(GetButtonDesirability(b)) );
            Lights.AddRange(RequiredPattern.Select(_ => false));
        }

        public int GetButtonDesirability(List<int> button)
        {
            return button.Aggregate(0, (m, b) => m + Joltage[b]);
        }
        public string RenderButton(List<int> b) => $"({string.Join(',', b)})";
        public string RenderLights(IEnumerable<bool> l) => "[" + l.Select(l => l ? '#' : '.').Aggregate("", (a,b) => a+b) + "]";
        public string RenderJoltage()
        {
            return "{" + string.Join(',', Joltage) + "}";
        }

        public List<int> GetLeastButtonPressesNeededForLights()
        {
            Queue<(bool[] Lights, List<int> Buttons)> queue = new();
            queue.Enqueue( (Lights.ToArray(), new(){}) );

            while (queue.Count > 0)
            {
                var test = queue.Dequeue();
                for (int button=0; button<Buttons.Count; button++)
                {
                    var flips = Buttons[button];
                    var newState = test.Lights.ToArray();
                    foreach (var l in flips) { newState[l] = !newState[l]; }
                    var buttons = test.Buttons.Append(button).ToList();
                    // Console.WriteLine($"Flip Sequence: {string.Join(",", buttons) } {RenderLights(newState)} ");
                    if (newState.SequenceEqual(RequiredPattern))
                    {
                        return buttons;
                    }
                    else
                    {
                        queue.Enqueue( (newState, buttons) );
                    }
                }
            }

            return new();
        }

        public bool OverJolted(int[] acc)
        {
            return acc.Select((n, i) => n > Joltage[i]).Any(b => b);
        }

        public bool Jolted(int[] acc)
        {
            return Joltage.SequenceEqual(acc);
        }

        public int[] CalcJoltage(List<int> presses)
        {
            int[] acc = new int[Joltage.Count];
            for(int x=0; x<Buttons.Count; x++)
            {
                for (int y=0; y<Buttons[x].Count; y++)
                {
                    acc[Buttons[x][y]] += presses[x];
                }
            }
            return acc;
        }

        public class ListStack<T> : List<T>
        {
            public void Push(T item) => Add(item);
            public T Pop()
            {
                var n = this[^1];
                RemoveAt(Count - 1);
                return n;
            }
        }

        public List<int> GetLeastButtonPressesNeededForJoltage()
        {
            var Buttons = this.Buttons.ToList();
            ListStack<(int[] Jolts, List<int> presses, int nextButton)> Stack = new();
            Stack.Push( (Joltage.Select(n=>0).ToArray(), Buttons.Select(b => 0).ToList(), 0) );
            List<int> shortestButtons = new();
            int shortestCount = int.MaxValue;
            long iters = 0;

            while (Stack.Count > 0)
            {
                var test = Stack.Pop();
                if (test.presses.Sum() >= shortestCount)
                {
                    continue;
                }
                if (++iters % 100000 == 0)
                {
                    TimeCheck($"{Thread.CurrentThread.ManagedThreadId:###} Iterations: {iters/1000:#########}k / {Stack.Count} {string.Join(",", test.presses)} {string.Join(",",test.Jolts)} {test.nextButton}");
                }
                if (Jolted(test.Jolts))
                {
                    shortestButtons = test.presses.SelectMany((n, i) => Enumerable.Range(0, n).Select(n => i)).ToList();
                    shortestCount = shortestButtons.Count;
                    TimeCheck($"{Thread.CurrentThread.ManagedThreadId:###} Max Shortest! {shortestCount}");
                    continue;
                    // if (shortestCount == Joltage.Max()) 
                    //     return shortestButtons;
                }
                if (test.nextButton >= Buttons.Count) continue;
                var c = test.presses.Sum();
                var max = Math.Min(shortestCount, Joltage.Sum());
                for (int presses=1; presses <= max - c; presses++)
                {
                    // var i = Stack.FindIndex(c => c.presses.Sum() >= test.presses.Sum() + presses);

                    for (int button=test.nextButton; button<Buttons.Count; button++)
                    {
                        // add an entry for the stack for each state of pressing this button n times up until we run out
                        var flips = Buttons[button];
                        var maxPressCount = flips.Select(f => Joltage[f] - test.Jolts[f]).Min(); // min because the rest go over
                        if (maxPressCount < presses) continue;
                        var newPress = test.presses.ToList();
                        newPress[button] += presses;
                        var pressed = newPress.Sum();
                        var jolts = CalcJoltage(newPress);
                        if (OverJolted(jolts)) continue;
                        var entry = (jolts, newPress, button + 1);
                        if (button == Buttons.Count - 1 && presses != maxPressCount)
                        {
                            break;
                        }
                        // if (i == -1)
                        // {
                            Stack.Push(entry);
                        // }
                        // else
                        // {
                        //     Stack.Insert(i, entry);
                        // }
                    }
                }
            }
            

            return shortestButtons;
        }


        public override string ToString()
        {
            return RenderLights(RequiredPattern) + " " + string.Join(' ', Buttons.Select(RenderButton)) + " " + RenderJoltage(); 
        }

    }

    public List<Machine> Machines = new();

    public override void Parse(string filename)
    {
        base.Parse(filename);
        foreach(var line in lines) Machines.Add(new Machine(line));
    }

    public override void Part1()
    {
        // TimeCheck("Machine Tests");
        // int totalPresses = 0;
        // foreach (var m in Machines)
        // {
        //     // Console.WriteLine(m);
        //     var presses = m.GetLeastButtonPressesNeededForLights();
        //     totalPresses += presses.Count;
        //     TimeCheck("Machine " + m);
        //     // Console.WriteLine(string.Join(",", presses) + " " + presses.Count);
        // }
        // // foreach (var line in lines)
        // // {

        // // }
        // Console.WriteLine(totalPresses);
    }


    public override void Part2()
    {
        bool cacheEnabled = true;
        Dictionary<string, int> solutions = new();
        void WriteCache(string m, int count)
        {
            if (!cacheEnabled) return;
            lock (solutions)
            {
                solutions[m] = count;
                using (var writer = new StreamWriter("cache"))
                {
                    writer.Write(JsonConvert.SerializeObject(solutions, Formatting.Indented));
                }
            }
        }
        if (cacheEnabled && !File.Exists("cache"))
        {
            WriteCache("", 0);
        }
        if (cacheEnabled)
        {
            using (var reader = new StreamReader("cache"))
            {
                solutions = JsonConvert.DeserializeObject<Dictionary<string, int>>(reader.ReadToEnd());
            }
        }
        TimeCheck("Machine Tests");
        int totalPresses = 0;
        var opts = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
        Parallel.ForEach(Machines, opts, m =>
        {
            if (cacheEnabled)
            {
                lock(solutions)
                {
                    if (solutions.TryGetValue(m.ToString(), out var cached))
                    {
                        TimeCheck($"{Thread.CurrentThread.ManagedThreadId:###} Got cache {m} {cached}");
                        Interlocked.Add(ref totalPresses, cached);
                        return;
                    }
                }
            }
            var presses = m.GetLeastButtonPressesNeededForJoltage();
            WriteCache(m.ToString(), presses.Count);
            Interlocked.Add(ref totalPresses, presses.Count);
            TimeCheck($"{Thread.CurrentThread.ManagedThreadId:###} {string.Join(",", presses)} {presses.Count}");
        });
        Console.WriteLine(totalPresses);
    }
}