---
title: CallbackLoginData
icon: link
---

回调登录数据（`GenerateCallbackLoginUrl` 的返回值）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.Login;

public class CallbackLoginData
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `BeginUrl` | `string` | 用户开始登录的授权链接 |
| `CodeVerifier` | `string` | PKCE code verifier（换 token 时使用） |
| `RedirectUrl` | `string` | 授权完成后的回调地址 |
| `State` | `string` | 防 CSRF 状态值 |
| `CodeChallenge` | `string` | PKCE code challenge |
| `Scope` | `string` | 授权范围（如 `public_profile`） |
