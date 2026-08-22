---
title: TapTapUserInfo
icon: book
---

LeanCloud Pointer 类型（上传用户引用）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.HttpModels;

public class TapTapUserInfo
```

## 属性（JSON 键名见括号）

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Type` | `string`（`__type`，required） | 类型标记（`Pointer`） |
| `ClassName` | `string`（`className`，required） | 类名（`_User`） |
| `ObjectId` | `string`（`objectId`，required） | 用户 object id |
