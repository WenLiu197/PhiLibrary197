本库全部 JSON 序列化使用 **System.Text.Json 源生成器**，不依赖反射，Native AOT 下可直接使用。

## 序列化上下文

```csharp
// PhiLibrary197 命名空间
public partial class PhiLibrary197JsonSerializerContext : JsonSerializerContext
```

注册类型：`SaveInfoContainer`、`SaveInfo`、`RawScore`、`LCCombinedAuthData`、`TapTapProfileData`、`TapTapTokenData`、`PartialTapTapQRCodeData`、`JsonNode`。

源生成配置与库内部一致：`AllowTrailingCommas`、`PropertyNameCaseInsensitive`、`IncludeFields`。

## 在自己代码中使用

```csharp
using System.Text.Json;
using PhiLibrary197;
using PhiLibrary197.CloudSave.HttpModels;
using PhiLibrary197.LocalSave;

// 反序列化
SaveInfoContainer container = JsonSerializer.Deserialize(
    json, PhiLibrary197JsonSerializerContext.Default.SaveInfoContainer);

RawScore raw = JsonSerializer.Deserialize(
    json, PhiLibrary197JsonSerializerContext.Default.RawScore);

// 序列化
string json = JsonSerializer.Serialize(
    container, PhiLibrary197JsonSerializerContext.Default.SaveInfoContainer);

// 序列化为 JsonNode（拼请求体等）
JsonNode node = JsonSerializer.SerializeToNode(
    authData, PhiLibrary197JsonSerializerContext.Default.LCCombinedAuthData);
```

> [!NOTE]
> `SaveInfo` 的成员类型（`GameFile` 等）与嵌套类型（`TokenData` 等）会自动生成元数据，无需单独注册。

## 自定义类型的源生成

```csharp
using System.Text.Json.Serialization;

[JsonSerializable(typeof(YourModel))]
[JsonSerializable(typeof(PhiLibrary197.CloudSave.SaveInfo))]  // 也可注册库类型
public partial class MyJsonContext : JsonSerializerContext { }
```

## AOT 发布

```xml
<PropertyGroup>
  <PublishAot>true</PublishAot>
</PropertyGroup>
```

```bash
dotnet publish -c Release -r win-x64 --self-contained -p:PublishAot=true
```

库已标记 `IsAotCompatible`，构建时即会暴露任何 AOT 不兼容警告（IL 系列），保证零反射残留。

## 注意事项

1. **不要使用 `JsonSerializer.Deserialize<T>(json)` 无参重载**——默认反射，AOT 下报 IL2026/IL3050 警告
2. **私有别名属性**（`TokenData` 的 `token`/`tokenType`/`macKey`/`macAlgorithm` 键）：与上游行为一致，源生成默认忽略私有属性
3. **未注册类型**：库内部泛型反序列化遇未注册类型会抛 `InvalidOperationException`（fail-fast，正常使用不会触发）
4. **WASM / AOT 混合**：`Save.Decryptor`/`Encryptor`、`LCHelper.GetMD5HashHexString` 的委托注入在 AOT 下同样有效
