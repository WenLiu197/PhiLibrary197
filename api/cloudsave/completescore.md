完整成绩：在 [SongScore](SongScore.md) 基础上附加定数与歌名，用于 RKS 计算与展示。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public struct CompleteScore :
    IComparable<CompleteScore>, IEquatable<CompleteScore>, IEqualityOperators<CompleteScore, CompleteScore, bool>
```

## 静态属性

```csharp
public static CompleteScore Default { get; }
```

占位用空成绩（使用两个空映射：定数恒 0、歌名恒空）。访问 `Rks` / `Name` 会抛异常，仅用于列表填充。

## 构造函数

```csharp
public CompleteScore(
    SongScore score,
    IReadOnlyDictionary<ChartConstantKey, float> constantMap,
    IReadOnlyDictionary<string, string> nameMap)
```

> [!WARNING]
> `constantMap` 必须是真实定数表（`GameRecord` 用 RKS 排序），不要传 mock 表。

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Score` | `SongScore` | 基础成绩（可读写） |
| `Name` | `string` | 歌名，nameMap 查不到抛 `KeyNotFoundException` |
| `NameOrDefault` | `string` | 歌名，查不到返回曲目 id |
| `ChartConstant` | `float` | 定数，constantMap 查不到抛 `KeyNotFoundException` |
| `Rks` | `double` | 单曲 RKS：`acc < 70 ? 0 : ((acc - 55) / 45)² × 定数` |

## 方法

### CompareTo

```csharp
public readonly int CompareTo(CompleteScore other)
```

按 RKS **降序**比较（RKS 高的排前面）。

### Equals

```csharp
public readonly bool Equals(CompleteScore other)
public override readonly bool Equals(object? obj)
public override readonly int GetHashCode()
public static bool operator ==(CompleteScore left, CompleteScore right)
public static bool operator !=(CompleteScore left, CompleteScore right)
```

按基础成绩 + 定数 + 歌名（`NameOrDefault`）比较相等。

## 示例

```csharp
foreach (CompleteScore s in record.GetCompleteScores(constantMap, nameMap))
{
    Console.WriteLine($"{s.NameOrDefault} | {s.Score.Difficulty} | 定数 {s.ChartConstant} | RKS {s.Rks:F4}");
}
```
