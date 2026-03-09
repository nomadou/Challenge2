using System;

namespace MatchmakingSystem;

/// <summary>
/// 裝飾器，用於反轉另一個策略的理想性。由於每個具體策略僅產生數值分數，取負數使最佳候選者成為最差，反之亦然。沒有布林旗標 – 客戶端在需要時明確組合反轉策略，例如 <c>new ReverseStrategy(new DistanceBasedStrategy())</c>。
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
        // 簡單地翻轉包裝策略將返回的符號（零保持零，相等分數保持相等）。
        return -_inner.Score(self, candidate);
    }
}
