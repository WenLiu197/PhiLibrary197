---
title: API 参考
icon: book
order: 10
---

本页是 0.3.0 的公开类型速查。逐成员说明可直接看 Visual Studio 的 XML 注释；完整可运行用法见各主题页和 [示例项目](11-示例项目.md)。

## PhiLibrary197

### ScoreHelper

`public static class ScoreHelper`

| 方法 | 说明 |
| --- | --- |
| `ParseStatus(RawScore record)` | 从本地原始成绩推导 `ScoreStatus` |
| `ParseStatus(int score, double accuracy, bool isFc)` | 从原始值推导状态 |
| `DifficultyStringToIndex(string diff)` | `"EZ"/"HD"/"IN"/"AT"` → 索引；未知值抛异常 |
| `LoadConstantTable(string)` / `(TextReader)` / `(Stream)` | 加载 difficulty.tsv |
| `LoadSongInfo(string)` / `(TextReader)` / `(Stream)` | 加载 info.tsv |

### PhiLibrary197JsonSerializerContext

`public partial class PhiLibrary197JsonSerializerContext : JsonSerializerContext`

源生成 JSON 上下文。注册了 `SaveInfoContainer`、`SaveInfo`、`RawScore`、`LCCombinedAuthData`、`TapTapProfileData`、`TapTapTokenData`、`PartialTapTapQRCodeData`、`JsonNode`。

### 异常

| 类型 | 说明 |
| --- | --- |
| `DebugArgumentNullException` | 内部空引用，带调试信息 |
| `MaxValueArgumentOutOfRangeException` | 索引越界 |

## PhiLibrary197.Authentication

### TapTapHelper

`public static class TapTapHelper`

| 成员 | 说明 |
| --- | --- |
| `Proxy` | 自定义 TapTap HTTP 请求委托 |
| `RequestLoginQrCode(...)` | 请求 QRCode 设备码 |
| `CheckQRCodeResult(...)` | 兼容入口；非成功返回 `null` |
| `CheckQRCodeResultDetailed(...)` | 返回 `TapTapQrCodeResult`，区分 pending/denied/expired |
| `GenerateCallbackLoginUrl(...)` | 生成 OAuth 回调登录数据 |
| `StartCallbackLogin(...)` | 启动 localhost 回调服务器，返回 `TapTapCallbackServer` |
| `HandleCallbackLogin(login, code, ...)` | 用 code 换 token（不校验 state） |
| `HandleCallbackLogin(login, code, state, ...)` | 校验 state 后换 token |
| `GetProfile(token, ...)` | 获取 TapTap 用户资料 |
| `GetAccountUrl/GetCodeUrl/GetTokenUrl/GetProfileUrl(...)` | 端点地址 |

### TapTapQrCodeLogin

`public sealed class TapTapQrCodeLogin`

| 成员 | 说明 |
| --- | --- |
| `CreateAsync(permissions, useChinaEndpoint, ct)` | 请求新的 QRCode 登录 |
| `QrCode` | `CompleteQRCodeData` |
| `Url` | 二维码/登录 URL |
| `ExpiresInSeconds` | 有效期 |
| `PollAsync(ct)` | 单次轮询，返回 `TapTapQrCodeResult` |
| `WaitForTokenAsync(ct)` | 自动轮询直到成功/拒绝/过期 |

### TapTapCallbackServer

`public sealed class TapTapCallbackServer : IDisposable`

| 成员 | 说明 |
| --- | --- |
| `Start(port, useChinaEndpoint, path, permissions)` | 启动 localhost 回调服务器 |
| `Port` / `CallbackUrl` / `BeginUrl` | 实际端口和 URL |
| `LoginData` | `CallbackLoginData` |
| `WaitForCallbackAsync(ct)` | 等待并校验回调，返回 code/state |
| `WaitForTokenAsync(ct)` | 等待回调并直接换 token |
| `Dispose()` | 停止监听 |

