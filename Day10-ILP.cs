using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Google.OrTools.LinearSolver;
namespace AoC2025;

class Day110 : Day10
{
    // public override void Parse(string filename)
    // {
    //     base.Parse(filename);
    // }

    // public override void Part1()
    // {
        // foreach (var line in lines)
        // {

        // }
        // Console.WriteLine();
    // }

    public int SolveMachine(Day10.Machine machine)
    {
        Solver solver = Solver.CreateSolver("SCIP");
        
        var buttonPresses = machine.Buttons.Select((button, index) => {
            // we can only press this button a number of times equal to the smallest joltage requirement we affect
            var bmax = button.Select(flips => machine.Joltage[flips]).Min();
            return solver.MakeIntVar(0.0, bmax, $"b{index}");
        }).ToList();

        var zero = solver.MakeIntVar(0, 0, "zero");

        for (int j = 0; j<machine.Joltage.Count; j++)
        {
            var buttons = buttonPresses.Where((v, index) => machine.Buttons[index].Contains(j)).ToList();
            solver.Add(buttons.Aggregate((LinearExpr) zero, (m, b) => m + b) == machine.Joltage[j]);
        }

        solver.Minimize(buttonPresses.Aggregate((LinearExpr) zero, (m, b) => m + b));
        Solver.ResultStatus resultStatus = solver.Solve();

        Console.WriteLine("Problem solved in " + solver.Iterations() + " iterations");
        Console.WriteLine("Problem solved in " + solver.Nodes() + " branch-and-bound nodes");
        foreach (var b in buttonPresses)
        {
            Console.Write($"{b.Name()} = {(int)b.SolutionValue()}; ");
        }
        Console.WriteLine();
        TimeCheck(machine.ToString() + ": " + solver.Objective().Value());

        return (int)solver.Objective().Value();
    }

    public override void Part2()
    {
        Console.WriteLine(lines.Select(l => new Day10.Machine(l)).Select(SolveMachine).Sum());
        Console.WriteLine(lines.Select(l => new Day10.Machine(l)).Select(m => m.Buttons.Count()).Max());
    }
}