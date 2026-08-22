## 概念

Phigros 每张谱面（歌曲 id + 难度）有一个社区标定的**定数（Chart Constant）**，如 `Credits.Frums.0` 的 IN 为 `14.0`。

RKS（Rating Score）计算公式：

```
单曲 RKS = (acc < 70) ? 0 : ((acc - 55) / 45)² × 定数
总  RKS  = Σ(前3个Phi单曲RKS)/30 + Σ(前27个最高单曲RKS)/30
```

## ChartConstantKey

```csharp
public record struct ChartConstantKey(string SongId, Difficulty Difficulty);
```

- `SongId`：含数字后缀的完整 id，如 `"Stasis.Maozon.0"`
- `Difficulty`：难度枚举

## 加载定数表

```csharp
using PhiLibrary197;

// 一行加载：difficulty.tsv（曲目名\tEZ\tHD\tIN\tAT，列按存在情况可选）
Dictionary<ChartConstantKey, float> constantMap = ScoreHelper.LoadConstantTable(@"difficulty.tsv");
```

内置容错：跳过注释/空行/脏行；数值解析失败跳过该列；曲名自动补 `.0` 后缀。

加载歌名表（info.tsv）：

```csharp
// info.tsv（曲目名\t显示名\t曲师\t画师\t谱师...）
Dictionary<string, string> nameMap = ScoreHelper.LoadSongInfo(@"info.tsv");
```

> [!TIP]
> 两个表都建议用 [SongDatabase](07-SongDatabase.md) 统一加载——`db.ToConstantMap()` / `db.ToNameMap()` 直接产出上面两个字典，还能查曲目百科。

## 计算 RKS

```csharp
Save save = new(token, false);
SaveContext ctx = await save.GetSaveContextAsync(0);
GameRecord record = ctx.ReadGameRecord();

// 方式一：完整排序（推荐）
var (phis, otherScores, rks) = record.GetSortedListForRks(constantMap, nameMap);
Console.WriteLine($"RKS: {rks:F4}");
foreach (var phi in phis) Console.WriteLine($"[Phi] {phi.NameOrDefault} {phi.Score.Difficulty}: {phi.Rks:F4}");

// 方式二：合并排序（前 3 个是 Phi，不足补 Default 占位）
var (scores, rks2) = record.GetSortedListForRksMerged(constantMap, nameMap);

// 方式三：只取完整成绩（不排序）
IEnumerable<CompleteScore> all = record.GetCompleteScores(constantMap, nameMap);
```

## 排序语义

`GetSortedListForRks` 的行为：

1. 全部成绩转 `CompleteScore` 并按 RKS 降序
2. `Phis` = 状态为 `ScoreStatus.Phi` 的前 3 个（不足 3 个**不填充**）
3. `Rks = Σ(Phi.Rks)/30 + Σ(前27条.Rks)/30`

`GetSortedListForRksMerged`：Phis 不足 3 个时用 `CompleteScore.Default` 补齐并插入列表头部。

## 缺定数成绩的处理

> [!IMPORTANT]
> `GetCompleteScores` / `GetSortedListForRks` / `GetSortedListForRksMerged` 会**自动跳过**定数表查不到的成绩（Legacy/SP 难度、表未收录的新曲），不会抛异常，与游戏内 RKS 计算行为一致。
> 若绕过上述方法直接访问 `CompleteScore.ChartConstant` / `.Rks`，缺定数时仍会抛 `KeyNotFoundException`。

## 导出 Excel 友好格式

```csharp
var export = record.GetCompleteScores(constantMap, nameMap)
    .Select(s => new ExportScore
    {
        ID = s.Score.Id,
        Name = s.NameOrDefault,
        Difficulty = s.Score.Difficulty.ToString(),
        ChartConstant = s.ChartConstant,
        Score = s.Score.Score,
        Acc = s.Score.Accuracy,
        RksGiven = s.Rks,
        Status = s.Score.Status.ToString(),
    });
```

## 常见坑

1. **定数表必须真实**：`CompleteScore` 用定数排序，不要用 mock/假定数表，否则排序与 RKS 全错
2. **表未更新的新曲**：新曲目成绩会被自动跳过（RKS 不受影响），但成绩列表里看不到它——及时更新定数表
3. **游戏内 RKS vs 计算 RKS**：`Summary.Rks` 是游戏内显示值（可能滞后），应以计算结果为准
4. **难度枚举**：EZ=0 / HD=1 / IN=2 / AT=3
