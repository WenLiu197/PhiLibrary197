---
title: DebugArgumentNullException
icon: book
---

带调试附加信息的 `ArgumentNullException`。

## 定义

```csharp
namespace PhiLibrary197;

public class DebugArgumentNullException : ArgumentNullException, IFormattable
```

库内部空值检查抛出的异常，附加消息可能包含请求响应等调试信息。

## 构造函数

```csharp
public DebugArgumentNullException(string? paramName, string? additionalMessage)
```

| 参数 | 说明 |
| --- | --- |
| `paramName` | 引发异常的参数名 |
| `additionalMessage` | 附加调试消息 |

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `AdditionalMessage` | `string?` | 附加调试消息，默认不包含在 `ToString()` 中 |

## 方法

### ToStringDetailed

```csharp
public string ToStringDetailed()
```

返回包含附加消息的完整异常描述（`基础信息 + "\nAdditional message: ..."`）。

### ToString(string?, IFormatProvider?)

```csharp
public string ToString(string? format, IFormatProvider? formatProvider)
```

`IFormattable` 实现。`format == "D"` 时等价于 `ToStringDetailed()`，否则等价于 `ToString()`。

> [!WARNING]
> 详细消息可能包含敏感数据（如服务器响应原文），只用于后端/日志排查，**不要直接展示给玩家**。
