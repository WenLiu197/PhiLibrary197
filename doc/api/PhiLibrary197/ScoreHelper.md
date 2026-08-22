---
title: ScoreHelper
icon: zap
---

成绩状态判定与数据表加载的静态工具类。

## 定义

```csharp
namespace PhiLibrary197;

public static class ScoreHelper
```

## 方法

### ParseStatus(RawScore)

```csharp
public static ScoreStatus ParseStatus(RawScore record)
```

根据 `RawScore` 的分数、准确率与 FC 标记计算成绩状态。

### ParseStatus(int, double, bool)

```csharp
public static ScoreStatus ParseStatus(int score, double accuracy, bool isFc)
```

根据原始值计算成绩状态。

| 参数 | 说明 |
| --- | --- |
| `score` | 分数，如 920000、1000000 |
| `accuracy` | 准确率，如 99.114514、100 |
| `isFc` | 是否 Full Combo |

**判定规则**（依次）：

| 条件 | 结果 |
| --- | --- |
| `accuracy == 100 && score == 1000000` | `ScoreStatus.Phi` |
| `accuracy == 100` | `ScoreStatus.Bugged` |
| `isFc` | `ScoreStatus.Fc` |
| `score >= 960000` | `ScoreStatus.Vu` |
| `score >= 920000` | `ScoreStatus.S` |
| `score >= 880000` | `ScoreStatus.A` |
| `score >= 820000` | `ScoreStatus.B` |
| `score >= 700000` | `ScoreStatus.C` |
| `score >= 0` | `ScoreStatus.False` |
| 其他 | `ScoreStatus.Bugged` |

### DifficultyStringToIndex

```csharp
public static byte DifficultyStringToIndex(string diff)
```

将难度字符串转换为难度索引（`"EZ"` → 0，`"HD"` → 1，`"IN"` → 2，`"AT"` → 3），不区分大小写。

### LoadConstantTable

```csharp
public static Dictionary<ChartConstantKey, float> LoadConstantTable(string tsvPath)
```

从 difficulty.tsv 加载定数表。格式：`曲目名\tEZ\tHD\tIN\tAT`（列按存在情况可选）。容错：跳过注释/空行/脏行，数值解析失败跳过该列，曲名自动补 `.0` 后缀。

### LoadSongInfo

```csharp
public static Dictionary<string, string> LoadSongInfo(string tsvPath)
```

从 info.tsv 加载歌名表（完整曲目 id → 显示名）。info.tsv 第二列为显示名，其余列（曲师/画师/谱师）忽略。容错规则同 `LoadConstantTable`。

> [!TIP]
> 两张表建议统一用 `SongDatabase.Load` 加载，详见 [SongDatabase](../CloudSave/SongDatabase.md)。
