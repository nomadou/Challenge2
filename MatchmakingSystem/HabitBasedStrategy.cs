using System;
using System.Linq;

namespace MatchmakingSystem;

/// <summary>
/// 根據與 <paramref name="self"/> 共享習慣的數量評分候選者。更多共享興趣產生更高的分數。由ID進行的決勝負由擴展方法管理。
/// </summary>
public class HabitBasedStrategy : IMatchmakingStrategy
{
    public double Score(Individual self, Individual candidate)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        if (candidate == null) throw new ArgumentNullException(nameof(candidate));

        // 交集大小；我們不關心重複
        var set = self.Habits.ToHashSet(StringComparer.OrdinalIgnoreCase);
        return candidate.Habits.Count(h => set.Contains(h));
    }
}
