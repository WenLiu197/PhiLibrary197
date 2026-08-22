---
title: PlayerInfo
icon: book
---

玩家信息（云存档用户）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.HttpModels;

public class PlayerInfo
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `NickName` | `string?` | 昵称，缺失时为 `null`（游戏显示 `guest`） |
| `UserName` | `string` | 用户名 |
| `CreationTime` | `DateTime` | 创建时间 |
| `ModificationTime` | `DateTime` | 修改时间 |
| `ObjectId` | `string` | 用户 object id（可用于存档过滤） |

## 示例

```csharp
PlayerInfo me = await save.GetPlayerInfoAsync();
Console.WriteLine($"昵称: {me.NickName ?? "guest"}");
```
