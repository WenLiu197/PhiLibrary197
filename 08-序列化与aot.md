库内所有 JSON 序列化都使用 **System.Text.Json 源生成器**，不依赖反射；内置曲目数据和 Tips 是嵌入资源，不依赖当前目录。因此 Native AOT、单文件、trimming 都可以直接使用。

## 源生成上下文

```csharp
// PhiLibrary197 命名空间
public partial class PhiLibrary197JsonSerializerContext : JsonSerializerContext
```

注册类型：

- `SaveInfoContainer`
- `SaveInfo`
- `RawScore`
- `LCCombinedAuthData`
- `TapTapProfileData`
- `TapTapTokenData`
- `PartialTapTapQRCodeData`
- `JsonNode`

配置与库内部一致：

```csharp
AllowTrailingCommas = true
PropertyNameCaseInsensitive = true
IncludeFields = true
```

## 正确用法

```csharp
using System.Text.Json;
using PhiLibrary197;
using PhiLibrary197.LocalSave;
using PhiLibrary197.Scores;

RawScore score = new()
{
    Score = 1_000_000,
    Accuracy = 100f,
    Status = ScoreStatus.Phi,
};

string json = JsonSerializer.Serialize(
    score,
    PhiLibrary197JsonSerializerContext.Default.RawScore);

RawScore roundTrip = JsonSerializer.Deserialize(
    json,
    PhiLibrary197JsonSerializerContext.Default.RawScore);

Console.WriteLine(json);
Console.WriteLine(roundTrip.Score);
```

> [!WARNING]
> 不要使用 `JsonSerializer.Serialize(obj)` / `JsonSerializer.Deserialize<T>(json)` 的无参重载。它们会走反射，AOT 下产生 IL2026/IL3050 警告，运行时也可能失败。

## 自定义类型

自己的类型需要自己的 source-gen context：

```csharp
using System.Text.Json.Serialization;
using PhiLibrary197.CloudSave.HttpModels;

[JsonSerializable(typeof(MyModel))]
[JsonSerializable(typeof(SaveInfo))]
public partial class MyJsonContext : JsonSerializerContext
{
}
```

使用：

```csharp
string json = JsonSerializer.Serialize(model, MyJsonContext.Default.MyModel);
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

单文件 + trim：

```bash
dotnet publish -c Release -r win-x64 `
  -p:PublishSingleFile=true `
  -p:SelfContained=true `
  -p:PublishTrimmed=true
```

`PhiLibrary197` 已标记 `IsAotCompatible`，构建时会显示 AOT 兼容警告。

## 完整可运行示例

```csharp
using System.Text.Json;
using PhiLibrary197;
using PhiLibrary197.LocalSave;

RawScore score = new()
{
    Score = 1_000_000,
    Accuracy = 100f,
    Status = PhiLibrary197.Scores.ScoreStatus.Phi,
};

string json = JsonSerializer.Serialize(
    score,
    PhiLibrary197JsonSerializerContext.Default.RawScore);

Console.WriteLine(json);

RawScore roundTrip = JsonSerializer.Deserialize(
    json,
    PhiLibrary197JsonSerializerContext.Default.RawScore);

Console.WriteLine($"round-trip: {roundTrip.Score}, {roundTrip.Accuracy}, {roundTrip.Status}");
```

运行：

```bash
dotnet run --project samples/PhiLibrary197.Samples -- json
```

输出：

```text
{"s":1000000,"a":100,"c":2}
round-trip: 1000000, 100, Phi
```

## 注意事项

### 私有别名属性

`TapTapTokenData.TokenData` 中有 `token`/`tokenType`/`macKey`/`macAlgorithm` 的私有别名。源生成默认忽略私有属性，这是与上游一致的行为；如果服务端只返回别名，需要自己适配。

### 未注册类型

库内部泛型反序列化遇到未注册类型会抛 `InvalidOperationException`（fail-fast）。正常使用公开 API 不会触发。

### 委托注入

`Save.Decryptor`/`Encryptor`、`LCHelper.GetMD5HashHexString`、`TapTapHelper.Proxy` 都是委托注入，AOT 下同样可用。

### 嵌入资源

`SongDatabase.LoadFromBuiltIn()` 和 `TipProvider.GetTip()` 读取程序集资源，不依赖 `AppContext.BaseDirectory` 或当前工作目录；发布时不要把资源误裁剪掉。库的 `.csproj` 已设置 `EmbeddedResource`，正常引用 NuGet 不需要额外配置。

## 相关文档

- [安装与配置](01-安装与配置.md)
- [示例项目](11-示例项目.md)
- [云存档](03-云存档.md)
