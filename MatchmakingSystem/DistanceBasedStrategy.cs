using System;

namespace MatchmakingSystem;

/// <summary>
/// 根據候選者與請求者的歐幾里得距離評分。更近的個體獲得更高的分數（平方距離的負數），這使得擴展方法選擇最近的人。此類別沒有“反轉”概念的知識 – 反轉通過與 <see cref="ReverseStrategy"/> 組合處理。
/// </summary>
public class DistanceBasedStrategy : IMatchmakingStrategy
{
    public double Score(Individual self, Individual candidate)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        if (candidate == null) throw new ArgumentNullException(nameof(candidate));

        // 平方距離避免不必要的sqrt；因為只有相對順序重要，我們可以取負數使更近 => 更高分數。
        long dx = self.X - candidate.X;
        long dy = self.Y - candidate.Y;
        double squared = dx * dx + dy * dy;
        return -squared;
    }
}
