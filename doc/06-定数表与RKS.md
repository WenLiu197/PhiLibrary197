---
title: 定数表与 RKS
icon: zap
order: 50
---

## 定数

Phigros 每张谱面有一个社区标定的**定数（Chart Constant）**。计算 RKS 时使用 `(歌曲完整 id, 难度)` 作为键：

```csharp
using PhiLibrary197.Scores;

ChartConstantKey key = new("Credits.Frums.0", Difficulty.IN);
```

单曲 RKS：

```text
acc < 70  -> 0
否则      -> ((acc - 55) / 45)^2 * 定数
```

## 加载定数表和歌名表

最简单的方式是使用内置数据：

```csharp
using PhiLibrary197.Songs;

SongDatabase db = SongDatabase.LoadFromBuiltIn();

Dictionary<ChartConstantKey, float> constants = db.ToConstantMap();
Dictionary<string, string> names = db.ToNameMap();
```

如果使用自己的 TSV：

```csharp
using PhiLibrary197;

Dictionary<ChartConstantKey, float> constants =
    ScoreHelper.LoadConstantTable("difficulty.tsv");

Dictionary<string, string> names =
    ScoreHelper.LoadSongInfo("info.tsv");
```

`ScoreHelper` 还支持从内存/网络读取：

```csharp
using TextReader reader = new StringReader(tsvText);
var constants = ScoreHelper.LoadConstantTable(reader);

using Stream stream = httpResponse.Content.ReadAsStream();
var names = ScoreHelper.LoadSongInfo(stream);
```

格式：

```text
difficulty.tsv: 曲目id前缀<TAB>EZ<TAB>HD<TAB>IN<TAB>[AT]
info.tsv:       曲目id前缀<TAB>显示名<TAB>曲师<TAB>画师<TAB>谱师...
```

解析规则：

- 空行、`#`/`//` 注释行跳过；
- 定数值无法解析时跳过该列；
- 曲目前缀自动补 `.0`；
- 缺 AT 列时允许省略。

## 从 GameRecord 计算

```csharp
using PhiLibrary197.Scores;

GameRecord record = context.ReadGameRecord();

(List<CompleteScore> phis, List<CompleteScore> sorted, double rks) =
    record.GetSortedListForRks(constants, names);

Console.WriteLine($"RKS: {rks:F4}");
Console.WriteLine($"Phi count: {phis.Count}");
```

`GetSortedListForRks` 的行为：

1. 把所有可计算定数的成绩转成 `CompleteScore`；
2. 同一基础曲目、同一难度只保留 RKS 最高的一条（Random 变体去重）；
3. 按 RKS 降序排序；
4. `Phis` 取排序后状态为 `Phi` 的前 3 条（不足 3 条不补）；
5. `OtherScores` 是完整排序列表（包含 Phi）；
6. RKS 按当前实现计算：

```text
sum(Phis.Rks / 30) + sum(OtherScores.Take(27).Rks / 30)
```

> [!NOTE]
> 想要展示“B30”，通常直接取 `sorted.Take(30)`；`GetSortedListForRksMerged` 则是另一个“前 3 项补 Phi 占位”的展示结构。

## 三种返回结构

### 完整排序（推荐计算 RKS）

```csharp
var (phis, sorted, rks) = record.GetSortedListForRks(constants, names);
```

### 合并显示（前 3 项固定为 Phi）

```csharp
var (scores, rks) = record.GetSortedListForRksMerged(constants, names);
```

合并列表的前 3 项是 Phi；不足 3 个时用 `CompleteScore.Default` 补齐。`CompleteScore.Default` 只用于占位，不能拿它的定数/名字做真实展示。

### 所有可计算成绩

```csharp
IEnumerable<CompleteScore> all = record.GetCompleteScores(constants, names);
```

包含同一曲目的多个 Random 变体，不排序、不去重。

## 完整可运行示例

不依赖云存档，直接用内置数据和合成成绩：

```csharp
using PhiLibrary197.Scores;
using PhiLibrary197.Songs;

SongDatabase db = SongDatabase.LoadFromBuiltIn();

List<SongScore> records = [];
foreach (SongEntry entry in db.Songs.Where(x => x.Constants.ContainsKey(Difficulty.IN)).Take(30))
{
    records.Add(new SongScore(
        score: 1_000_000,
        acc: 100f,
        id: entry.Id,
        difficulty: Difficulty.IN,
        status: ScoreStatus.Phi));
}

GameRecord record = new(records, version: 1);
var (phis, sorted, rks) = record.GetSortedListForRks(db.ToConstantMap(), db.ToNameMap());

Console.WriteLine($"RKS: {rks:F4}");
foreach (CompleteScore score in sorted.Take(10))
    Console.WriteLine($"{score.NameOrDefault} [{score.Score.Difficulty}] {score.Rks:F4}");
```

对应示例项目：

```bash
dotnet run --project samples/PhiLibrary197.Samples -- builtin
```

## 真实云存档

```csharp
using PhiLibrary197.CloudSave;
using PhiLibrary197.Songs;

using Save save = new(token, isInternational: false);
SaveContext context = await save.GetSaveContextAsync(0);
GameRecord record = context.ReadGameRecord();

SongDatabase db = SongDatabase.LoadFromBuiltIn();
var (phis, sorted, rks) = record.GetSortedListForRks(db.ToConstantMap(), db.ToNameMap());
```

完整示例：[CloudSaveExample.cs](https://github.com/WenLiu197/PhiLibrary197/blob/master/samples/PhiLibrary197.Samples/CloudSaveExample.cs)

## 定数缺失

`GetCompleteScores` / `GetSortedListForRks` / `GetSortedListForRksMerged` 会跳过定数表查不到的成绩，不抛异常。常见原因：

- Legacy/SP 难度没有定数；
- 新曲还没更新内置数据；
- 自定义表缺少该曲目/难度。

如果绕过这些方法直接访问 `CompleteScore.ChartConstant` 或 `Rks`，缺失时会抛 `KeyNotFoundException`。

## 常见问题

### 为什么要用内置 SongDatabase？

`SongDatabase.LoadFromBuiltIn()` 同时提供 327 首曲目的定数和歌名，避免自己维护两份表。需要自定义数据时，仍可用 `LoadFromFile` 或 `ScoreHelper`。

### 为什么 RKS 和游戏里显示不一样？

`Summary.Rks` 是游戏内显示值，可能滞后；应以 `GetSortedListForRks` 计算值为准。

### Random 变体怎么算？

`GetSortedListForRks` 会按基础曲目 + 难度去重，保留 RKS 最高的一条；`GetCompleteScores` 会保留所有变体。展示全部成绩用后者，算 B30/RKS 用前者。

### 怎么导出成绩？

库只提供 `ExportScore` 模型和 `CompleteScore` 数据，不提供 TSV/CSV/Excel 输出。请按自己的列格式映射后输出；变体和排序策略由调用方决定。
