using NUnit.Framework;
namespace AoC2025;

class Day1 : Puzzle
{

    int Dial = 50;

    public int R(int Amount)
    {
        Dial = (Dial + Amount);
        int zeros = 0;
        while (Dial > 99)
        {
            zeros++;
            Dial -= 100;
        }
        return zeros;
    }

    public int L(int Amount)
    {
        bool startZero = Dial == 0;
        Dial = (Dial - Amount);
        int zeros = startZero ? -1 : 0;
        while (Dial < 0)
        {
            zeros++;
            Dial += 100;
        }
        if (Dial == 0) zeros++;
        return zeros;
    }

    public override void Part1()
    {
        int Password = 0;

        for (int x=0;x<lines.Count();x++)
        {
            var parts = lines[x];
            if (parts.Length > 1 && int.TryParse(parts[1..], out var amount))
            {
                if (parts[0] == 'R') R(amount); else L(amount);
                Console.WriteLine($"{lines[x]} -> {Dial}");
                if (Dial == 0) Password ++;
            }
        }
        Console.WriteLine(Password);
    }

    public override void Part2()
    {
        int Password = 0;
        Dial = 50;
        for (int x=0;x<lines.Count();x++)
        {
            var parts = lines[x];
            if (parts.Length > 1 && int.TryParse(parts[1..], out var amount))
            {
                Password+=parts[0] == 'R' ? R(amount) : L(amount);
                Console.WriteLine($"{lines[x]} -> {Dial} -> {Password}");
            }
        }
        Console.WriteLine(Password);
   }
}