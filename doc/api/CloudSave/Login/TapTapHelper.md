---
title: TapTapHelper
icon: key
---

TapTap 登录辅助类（扫码 / 回调登录）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.Login;

public static class TapTapHelper
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Proxy` | `Func<HttpClient, HttpRequestMessage, Task<HttpResponseMessage>>?` | 请求代理（WASM CORS / 调试） |
| `WebHost` / `ChinaWebHost` | `string` | 国服/国际服 Web 域名 |
| `AccountHost` / `ChinaAccountHost` | `string` | 账号域名 |
| `ApiHost` / `ChinaApiHost` | `string` | API 域名 |

域名选择辅助方法：`GetWebHost(bool)`、`GetAccountHost(bool)`、`GetApiHost(bool)`、`GetCodeUrl(bool)`、`GetTokenUrl(bool)`、`GetProfileUrl(bool, bool)`。

## 方法

### RequestLoginQrCode

```csharp
public static async Task<CompleteQRCodeData> RequestLoginQrCode(
    string[]? permissions = null, bool useChinaEndpoint = true, CancellationToken ct = default)
```

请求扫码登录数据（含二维码 URL 与设备码）。

### CheckQRCodeResult

```csharp
public static async Task<TapTapTokenData?> CheckQRCodeResult(
    CompleteQRCodeData qrCodeData, bool useChinaEndpoint = true, CancellationToken ct = default)
```

轮询扫码结果：已确认返回 token，未确认返回 `null`，未知错误抛 `RequestException`。

### GenerateCallbackLoginUrl

```csharp
public static CallbackLoginData GenerateCallbackLoginUrl(
    string callbackUrl, bool useChinaEndpoint = true, string[]? permissions = null)
```

生成回调登录授权链接（OAuth 2.0，PKCE）。

### HandleCallbackLogin

```csharp
public static async Task<TapTapTokenData> HandleCallbackLogin(
    CallbackLoginData loginData, string code, bool useChinaEndpoint = true, CancellationToken ct = default)
```

用回调 code 换取 token（`code` 从你的回调端点 query 解析）。

### GetProfile

```csharp
public static async Task<TapTapProfileData> GetProfile(
    TapTapTokenData.TokenData token, int timestamp = 0, bool useChinaEndpoint = true, CancellationToken ct = default)
```

用 token 拉取用户资料（MAC 签名请求）。

## 示例

见 [登录认证](../../../04-登录认证.md) 的完整扫码/回调流程。
