LeanCloud 登录的组合数据：TapTap token + 用户资料。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.Login;

public class LCCombinedAuthData
```

## 构造函数

```csharp
public LCCombinedAuthData(TapTapProfileData.ProfileData profileData, TapTapTokenData.TokenData tokenData)
```

从 TapTap 资料与 token 构造。

## 属性（JSON 键名见括号）

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Kid` | `string`（`kid`） | MAC key id |
| `Token` | `string`（`access_token`） | TapTap access token |
| `TokenType` | `string`（`token_type`） | token 类型 |
| `MacKey` | `string`（`mac_key`） | MAC 密钥 |
| `MacAlgorithm` | `string`（`mac_algorithm`） | MAC 算法 |
| `OpenID` | `string`（`openid`） | 用户 open id |
| `Name` | `string`（`name`） | 昵称 |
| `Avatar` | `string`（`avatar`） | 头像地址 |
| `UnionID` | `string`（`unionid`） | 用户 union id |

## 示例

```csharp
LCCombinedAuthData auth = new(profile.Data, tokenData.Data);
string sessionToken = await LCHelper.LoginAndGetToken(auth);
```