### LCHelper

`public static class LCHelper`

| 成员 | 说明 |
| --- | --- |
| `LoginWithAuthData(data, ...)` | 登录 LeanCloud，返回原始 JSON |
| `LoginAndGetToken(data, ...)` | 登录并返回 session token |
| `GetMD5HashHexString` | 可替换的 MD5 实现 |

### 登录模型

| 类型 | 关键成员 |
| --- | --- |
| `TapTapTokenData` | `Data.TokenData`：`Kid`、`Token`、`TokenType`、`MacKey`、`MacAlgorithm`、`Scope` |
| `TapTapProfileData` | `Data.ProfileData`：`OpenId`、`UnionId`、`Name`、`Gender`、`Avatar`、`Bio` |
| `LCCombinedAuthData` | `Kid`、`Token`、`TokenType`、`MacKey`、`MacAlgorithm`、`OpenID`、`Name`、`Avatar`、`UnionID` |
| `CompleteQRCodeData` | `DeviceID`、`DeviceCode`、`ExpiresInSeconds`、`Url`、`Interval` |
| `PartialTapTapQRCodeData` | 设备码原始响应 |
| `CallbackLoginData` | `BeginUrl`、`CodeVerifier`、`RedirectUrl`、`State`、`CodeChallenge`、`Scope` |
| `TapTapQrCodeResult` | `Status`、`Token` |
| `TapTapCallbackResult` | `Code`、`State` |
| `RequestException` | `Failing`、`HttpStatus` |

### 枚举

`TapTapQrCodeStatus`

```text
Pending, Waiting, SlowDown, Denied, Expired, Success, Unknown
```

`RequestException.FailingType`

```text
Pending, Denied, Waiting, SlowDown, Expired, None, Unknown
```

## PhiLibrary197.CloudSave

### Save

`public class Save : IDisposable`

| 成员 | 说明 |
| --- | --- |
| `new Save(sessionToken, isInternational)` | 创建实例；token 25 位字母数字 |
| `SharedClient` | 静态共享 `HttpClient` |
| `RequestHandler` | 请求拦截 |
| `Decryptor` / `Encryptor` | AES 委托 |
| `IsSemanticallyValidToken(string)` | 格式校验 |
| `GetSaveInfoFromCloudAsync(queries, ct)` | 查询存档列表 |
| `GetSaveZipAsync(PhiCloudObj/SimplifiedSaveInfo, ct)` | 下载原始 zip |
| `GetSaveContextAsync(int index, ...)` | 按索引下载并解密 |
| `GetSaveContextAsync(SaveInfo, ct)` | 按 `SaveInfo` 下载并解密 |
| `GetRawAddressAsync(address, ct)` | GET 任意地址 |
| `GetPlayerInfoAsync(ct)` / `GetUserObjectId()` | 玩家信息 |

### SaveContext

`public class SaveContext`

| 成员 | 说明 |
| --- | --- |
| `FromZipAsync(byte[]/Stream, SaveInfo, CipherFunction, ct)` | 带云端元数据构造 |
| `FromZipAsync(byte[]/Stream, CipherFunction, ct)` | 无 `SaveInfo`；`ReadSummary` 不可用 |
| `ReadSummary/GameRecord/GameSettings/GameProgress/GameKey/GameUserInfo()` | 读取条目 |
| `SaveSummary/SaveGameRecord/...` | 更新内存条目 |
| `SaveToStreamAsync(Stream, CipherFunction, ct)` | 输出新 zip |
| `SaveToZipAsync(ZipArchive, CipherFunction, ct)` | 写入现有 zip |

### 云存档模型

