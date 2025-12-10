using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using NUnit.Framework.Internal;
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

            Buttons.Sort( (a, b) => GetButtonDesirability(b).CompareTo(GetButtonDesirability(a)) );
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

        public List<int> GetLeastButtonPressesNeededForJoltage()
        {
            var Buttons = this.Buttons.ToList();
            Stack<(int[] Jolts, List<int> Buttons)> Stack = new();
            Stack.Push( (Joltage.Select(n=>0).ToArray(), new(){}) );
            List<int> shortestButtons = new();
            int shortestCount = int.MaxValue;
            long iters = 0;

            while (Stack.Count > 0)
            {
                if (++iters % 10000000 == 0)
                {
                    TimeCheck($"Iterations: {iters} / {Stack.Count}");
                }
                var test = Stack.Pop();
                if (test.Buttons.Count >= shortestCount) continue;
                // Console.WriteLine($"Flip Sequence: /{string.Join(",", buttons.Select(b => b))}\\ -{string.Join(",", newState)}- {RenderJoltage()}");
                if (test.Jolts.SequenceEqual(Joltage))
                {
                    shortestButtons = test.Buttons;
                    shortestCount = test.Buttons.Count;
                    TimeCheck($"New Shortest! {shortestCount}");
                    continue;
                }

                for (int button=test.Buttons.Count > 0 ? test.Buttons[^1] + 1 : 0; button<Buttons.Count; button++)
                {
                    // add an entry for the stack for each state of pressing this button n times up until we run out

                    var flips = Buttons[button];
                    var buttons = test.Buttons.ToList();
                    var newState = test.Jolts.ToArray();
                    buttons.Add(button);
                    foreach (var l in flips) { newState[l]++; }
                    while (buttons.Count < shortestCount && !Joltage.Select( (value, index) => newState[index] > value ).Any( b => b ))
                    {
                        Stack.Push( (newState.ToArray(), buttons.ToList()) );
                        buttons.Add(button);
                        foreach (var l in flips) { newState[l]++; }
                    }
                }
            }
            

            return shortestButtons;
        }


        public override string ToString()
        {
            return RenderLights(Lights) + RenderLights(RequiredPattern) + " " + string.Join(' ', Buttons.Select(RenderButton)) + " " + RenderJoltage(); 
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
        TimeCheck("Machine Tests");
        int totalPresses = 0;
        foreach (var m in Machines)
        {
            Console.WriteLine(m);
            var presses = m.GetLeastButtonPressesNeededForJoltage();
            totalPresses += presses.Count;
            TimeCheck(string.Join(",", presses) + " " + presses.Count);
        }
        // foreach (var line in lines)
        // {

        // }
        Console.WriteLine(totalPresses);
    }
}