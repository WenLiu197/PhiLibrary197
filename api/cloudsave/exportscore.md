Excel 友好的成绩导出模型。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public class ExportScore
```

## 属性（全部 `required`）

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `ID` | `string` | 曲目 id（如 `Stasis.Maozon.0`） |
| `Name` | `string` | 歌名（如 `Stasis`） |
| `Difficulty` | `string` | 难度名（如 `AT`） |
| `ChartConstant` | `float` | 定数（如 11.4） |
| `Score` | `int` | 分数 |
| `Acc` | `double` | 准确率 |
| `RksGiven` | `double` | 单曲 RKS |
| `Status` | `string` | 成绩状态（如 `A`） |

## 示例

```csharp
var export = record.GetCompleteScores(constantMap, nameMap)
    .Select(s => new ExportScore
    {
        ID = s.Score.Id,
        Name = s.NameOrDefault,
        Difficulty = s.Score.Difficulty.ToString(),
        ChartConstant = s.ChartConstant,
        Score = s.Score.Score,
        Acc = s.Score.Accuracy,
        RksGiven = s.Rks,
        Status = s.Score.Status.ToString(),
    });
```
