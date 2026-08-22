参数越界时抛出的异常，用于"最大可用值"可确定的场景。

## 定义

```csharp
namespace PhiLibrary197;

public class MaxValueArgumentOutOfRangeException : ArgumentOutOfRangeException
```

## 备注

- 构造函数为 `internal`，由库内部抛出（如 `Save.GetSaveContextAsync` 的存档索引越界时）
- 继承 `ArgumentOutOfRangeException`，可用标准的 `ParamName` / `ActualValue` / `Message` 属性获取详情
- 示例触发场景：

```csharp
// 存档只有 1 条，请求索引 5
SaveContext ctx = await save.GetSaveContextAsync(5); // 抛 MaxValueArgumentOutOfRangeException
```
