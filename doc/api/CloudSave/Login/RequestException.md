---
title: RequestException
icon: book
---

TapTap 登录请求返回未知响应时抛出的异常。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.Login;

public class RequestException : Exception
```

## 说明

- 构造函数为 `internal`，由库内部抛出（如 `CheckQRCodeResult` 收到无法识别的错误响应时）
- 内部状态 `FailingType`（`Pending` / `Denied` / `Waiting` / `None` / `Unknown`）与 `HttpStatusCode` 为 **`internal`**，外部无法直接访问
- 通过 `ToString()` 获取失败类型与状态码描述：

```csharp
catch (RequestException ex)
{
    Console.WriteLine(ex.ToString());   // 如 "Unknown, Unauthorized: ..."
}
```

> [!NOTE]
> 扫码登录轮询中，`RequestException` 的 `Failing` 状态可区分"等待确认/被拒绝/未知错误"；由于该属性为 internal，外部判断需解析 `ToString()` 或 `Message`。
