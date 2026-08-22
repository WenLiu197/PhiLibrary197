云端存档信息（LeanCloud 对象模型）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.HttpModels;

public class SaveInfo
```

## 属性（JSON 键名见括号）

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `CreatedAt` | `DateTime`（`createdAt`，required） | 创建时间 |
| `GameFile` | `GameFile?`（`gameFile`） | 存档文件，**可为 null**（损坏/缺失场景） |
| `ModifiedAt` | `TapTapSaveTime`（`modifiedAt`，required） | 修改时间 |
| `Name` | `string`（`name`，required） | 存档名 |
| `ObjectId` | `string`（`objectId`，required） | 对象 id |
| `Summary` | `string`（`summary`，required） | 游玩统计（base64） |
| `UpdatedAt` | `DateTime`（`updatedAt`，required） | 更新时间 |
| `User` | `TapTapUserInfo`（`user`，required） | 上传用户 |

## 方法

```csharp
public SimplifiedSaveInfo Simplify()
```

转为简化存档（`GameFile` 为 null 时 `GameSave.Url` 为 null）。

> [!WARNING]
> 用 `GameFile` 为 `null` 的 `SaveInfo` 获取存档上下文会抛异常，访问前先判空。
