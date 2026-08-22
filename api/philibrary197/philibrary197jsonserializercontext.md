本库全部 JSON 序列化的源生成上下文，Native AOT 友好。

## 定义

```csharp
namespace PhiLibrary197;

[JsonSourceGenerationOptions(AllowTrailingCommas = true, PropertyNameCaseInsensitive = true, IncludeFields = true)]
[JsonSerializable(...)]
public partial class PhiLibrary197JsonSerializerContext : JsonSerializerContext
```

## 注册类型

| 类型 | 说明 |
| --- | --- |
| `SaveInfoContainer` | 存档列表容器 |
| `SaveInfo` | 存档信息（成员类型自动包含） |
| `RawScore` | 本地成绩 |
| `LCCombinedAuthData` | 登录组合数据 |
| `TapTapProfileData` | TapTap 资料 |
| `TapTapTokenData` | TapTap token |
| `PartialTapTapQRCodeData` | 扫码数据 |
| `JsonNode` | DOM 节点 |

## 使用

```csharp
using System.Text.Json;
using PhiLibrary197;

// 反序列化
SaveInfoContainer container = JsonSerializer.Deserialize(
    json, PhiLibrary197JsonSerializerContext.Default.SaveInfoContainer);

// 序列化
string json = JsonSerializer.Serialize(
    container, PhiLibrary197JsonSerializerContext.Default.SaveInfoContainer);
```

> [!IMPORTANT]
> 请勿使用 `JsonSerializer.Deserialize<T>(json)` 无参重载（默认反射，AOT 下报 IL2026/IL3050 警告）。

## 备注

- 生成配置与库内部一致：`AllowTrailingCommas`、`PropertyNameCaseInsensitive`、`IncludeFields`
- `TokenData` 的私有别名属性（`token`/`tokenType`/`macKey`/`macAlgorithm` 键）与上游行为一致，默认忽略
- 完整用法见 [序列化与 AOT](../../08-序列化与AOT.md)
