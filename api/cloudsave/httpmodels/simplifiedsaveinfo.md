简化存档信息（只保留常用字段）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.HttpModels;

public class SimplifiedSaveInfo
```

## 属性（全部 `required`）

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `GameSave` | `PhiCloudObj` | 存档文件对象（含下载 URL） |
| `CreationDate` | `DateTime` | 创建时间 |
| `ModificationTime` | `DateTime` | 修改时间 |
| `Summary` | `string` | 游玩统计（base64） |
