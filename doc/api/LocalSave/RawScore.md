---
title: RawScore
icon: book
---

本地存档的原始成绩（JSON 格式）。

## 定义

```csharp
namespace PhiLibrary197.LocalSave;

public struct RawScore
```

## 属性（JSON 键名见括号）

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Score` | `int`（`s`） | 分数（0 ~ 1,000,000） |
| `Accuracy` | `float`（`a`） | 准确率（0 ~ 100） |
| `Status` | `ScoreStatus`（`c`） | 原始状态（0 = 非 FC，1 = FC…） |

## 方法

### FromJson

```csharp
public static RawScore FromJson(string json)
```

从 JSON 字符串解析（走源生成反序列化，AOT 友好）。格式如 `{"s":996105,"a":99.56718444824219,"c":1}`。

### ToSongScore

```csharp
public SongScore ToSongScore(string songId, Difficulty difficulty)
```

转换为 [SongScore](../CloudSave/SongScore.md)（自动判定成绩状态）。

## 示例

```csharp
RawScore raw = RawScore.FromJson("""{"s":996105,"a":99.56,"c":1}""");
SongScore song = raw.ToSongScore("MARENOL.LeaF.0", Difficulty.HD);
```
