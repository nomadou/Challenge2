using System;
using System.Collections.Generic;
using System.IO;

namespace MatchmakingSystem;

// simple demo: read the built‑in data.csv and run a few strategies so that the
// behaviour can be inspected when executing the program.

static class Program
{
    private const string DataFile = "data.csv";

    static void Main()
    {
        var people = LoadData();
        if (people.Count == 0)
        {
            Console.WriteLine("no data");
            return;
        }

        // the user is expected to be one of the entries in the CSV.  prompt for
        // an ID and only compute a match for that specific individual.
        Console.Write("Enter ID to match: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("invalid ID");
            return;
        }

        var user = people.Find(p => p.Id == id);
        if (user == null)
        {
            Console.WriteLine($"no individual with ID {id}");
            return;
        }

        var sys = new MatchmakingSystem(new DistanceBasedStrategy());
        Console.WriteLine("Distance‑based (closest):");
        DumpMatch(sys, user, people);

        sys.Strategy = new ReverseStrategy(new DistanceBasedStrategy());
        Console.WriteLine("Distance‑based (farthest via reverse):");
        DumpMatch(sys, user, people);

        sys.Strategy = new HabitBasedStrategy();
        Console.WriteLine("Habit‑based (most shared interests):");
        DumpMatch(sys, user, people);

        sys.Strategy = new ReverseStrategy(new HabitBasedStrategy());
        Console.WriteLine("Habit‑based (least shared via reverse):");
        DumpMatch(sys, user, people);

    }

    private static List<Individual> LoadData()
    {
        var list = new List<Individual>();
        if (!File.Exists(DataFile)) return list;

        using var reader = new StreamReader(DataFile);
        string? header = reader.ReadLine(); // ignore header
        while (true)
        {
            var line = reader.ReadLine();
            if (line == null) break;
            try
            {
                list.Add(Individual.FromCsv(line));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"failed to parse line: {ex.Message}");
            }
        }
        return list;
    }

    private static void DumpMatches(MatchmakingSystem sys, List<Individual> people, int take)
    {
        foreach (var p in people.Take(take))
        {
            var mate = sys.Match(p, people);
            Console.WriteLine($"{p} -> {mate}");
        }
        Console.WriteLine();
    }

    private static void DumpMatch(MatchmakingSystem sys, Individual user, List<Individual> people)
    {
        var mate = sys.Match(user, people);
        Console.WriteLine($"{user} -> {mate}");
        Console.WriteLine();
    }
}
