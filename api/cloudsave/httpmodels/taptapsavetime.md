LeanCloud Date 类型（`__type` + `iso`）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.HttpModels;

public class TapTapSaveTime
```

## 属性（JSON 键名见括号）

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Type` | `string`（`__type`，required） | 类型标记（`Date`） |
| `Time` | `DateTime`（`iso`，required） | ISO 8601 时间 |
