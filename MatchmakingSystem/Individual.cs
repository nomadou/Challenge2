using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchmakingSystem;

public class Individual
{
    // all fields are backed by read‑only auto properties; they are public so
    // strategy implementations can easily inspect them.
    public int Id { get; }
    public string Gender { get; }
    public int Age { get; }
    public string Intro { get; }
    public IReadOnlyList<string> Habits { get; }
    public int X { get; }
    public int Y { get; }

    public Individual(int id,
                      string gender,
                      int age,
                      string intro,
                      IEnumerable<string> habits,
                      int x,
                      int y)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
        Id = id;
        Gender = gender ?? throw new ArgumentNullException(nameof(gender));
        Age = age;
        Intro = intro ?? string.Empty;
        Habits = habits?.Select(h => h.Trim()).Where(h => h.Length > 0).ToList() ??
                 new List<string>();
        X = x;
        Y = y;
    }

    /// <summary>
    /// Convenience factory that parses a line from the provided CSV file.
    /// The habits column may be separated by either commas or semicolons; the
    /// parser normalizes them to a list of strings.
    /// </summary>
    public static Individual FromCsv(string csvLine)
    {
        if (csvLine == null) throw new ArgumentNullException(nameof(csvLine));
        var parts = csvLine.Split(',');
        if (parts.Length < 7)
            throw new FormatException("CSV line does not have enough fields");

        int id = int.Parse(parts[0]);
        string gender = parts[1];
        int age = int.Parse(parts[2]);
        string intro = parts[3];
        var rawHabits = parts[4];
        var habits = rawHabits.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(s => s.Trim())
                              .ToList();
        int x = int.Parse(parts[5]);
        int y = int.Parse(parts[6]);
        return new Individual(id, gender, age, intro, habits, x, y);
    }

    public override string ToString()
    {
        return $"#{Id} ({Gender}, age {Age}) at ({X},{Y})";
    }
}
