using System;
using System.Collections.Generic;
using System.IO;

namespace MatchmakingSystem;

// 簡單演示：讀取內建的data.csv並運行幾個策略，以便在執行程式時檢查行為。

static class Program
{
    // 資料檔案預期與可執行檔案並存。我們計算絕對路徑，以便當前工作目錄無關緊要 – 這可以防止使用者從不同資料夾運行程式時的混亂（例如，雙擊exe）。
    private static readonly string DataFile =
        Path.Combine(AppContext.BaseDirectory, "data.csv");

    static void Main()
    {
        var sys = new MatchmakingSystem(DataFile);
        if (sys.IsEmpty)
        {
            return;
        }

        // 使用者預期是CSV中的一個條目。提示輸入ID，並僅為該特定個體計算匹配。
        Console.Write("Enter ID to match: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("invalid ID");
            return;
        }

        var user = sys.FindById(id);
        if (user == null)
        {
            Console.WriteLine($"no individual with ID {id}");
            return;
        }

        sys.SetStrategy(new DistanceBasedStrategy());
        Console.WriteLine("Distance-based (closest):");
        var mate1 = sys.Match(user);
        Console.WriteLine($"{user} -> {mate1}");
        Console.WriteLine();

        sys.SetStrategy(new ReverseStrategy(new DistanceBasedStrategy()));
        Console.WriteLine("Distance-based & Reverse (farthest):");
        var mate2 = sys.Match(user);
        Console.WriteLine($"{user} -> {mate2}");
        Console.WriteLine();

        sys.SetStrategy(new HabitBasedStrategy());
        Console.WriteLine("Habit-based (most shared interests)");
        var mate3 = sys.Match(user);
        Console.WriteLine($"{user} -> {mate3}");
        Console.WriteLine($"[{string.Join(", ", user.Habits)}] -> [{string.Join(", ", mate3.Habits)}]");
        Console.WriteLine();

        sys.SetStrategy(new ReverseStrategy(new HabitBasedStrategy()));
        Console.WriteLine("Habit-based & Reverse (least shared via reverse)");
        var mate4 = sys.Match(user);
        Console.WriteLine($"{user} -> {mate4}");
        Console.WriteLine($"[{string.Join(", ", user.Habits)}] -> [{string.Join(", ", mate4.Habits)}]");
        Console.WriteLine();

    }
}