| 类型 | 说明 |
| --- | --- |
| `Summary` | 游戏统计、挑战码、显示 RKS |
| `PlayCountSummary` | 通关/FC/Phi 计数 |
| `Challenge` | 挑战码 |
| `Money` | 大数货币 |
| `GameProgress` | 解锁进度与 Node2/3/4 |
| `GameSettings` | 设置 |
| `GameUserInfo` | 用户信息 |
| `GameKey` / `GameKeyFlag` | Key 数据 |
| 枚举 | `ChallengeRank`、`DifficultyUnlockFlag`、`RandomVersionFlag`、`SongRecordFlag`、`Chapter8UnlockFlag`、`TakumiUnlockFlag`、`GameKeyFlagType` |

### HttpModels

| 类型 | 说明 |
| --- | --- |
| `SaveInfoContainer` | 存档列表；`Results` 默认空列表 |
| `SaveInfo` | 存档元数据；`GameFile` 可空 |
| `GameFile` / `GameFileMetaData` | 云文件信息 |
| `TapTapSaveTime` / `TapTapUserInfo` | 时间与用户 |
| `PlayerInfo` | 玩家信息 |
| `SimplifiedSaveInfo` | `GameSave`、时间、`Summary`、`Name`、`ObjectId` |
| `PhiCloudObj` | 简化文件引用 |

## PhiLibrary197.LocalSave

| 类型 | 说明 |
| --- | --- |
| `LocalSave` | `DecryptLocalSaveStringNew(string/byte[])` |
| `RawScore` | `Score`、`Accuracy`、`Status`、`FromJson`、`ToSongScore` |

> [!NOTE]
> `LocalSave` 类与 `PhiLibrary197.LocalSave` 命名空间同名，建议用 `using LocalSaveApi = PhiLibrary197.LocalSave.LocalSave;` 避免歧义。

## PhiLibrary197.Scores

| 类型 | 说明 |
| --- | --- |
| `Difficulty` | `EZ/HD/IN/AT/Legacy/SP` |
| `ScoreStatus` | `Bugged/NotFc/Fc/Phi/Vu/S/A/B/C/False` |
| `SongScore` | 单曲原始成绩 |
| `CompleteScore` | 成绩 + 定数 + 名称 + RKS |
| `GameRecord` | 成绩容器与 RKS 计算 |
| `ChartConstantKey` | `(SongId, Difficulty)` |
| `ExportScore` | Excel 友好模型，不负责输出 |

`GameRecord`：

```csharp
IEnumerable<CompleteScore> GetCompleteScores(...);
(List<CompleteScore> Phis, List<CompleteScore> OtherScores, double Rks)
    GetSortedListForRks(...);
(List<CompleteScore> Scores, double Rks) GetSortedListForRksMerged(...);
```

## PhiLibrary197.Songs

| 类型 | 说明 |
| --- | --- |
| `SongDatabase` | 内置/文件/Reader/Stream 加载曲目数据 |
| `SongEntry` | 曲目信息、变体 `BaseId`/`IsVariant`、`GetConstant`、`GetCharter` |

`SongDatabase`：

```csharp
LoadFromBuiltIn();
LoadFromFile(string difficultyPath, string infoPath);
LoadFromFile(TextReader difficulty, TextReader info);
LoadFromFile(Stream difficulty, Stream info);
Count; Songs; GetSong(id); FindByFullName(name);
ToConstantMap(); ToNameMap();
```

## PhiLibrary197.Tips

| 类型 | 说明 |
| --- | --- |
| `TipProvider` | `public static string GetTip()`，随机返回一条内置简中 Tip |

## 已移除/重命名

0.3.0 对旧文档中的这些名字做了破坏性调整：

| 旧 | 新 |
| --- | --- |
| `PhiLibrary197.CloudSave.Login` | `PhiLibrary197.Authentication` |
| `PhiLibrary197.CloudSave.SongDatabase/SongEntry` | `PhiLibrary197.Songs` |
| `PhiLibrary197.CloudSave.GameRecord/SongScore/...` | `PhiLibrary197.Scores` |
| `FindByName` | `FindByFullName` |
| `GameKey.Flags` 之类的旧描述 | 以当前 XML 注释为准 |
