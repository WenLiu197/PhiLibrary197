---
title: LCHelper
icon: key
---

LeanCloud 登录辅助类（Phigros 存档后端）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.Login;

public static class LCHelper
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `GetMD5HashHexString` | `Func<string, CancellationToken, Task<string>>` | MD5 实现（WASM 无 MD5 时替换） |

## 方法

### LoginWithAuthData

```csharp
public static async Task<JsonNode> LoginWithAuthData(
    LCCombinedAuthData data,
    bool useChinaEndpoint = true,
    bool failOnNotExist = false,
    CancellationToken ct = default)
```

使用组合登录数据在 LeanCloud 登录，返回原始响应节点（含 `sessionToken` 等字段）。

### LoginAndGetToken

```csharp
public static async Task<string> LoginAndGetToken(
    LCCombinedAuthData data,
    bool useChinaEndpoint = true,
    bool failOnNotExist = false,
    CancellationToken ct = default)
```

登录并直接返回 `sessionToken`（等价于 `LoginWithAuthData(...)["sessionToken"]`）。

> [!NOTE]
> 内部常量 `AppKey` / `ClientId` / `InternationalAppKey` / `InternationalClientId` 为 `internal`，不可从外部访问。

## 示例

```csharp
string sessionToken = await LCHelper.LoginAndGetToken(auth, useChinaEndpoint: false);
Save save = new(sessionToken, isInternational: false);
```
