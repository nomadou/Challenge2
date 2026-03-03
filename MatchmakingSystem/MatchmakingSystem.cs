using System;
using System.Collections.Generic;

namespace MatchmakingSystem;

/// <summary>
/// Client‑visible class.  It holds a reference to an <see cref="IMatchmakingStrategy"/>
/// and delegates the actual choice of partner to that strategy.  Because the
/// strategy is injected, callers can swap it at runtime or supply a reversed
/// version without modifying this class.
/// </summary>
public class MatchmakingSystem
{
    private IMatchmakingStrategy _strategy;

    public MatchmakingSystem(IMatchmakingStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public IMatchmakingStrategy Strategy
    {
        get => _strategy;
        set => _strategy = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Match a single individual against the supplied pool (which may include
    /// the individual; the implementation ignores self during evaluation).
    /// </summary>
    public Individual? Match(Individual someone, IEnumerable<Individual> pool)
        => _strategy.Match(someone, pool);

    /// <summary>
    /// Convenience helper that computes a dictionary mapping each person to their
    /// chosen partner.  For simplicity the returned map uses IDs.
    /// </summary>
    public IDictionary<int, int> MatchAll(IEnumerable<Individual> people)
    {
        if (people == null) throw new ArgumentNullException(nameof(people));

        var list = new List<Individual>(people);
        var result = new Dictionary<int, int>();
        foreach (var p in list)
        {
            var mate = Match(p, list);
            if (mate != null)
                result[p.Id] = mate.Id;
        }
        return result;
    }
}
