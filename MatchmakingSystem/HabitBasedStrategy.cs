using System;
using System.Linq;

namespace MatchmakingSystem;

/// <summary>
/// Scores candidates by the number of shared habits with <paramref name="self"/>.
/// More shared interests produce a higher score.  Tie‑breaking by ID is
/// managed by the extension method.
/// </summary>
public class HabitBasedStrategy : IMatchmakingStrategy
{
    public double Score(Individual self, Individual candidate)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        if (candidate == null) throw new ArgumentNullException(nameof(candidate));

        // intersection size; we don't care about duplicates
        var set = self.Habits.ToHashSet(StringComparer.OrdinalIgnoreCase);
        return candidate.Habits.Count(h => set.Contains(h));
    }
}
