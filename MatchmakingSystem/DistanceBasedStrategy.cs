using System;

namespace MatchmakingSystem;

/// <summary>
/// Scores candidates according to their Euclidean distance from the requester.
/// Closer individuals receive a higher score (negative of squared distance),
/// which makes the extension method pick the nearest person.  This class has
/// no knowledge of "reverse" concept – reversal is handled by composing with
/// <see cref="ReverseStrategy"/>.
/// </summary>
public class DistanceBasedStrategy : IMatchmakingStrategy
{
    public double Score(Individual self, Individual candidate)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        if (candidate == null) throw new ArgumentNullException(nameof(candidate));

        // squared distance avoids unnecessary sqrt; since only relative order
        // matters we can negate to make closer => larger score.
        long dx = self.X - candidate.X;
        long dy = self.Y - candidate.Y;
        double squared = dx * dx + dy * dy;
        return -squared;
    }
}
