成绩容器，提供 RKS 计算与排序。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public class GameRecord : IPhigrosCustomSerialization<GameRecord>
```

## 构造函数

```csharp
public GameRecord(List<SongScore> records, byte version)
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Version` | `byte` | 成绩文件版本，最新 1 |
| `Records` | `List<SongScore>` | 全部单曲成绩 |

## 方法

### GetCompleteScores

```csharp
public IEnumerable<CompleteScore> GetCompleteScores(
    IReadOnlyDictionary<ChartConstantKey, float> constantMap,
    IReadOnlyDictionary<string, string> nameMap)
```

将全部成绩转为 [CompleteScore](CompleteScore.md)。**自动跳过定数表查不到的成绩**（不抛异常）。

### GetSortedListForRks

```csharp
public (List<CompleteScore> Phis, List<CompleteScore> OtherScores, double Rks) GetSortedListForRks(
    IReadOnlyDictionary<ChartConstantKey, float> constantMap,
    IReadOnlyDictionary<string, string> nameMap)
```

按 RKS 降序排序并计算总 RKS。

| 返回 | 说明 |
| --- | --- |
| `Phis` | 状态为 `Phi` 的前 3 个成绩（不足 3 个不填充） |
| `OtherScores` | 完整排序列表 |
| `Rks` | 总 RKS = Σ(Phi.Rks)/30 + Σ(前27条.Rks)/30 |

> [!NOTE]
> 适用于游戏版本 > 3.11.0 的评分体系。

### GetSortedListForRksMerged

```csharp
public (List<CompleteScore> Scores, double Rks) GetSortedListForRksMerged(
    IReadOnlyDictionary<ChartConstantKey, float> constantMap,
    IReadOnlyDictionary<string, string> nameMap)
```

排序后合并 Phi 到列表头部（前 0~3 个为 Phi，不足用 `CompleteScore.Default` 填充）。

### FromReader / Serialize

```csharp
public static GameRecord FromReader(BinaryReader reader, byte objectVersion)
public void Serialize(BinaryWriter writer, out byte objectVersion)
```

二进制序列化（`IPhigrosCustomSerialization` 实现，一般通过 `SaveContext` 使用）。

## 示例

```csharp
GameRecord record = ctx.ReadGameRecord();

var constantMap = ScoreHelper.LoadConstantTable("difficulty.tsv");
var nameMap     = ScoreHelper.LoadSongInfo("info.tsv");

var (phis, other, rks) = record.GetSortedListForRks(constantMap, nameMap);
Console.WriteLine($"RKS: {rks:F4}");

foreach (var s in other.Take(10))
    Console.WriteLine($"{s.NameOrDefault} {s.Score.Difficulty}: {s.Rks:F4}");
```
