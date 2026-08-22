存档列表容器（LeanCloud 查询响应）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.HttpModels;

public struct SaveInfoContainer
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Results` | `List<SaveInfo>` | 存档列表（JSON 键 `results`） |

## 方法

```csharp
public readonly List<SimplifiedSaveInfo> GetParsedSaves()
```

将全部 `SaveInfo` 转为简化存档列表。
