解密后的存档上下文：包含各数据条目，支持读取、修改与加密回写。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public class SaveContext
```

## 嵌套类型

### Entry（存档条目）

```csharp
public struct Entry
{
    public byte ObjectVersion { get; set; }   // 条目版本
    public byte[] Data { get; set; }          // 解密后的数据字节
    public Entry(byte objectVersion, byte[] data);
}
```

### CipherFunction（加解密委托）

```csharp
public delegate Task<byte[]> CipherFunction(byte[] data, CancellationToken ct = default)
```

## 构造函数

```csharp
public SaveContext(Dictionary<string, Entry> decryptedEntries, SaveInfo originalData)
```

> [!NOTE]
> 推荐使用静态工厂 `FromZipAsync` 构造，不要直接调用构造函数。

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `RawSummary` | `byte[]` | 原始 summary 数据（base64 解码后） |
| `OriginalCloudObject` | `SaveInfo` | 原始存档信息 |
| `DecryptedDataEntries` | `Dictionary<string, Entry>` | 全部解密条目（按 zip 内文件名索引） |
| `DecryptedGameRecord` | `Entry` | 成绩条目（键 `gameRecord`） |
| `DecryptedGameProgress` | `Entry` | 进度条目（键 `gameProgress`） |
| `DecryptedGameKey` | `Entry` | Key 条目（键 `gameKey`） |
| `DecryptedGameSettings` | `Entry` | 设置条目（键 `settings`） |
| `DecryptedGameUserInfo` | `Entry` | 用户信息条目（键 `user`） |

## 静态方法

### FromZipAsync

```csharp
public static async Task<SaveContext> FromZipAsync(
    byte[] rawZip, SaveInfo originalData, CipherFunction decryptor, CancellationToken ct = default)

public static async Task<SaveContext> FromZipAsync(
    Stream rawZip, SaveInfo originalData, CipherFunction decryptor, CancellationToken ct = default)
```

从加密 zip（字节或流）构造上下文，逐条目调用 `decryptor` 解密。

## 读取方法

| 方法 | 返回 |
| --- | --- |
| `ReadSummary()` | [Summary](Summary.md) |
| `ReadGameRecord()` | [GameRecord](GameRecord.md) |
| `ReadGameSettings()` | [GameSettings](GameSettings.md) |
| `ReadGameProgress()` | [GameProgress](GameProgress.md) |
| `ReadGameKey()` | [GameKey](GameKey.md) |
| `ReadGameUserInfo()` | [GameUserInfo](GameUserInfo.md) |

## 保存方法

| 方法 | 说明 |
| --- | --- |
| `SaveSummary(Summary)` | 写回游玩统计（更新 `RawSummary`） |
| `SaveGameRecord(GameRecord)` | 写回成绩 |
| `SaveGameSettings(GameSettings)` | 写回设置 |
| `SaveGameProgress(GameProgress)` | 写回进度 |
| `SaveGameKey(GameKey)` | 写回 Key |
| `SaveGameUserInfo(GameUserInfo)` | 写回用户信息 |

## 回写 zip

```csharp
public async Task SaveToZipAsync(ZipArchive archive, CipherFunction encryptor, CancellationToken ct = default)
public async Task SaveToStreamAsync(Stream zipStream, CipherFunction encryptor, CancellationToken ct = default)
```

将全部条目加密写入 zip 归档/流（流版本保持打开状态）。

## 示例

```csharp
SaveContext ctx = await save.GetSaveContextAsync(0);

// 读取
var record = ctx.ReadGameRecord();
var progress = ctx.ReadGameProgress();

// 修改
progress.Money = Money.Zero;
ctx.SaveGameProgress(progress);

// 加密回写为 zip
using MemoryStream ms = new();
await ctx.SaveToStreamAsync(ms, save.Encrypt);
byte[] newZip = ms.ToArray();
```
