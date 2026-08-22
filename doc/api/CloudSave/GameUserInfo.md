---
title: GameUserInfo
icon: book
---

游戏内用户信息。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public class GameUserInfo : IPhigrosCustomSerialization<GameUserInfo>
```

## 构造函数

```csharp
public GameUserInfo(byte version, bool showUserId, string intro, string avatarId, string backgroundId)
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Version` | `byte` | 版本，最新 1 |
| `ShowUserId` | `bool` | 是否展开显示用户名 |
| `Intro` | `string` | 个人简介 |
| `AvatarId` | `string` | 头像 id |
| `BackgroundId` | `string` | 背景 id |
