using System;
using System.Collections.Generic;
using System.IO;

namespace MatchmakingSystem;

/// <summary>
/// 面向客戶端的類別。它持有對 <see cref="IMatchmakingStrategy"/> 的引用，並將實際的夥伴選擇委派給該策略。由於策略是注入的，調用者可以在運行時交換它或提供反轉版本，而無需修改此類別。
/// </summary>
public class MatchmakingSystem
{
    private readonly List<Individual> _candidates;
    private IMatchmakingStrategy? _strategy;

    public MatchmakingSystem(string csvPath)
    {
        _candidates = LoadIndividualsFromCsv(csvPath);
        if (_candidates.Count == 0)
        {
            Console.WriteLine("no data");
        }
    }

    public IMatchmakingStrategy? Strategy
    {
        get => _strategy;
    }

    /// <summary>
    /// 取得候選人列表。
    /// </summary>
    public IReadOnlyList<Individual> Candidates => _candidates.AsReadOnly();

    /// <summary>
    /// 檢查是否沒有候選人。
    /// </summary>
    public bool IsEmpty => _candidates.Count == 0;

    /// <summary>
    /// 設定匹配策略。
    /// </summary>
    public void SetStrategy(IMatchmakingStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    /// <summary>
    /// 將單個個體與系統中的候選人池匹配。
    /// </summary>
    public Individual? Match(Individual someone)
        => _strategy?.Match(someone, _candidates);

    /// <summary>
    /// 便利助手，計算將每個人都映射到其選擇的夥伴的字典。為了簡單起見，返回的映射使用ID。
    /// </summary>
    public IDictionary<int, int> MatchAll()
    {
        var result = new Dictionary<int, int>();
        foreach (var p in _candidates)
        {
            var mate = Match(p);
            if (mate != null)
                result[p.Id] = mate.Id;
        }
        return result;
    }

    /// <summary>
    /// 從指定的CSV檔案載入個體列表。
    /// </summary>
    public static List<Individual> LoadIndividualsFromCsv(string filePath)
    {
        var list = new List<Individual>();
        if (!File.Exists(filePath))
        {
            return list;
        }

        using var reader = new StreamReader(filePath);
        string? header = reader.ReadLine(); // ignore header
        while (true)
        {
            var line = reader.ReadLine();
            if (line == null) break;
            try
            {
                list.Add(Individual.FromCsv(line));
            }
            catch (Exception)
            {
                // 忽略解析錯誤的行
            }
        }
        return list;
    }

    /// <summary>
    /// 根據ID查找候選人。
    /// </summary>
    public Individual? FindById(int id)
    {
        return _candidates.FirstOrDefault(p => p.Id == id);
    }
}
