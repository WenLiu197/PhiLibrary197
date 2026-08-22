---
title: TapTapTokenData
icon: shield-check
---

TapTap 登录 token 数据。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.Login;

public class TapTapTokenData
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Data` | `TokenData` | token 数据（JSON 键 `data`） |

## 嵌套类型 TokenData

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Kid` | `string`（`kid`） | MAC key id |
| `Token` | `string`（`access_token`） | access token |
| `TokenType` | `string`（`token_type`） | token 类型 |
| `MacKey` | `string`（`mac_key`） | MAC 密钥 |
| `MacAlgorithm` | `string`（`mac_algorithm`） | MAC 算法（如 `hmac-sha-256`） |
| `Scope` | `string`（`scope`） | 授权范围（如 `public_profile`） |

> [!NOTE]
> 内部存在 `token` / `tokenType` / `macKey` / `macAlgorithm` 私有别名属性（兼容旧 JSON 键），源生成默认忽略，不影响解析行为。
