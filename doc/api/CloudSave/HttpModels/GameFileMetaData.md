---
title: GameFileMetaData
icon: file
---

存档文件元数据。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.HttpModels;

public class GameFileMetaData
```

## 属性（JSON 键名见括号）

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Checksum` | `string?`（`_checksum`） | 校验和 |
| `Prefix` | `string?`（`prefix`） | 存储前缀 |
| `Size` | `int?`（`size`） | 文件大小 |
