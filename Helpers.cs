namespace AoC2025;

using System.Diagnostics;
using System.IO;

public abstract class Puzzle {
    public string? extra = null;
    public string[] lines = new string[0];

    private string[]? _words;

    public string[] words => _words ??= lines.SelectMany(s => s.Split(' ')).ToArray();

    public virtual void Parse(string filename)
    {
        lines = File.ReadAllLines(filename);
        if (lines.Length == 0)
        {
            Console.WriteLine($"\u001b[1;31m:Empty Input: {filename}");
        }
    }

    public virtual void Part1() => Console.WriteLine("Not implemented yet");
    public virtual void Part2() => Console.WriteLine("Not implemented yet");

    private static Stopwatch stopwatch = new();
    public static void TimeCheck(string str)
    {
        if (!stopwatch.IsRunning) stopwatch.Start();
        Console.WriteLine($"[{stopwatch.Elapsed}] {str}");
    }
}