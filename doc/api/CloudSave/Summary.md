---
title: Summary
icon: book
---

玩家的游玩统计摘要（含挑战码与各难度通关计数）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public class Summary : IPhigrosCustomSerialization<Summary>
```

## 静态属性

```csharp
public static Summary Default { get; }
```

## 构造函数

```csharp
public Summary(
    byte saveVersion, Challenge challenge, float rks, int gameVersion,
    string avatar, PlayCountSummary ez, PlayCountSummary hd,
    PlayCountSummary @in, PlayCountSummary at)
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `SaveVersion` | `byte` | 存档版本 |
| `GameVersion` | `int` | 游戏版本 |
| `Rks` | `float` | 游戏内显示的 RKS（可能不准，建议用 `GameRecord` 自行计算） |
| `Challenge` | `Challenge` | 挑战码 |
| `Avatar` | `string` | 头像 id（如 `Introduction`、`-SURREALISM-`） |
| `EZPlayRecord` | `PlayCountSummary` | EZ 难度通关统计 |
| `HDPlayRecord` | `PlayCountSummary` | HD 难度通关统计 |
| `INPlayRecord` | `PlayCountSummary` | IN 难度通关统计 |
| `ATPlayRecord` | `PlayCountSummary` | AT 难度通关统计 |

## 示例

```csharp
Summary summary = ctx.ReadSummary();
Console.WriteLine($"挑战: {summary.Challenge.Rank} Lv.{summary.Challenge.Level}");
Console.WriteLine($"EZ 通关 {summary.EZPlayRecord.ClearedCount} 首, Phi {summary.EZPlayRecord.PhiCount} 首");
```
