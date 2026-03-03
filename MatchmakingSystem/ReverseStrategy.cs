using System;

namespace MatchmakingSystem;

/// <summary>
/// Decorator that inverts the desirability of another strategy.  Because each
/// concrete strategy simply produces a numeric score, negating that score
/// makes the best‑candidate become the worst and vice‑versa.  There is no
/// boolean flag—clients explicitly compose a reverse strategy when they need
/// it, e.g. <c>new ReverseStrategy(new DistanceBasedStrategy())</c>.
/// </summary>
public class ReverseStrategy : IMatchmakingStrategy
{
    private readonly IMatchmakingStrategy _inner;

    public ReverseStrategy(IMatchmakingStrategy inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    public double Score(Individual self, Individual candidate)
    {
        // simply flip the sign of whatever the wrapped strategy would return
        // (zero stays zero, equal scores remain equal).
        return -_inner.Score(self, candidate);
    }
}
