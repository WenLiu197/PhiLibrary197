---
title: TapTapProfileData
icon: book
---

TapTap 用户资料数据。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.Login;

public class TapTapProfileData
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Data` | `ProfileData` | 资料数据（JSON 键 `data`） |

## 嵌套类型 ProfileData

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `OpenId` | `string`（`openid`） | 用户 open id |
| `UnionId` | `string`（`unionid`） | 用户 union id |
| `Name` | `string`（`name`） | 昵称 |
| `Gender` | `string`（`gender`） | 性别 |
| `Avatar` | `string`（`avatar`） | 头像地址 |
