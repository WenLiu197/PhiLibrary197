单难度的通关计数统计。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public struct PlayCountSummary : IPhigrosCustomSerialization<PlayCountSummary>
```

## 构造函数

```csharp
public PlayCountSummary(short cleared, short fullCombo, short phi)
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `ClearedCount` | `short` | 通关数（含 FC 与 Phi） |
| `FullComboCount` | `short` | FC 数（含 Phi） |
| `PhiCount` | `short` | Phi 数 |

> [!NOTE]
> Summary 内的计数可能不准确，请以 `GameRecord` 计算为准。
