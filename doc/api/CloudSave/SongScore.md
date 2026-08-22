---
title: SongScore
icon: book
---

单曲成绩，版本无关的基础成绩数据（不含定数与 RKS）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public class SongScore : IEquatable<SongScore>, IEqualityOperators<SongScore, SongScore, bool>
```

## 静态属性

```csharp
public static SongScore Default { get; }   // 空成绩（0/0/""/EZ/False），每次返回新实例
```

## 构造函数

```csharp
public SongScore(int score, float acc, string id, Difficulty difficulty, ScoreStatus status)
public SongScore(int score, float acc, string id, bool isFc, Difficulty difficulty)
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Score` | `int` | 分数（0 ~ 1,000,000） |
| `Accuracy` | `float` | 准确率（0 ~ 100） |
| `Id` | `string` | 曲目 id，如 `Stasis.Maozon.0` |
| `Difficulty` | `Difficulty` | 难度 |
| `Status` | `ScoreStatus` | 成绩状态（setter 会同步内部 FC 标记） |

## 方法

`ToString()` 输出 JSON 风格多行描述；支持 `Equals` / `==` / `!=`（按分数/准确率/id/难度/FC 标记比较）。

## 备注

- 构造时若传 `ScoreStatus.Fc` 或 `ScoreStatus.Phi`（且分数=1000000、acc=100），内部 FC 标记置位
- `Status` 与内部 FC 标记分离存储，`ParseStatus` 结果由两者推导
