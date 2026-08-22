单曲完整信息（`SongDatabase` 的元素）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public sealed record SongEntry(
    string Id,
    string? Name,
    string? Composer,
    string? Illustrator,
    string? EzCharter,
    string? HdCharter,
    string? InCharter,
    string? AtCharter,
    IReadOnlyDictionary<Difficulty, float> Constants)
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Id` | `string` | 完整曲目 id（如 `Glaciaxion.SunsetRay.0`） |
| `Name` | `string?` | 显示名（info 第 2 列），缺失为 `null` |
| `Composer` | `string?` | 曲师（info 第 3 列） |
| `Illustrator` | `string?` | 曲绘画师（info 第 4 列） |
| `EzCharter` | `string?` | EZ 谱师（info 第 5 列） |
| `HdCharter` | `string?` | HD 谱师（info 第 6 列） |
| `InCharter` | `string?` | IN 谱师（info 第 7 列） |
| `AtCharter` | `string?` | AT 谱师（info 第 8 列），无 AT 谱为 `null` |
| `Constants` | `IReadOnlyDictionary<Difficulty, float>` | 各难度定数（仅收录表中存在的难度） |

## 方法

### GetConstant

```csharp
public float? GetConstant(Difficulty difficulty)
```

取指定难度定数，表中不存在返回 `null`。

## 示例

```csharp
SongEntry song = db.GetSong("Chronostasis.黒皇帝")!;
Console.WriteLine($"{song.Name} | 曲师 {song.Composer} | IN {song.GetConstant(Difficulty.IN)}");
```
