---
title: SongDatabase
icon: database
---

曲目数据库：合并 difficulty.tsv（定数）与 info.tsv（歌名/曲师/画师/谱师）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public sealed class SongDatabase
```

## 静态方法

### Load

```csharp
public static SongDatabase Load(string difficultyTsvPath, string infoTsvPath)
```

加载并合并两张表。以曲目 id 前缀为键做**并集**：任一表存在的曲目都收录，缺失字段为 `null` / 空表。容错规则与 `ScoreHelper.LoadConstantTable` 一致。

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Songs` | `IReadOnlyList<SongEntry>` | 全部曲目（按完整 id 排序） |
| `Count` | `int` | 曲目数量 |

## 方法

### GetSong

```csharp
public SongEntry? GetSong(string id)
```

按完整 id 查询，也接受不带 `.0` 后缀的前缀形式。不存在返回 `null`。

### FindByName

```csharp
public IEnumerable<SongEntry> FindByName(string name)
```

按显示名搜索（不区分大小写），返回所有匹配项。

### ToConstantMap

```csharp
public Dictionary<ChartConstantKey, float> ToConstantMap()
```

导出定数表，供 `GameRecord.GetSortedListForRks` 使用。

### ToNameMap

```csharp
public Dictionary<string, string> ToNameMap()
```

导出歌名表（显示名为 `null` 的曲目不纳入，RKS 输出回退显示 id）。

## 示例

```csharp
SongDatabase db = SongDatabase.Load("difficulty.tsv", "info.tsv");

// 查询
SongEntry stasis = db.GetSong("Stasis.Maozon")!;
Console.WriteLine($"{stasis.Name} | {stasis.Composer} | IN定数 {stasis.GetConstant(Difficulty.IN)}");

// RKS
var (phis, other, rks) = record.GetSortedListForRks(db.ToConstantMap(), db.ToNameMap());
```
