存档文件信息（LeanCloud File 对象）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.HttpModels;

public class GameFile
```

## 属性（JSON 键名见括号）

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Type` | `string`（`__type`，required） | 类型标记 |
| `Bucket` | `string`（`bucket`，required） | 存储桶 |
| `CreatedAt` | `DateTime`（`createdAt`，required） | 创建时间 |
| `Key` | `string`（`key`，required） | 存储键 |
| `MetaData` | `GameFileMetaData`（`metaData`，required） | 元数据 |
| `MimeType` | `string`（`mime_type`，required） | MIME 类型 |
| `Name` | `string`（`name`，required） | 文件名 |
| `ObjectId` | `string`（`objectId`，required） | 对象 id |
| `Provider` | `string`（`provider`，required） | 存储服务商 |
| `UpdatedAt` | `DateTime`（`updatedAt`，required） | 更新时间 |
| `Url` | `string`（`url`，required） | 下载地址 |
