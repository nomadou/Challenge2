using System;
using System.Collections.Generic;

namespace MatchmakingSystem;

// 策略模式：封裝評分邏輯，以便系統可以使用任何演算法配置。每個具體實現為候選者返回一個數值分數；越高意味著“更好”。一個小的助手（擴展方法）執行常見的遍歷和決勝負。

public interface IMatchmakingStrategy
{
    /// <summary>
    /// 當從 <paramref name="self"/> 匹配時，為 <paramref name="candidate"/> 返回分數。配對助手將較大的值視為更理想，並通過較小的ID打破平局。
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
