using System;
using System.Collections.Generic;

namespace MatchmakingSystem;

// strategy pattern: encapsulate the scoring logic so that the system can be
// configured with any algorithm.  each concrete implementation returns a
// numeric score for a candidate; higher means "better".  a small helper
// (extension method) performs the common traversal and tie-breaking.

public interface IMatchmakingStrategy
{
    /// <summary>
    /// Returns a score for <paramref name="candidate"/> when matching from
    /// <paramref name="self"/>.  The matchmaking helper treats larger values
    /// as more desirable, breaking ties by smaller ID.
    /// </summary>
    double Score(Individual self, Individual candidate);
}

public static class MatchmakingStrategyExtensions
{
    public static Individual? Match(this IMatchmakingStrategy strategy,
                                    Individual self,
                                    IEnumerable<Individual> others)
    {
        if (strategy == null) throw new ArgumentNullException(nameof(strategy));
        if (others == null) throw new ArgumentNullException(nameof(others));

        Individual? best = null;
        double bestScore = double.MinValue;
        foreach (var other in others)
        {
            if (other == self)
                continue;

            double score = strategy.Score(self, other);
            if (best == null || score > bestScore ||
                (score == bestScore && other.Id < best.Id))
            {
                best = other;
                bestScore = score;
            }
        }

        return best;
    }
}
