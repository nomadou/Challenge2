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
        var people = MatchmakingSystem.LoadIndividualsFromCsv(DataFile);
        if (people.Count == 0)
        {
            Console.WriteLine("no data");
            return;
        }

        // 使用者預期是CSV中的一個條目。提示輸入ID，並僅為該特定個體計算匹配。
        Console.Write("Enter ID to match: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("invalid ID");
            return;
        }

        var user = people.Find(p => p.Id == id);
        if (user == null)
        {
            Console.WriteLine($"no individual with ID {id}");
            return;
        }

        var sys = new MatchmakingSystem(new DistanceBasedStrategy());
        Console.WriteLine("Distance‑based (closest):");
        DumpMatch(sys, user, people);

        sys.Strategy = new ReverseStrategy(new DistanceBasedStrategy());
        Console.WriteLine("Distance‑based (farthest via reverse):");
        DumpMatch(sys, user, people);

        sys.Strategy = new HabitBasedStrategy();
        Console.WriteLine("Habit‑based (most shared interests):");
        DumpMatch(sys, user, people);

        sys.Strategy = new ReverseStrategy(new HabitBasedStrategy());
        Console.WriteLine("Habit‑based (least shared via reverse):");
        DumpMatch(sys, user, people);

    }

    private static void DumpMatch(MatchmakingSystem sys, Individual user, List<Individual> people)
    {
        var mate = sys.Match(user, people);
        Console.WriteLine($"{user} -> {mate}");
        Console.WriteLine();
    }
}
