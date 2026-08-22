---
title: ChartConstantKey
icon: key
---

定数查找键：曲目 id + 难度的组合。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public record struct ChartConstantKey(string SongId, Difficulty Difficulty)
```

## 参数

| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `SongId` | `string` | 曲目完整 id，含数字后缀（如 `Stasis.Maozon.0`） |
| `Difficulty` | `Difficulty` | 难度 |

## 备注

- `record struct`：自动获得值相等、`GetHashCode`、`ToString`、`Deconstruct`
- 用作定数表（`Dictionary<ChartConstantKey, float>`）的键：

```csharp
Dictionary<ChartConstantKey, float> map = new()
{
    [new("Stasis.Maozon.0", Difficulty.AT)] = 16.8f,
    [new("Credits.Frums.0", Difficulty.IN)] = 14.0f,
};
```
