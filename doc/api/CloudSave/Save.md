---
title: Save
icon: cloud
---

Phigros 云存档访问的入口类，封装了 LeanCloud 请求、存档查询、下载与解密。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public class Save : IDisposable
```

## 常量

| 名称 | 值 | 说明 |
| --- | --- | --- |
| `CloudServerAddress` | `https://rak3ffdi.cloud.tds1.tapapis.cn` | 国服存档服务器 |
| `InternationalCloudServerAddress` | `https://kviehlel.cloud.ap-sg.tapapis.com` | 国际服存档服务器 |

```csharp
public static string GetCloudServerAddress(bool useChinaEndpoint)
```

按参数返回对应服务器地址。

## 构造函数

```csharp
public Save(string sessionToken, bool isInternational)
```

| 参数 | 说明 |
| --- | --- |
| `sessionToken` | LeanCloud session token（25 位字母数字），格式非法抛 `ArgumentException` |
| `isInternational` | `false` = 国服，`true` = 国际服 |

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `SessionToken` | `string` | 会话 token（`private init`） |
| `IsInternational` | `bool` | 是否国际服 |
| `SharedClient` | `HttpClient`（静态） | 所有实例共享的 HttpClient，可整体替换 |
| `RequestHandler` | `Func<Save, HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>` | 请求拦截/代理钩子 |
| `Decryptor` | `AESCipherFunction` | 自定义解密（WASM 等无 AES 环境必换） |
| `Encryptor` | `AESCipherFunction` | 自定义加密 |

## 委托

```csharp
public delegate Task<byte[]> AESCipherFunction(byte[] key, byte[] iv, byte[] data, CancellationToken ct = default)
```

## 静态方法

### IsSemanticallyValidToken

```csharp
public static bool IsSemanticallyValidToken(string sessionToken)
```

检查 token 是否语义合法（25 位字母数字）。不联网。

## 存档查询与下载

### GetSaveInfoFromCloudAsync

```csharp
public async Task<SaveInfoContainer> GetSaveInfoFromCloudAsync(
    ICollection<KeyValuePair<string, string>>? queries = null,
    CancellationToken ct = default)
```

查询存档列表。`queries` 为 `null` 时按当前用户过滤（默认查询）；传自定义参数（如 `limit`/`skip`）时需自行携带用户过滤条件。

### GetSaveZipAsync

```csharp
public Task<byte[]> GetSaveZipAsync(PhiCloudObj obj, CancellationToken ct = default)
public Task<byte[]> GetSaveZipAsync(SimplifiedSaveInfo obj, CancellationToken ct = default)
```

下载加密存档 zip 的原始字节。

### GetSaveContextAsync

```csharp
public async Task<SaveContext> GetSaveContextAsync(
    int index,
    ICollection<KeyValuePair<string, string>>? queries = null,
    CancellationToken ct = default)

public async Task<SaveContext> GetSaveContextAsync(SaveInfo rawSave, CancellationToken ct = default)
```

按索引（0 = 最新）或按 `SaveInfo` 下载并解密存档，返回 [SaveContext](SaveContext.md)。索引越界抛 `MaxValueArgumentOutOfRangeException`。

### GetRawAddressAsync

```csharp
public async Task<byte[]> GetRawAddressAsync(string address, CancellationToken ct = default)
```

对任意地址发起 GET 并返回原始字节。

## 用户信息

### GetPlayerInfoAsync

```csharp
public async Task<PlayerInfo> GetPlayerInfoAsync(CancellationToken ct = default)
```

获取当前用户信息，并缓存用户 objectId。

### GetUserObjectId

```csharp
public async ValueTask<string> GetUserObjectId()
```

获取用户 objectId（带缓存）。

## 加解密

```csharp
public Task<byte[]> Decrypt(byte[] data, CancellationToken ct = default)
public Task<byte[]> Encrypt(byte[] data, CancellationToken ct = default)
```

使用 Phigros 固定的 key/iv 加解密（实际调用 `Decryptor` / `Encryptor`）。

## 资源释放

```csharp
public virtual void Dispose()
```

`GC.SuppressFinalize`（v5 起不释放共享客户端）。

## 示例

```csharp
Save save = new("你的25位token", isInternational: false);
SaveContext ctx = await save.GetSaveContextAsync(0);
var summary = ctx.ReadSummary();
Console.WriteLine($"RKS: {summary.Rks}");

// 请求拦截示例
save.RequestHandler = async (s, request, ct) =>
{
    request.Headers.Add("X-Custom", "1");
    return await Save.SharedClient.SendAsync(request, ct);
};
```

> [!WARNING]
> 调用 `GetSaveContextAsync` 前确保 `SaveInfo.GameFile` 不为 `null`（v5 起可为 null，见 [SaveInfo](HttpModels/SaveInfo.md)）。
