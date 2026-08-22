按命名空间分类的完整 API 参考（微软 .NET 文档风格，每个类独立页面）。

## PhiLibrary197（根命名空间）

| 类型 | 说明 |
| --- | --- |
| [ScoreHelper](api/PhiLibrary197/ScoreHelper.md) | 成绩状态判定、定数表/歌名表加载 |
| [PhiLibrary197JsonSerializerContext](api/PhiLibrary197/PhiLibrary197JsonSerializerContext.md) | JSON 源生成序列化上下文 |
| [IPhigrosCustomSerialization\<TSelf\>](api/PhiLibrary197/IPhigrosCustomSerialization.md) | 自定义二进制序列化接口 |
| [DebugArgumentNullException](api/PhiLibrary197/DebugArgumentNullException.md) | 带调试信息的空引用异常 |
| [MaxValueArgumentOutOfRangeException](api/PhiLibrary197/MaxValueArgumentOutOfRangeException.md) | 越界参数异常 |

## PhiLibrary197.CloudSave

| 类型 | 说明 |
| --- | --- |
| [Save](api/CloudSave/Save.md) | 云存档访问入口 |
| [SaveContext](api/CloudSave/SaveContext.md) | 解密后的存档上下文（读取/修改/回写） |
| [GameRecord](api/CloudSave/GameRecord.md) | 成绩容器 + RKS 计算 |
| [SongScore](api/CloudSave/SongScore.md) | 单曲成绩 |
| [CompleteScore](api/CloudSave/CompleteScore.md) | 完整成绩（含定数/RKS） |
| [ChartConstantKey](api/CloudSave/ChartConstantKey.md) | 定数查找键 |
| [Summary](api/CloudSave/Summary.md) / [PlayCountSummary](api/CloudSave/PlayCountSummary.md) | 游玩统计 |
| [Challenge](api/CloudSave/Challenge.md) | 挑战码 |
| [Money](api/CloudSave/Money.md) | 货币 |
| [GameProgress](api/CloudSave/GameProgress.md) | 解锁进度（含 Node2/3/4） |
| [GameSettings](api/CloudSave/GameSettings.md) / [GameUserInfo](api/CloudSave/GameUserInfo.md) | 设置与用户信息 |
| [GameKey](api/CloudSave/GameKey.md) / [GameKeyFlag](api/CloudSave/GameKeyFlag.md) | Key 数据 |
| [ExportScore](api/CloudSave/ExportScore.md) | Excel 友好导出模型 |
| [SongDatabase](api/CloudSave/SongDatabase.md) / [SongEntry](api/CloudSave/SongEntry.md) | 曲目数据库 |
| [枚举](api/CloudSave/enums.md) | 全部枚举汇总 |

### HttpModels 子命名空间

| 类型 | 说明 |
| --- | --- |
| [SaveInfoContainer](api/CloudSave/HttpModels/SaveInfoContainer.md) | 存档列表容器 |
| [SaveInfo](api/CloudSave/HttpModels/SaveInfo.md) / [GameFile](api/CloudSave/HttpModels/GameFile.md) | 存档信息 |
| [GameFileMetaData](api/CloudSave/HttpModels/GameFileMetaData.md) / [TapTapSaveTime](api/CloudSave/HttpModels/TapTapSaveTime.md) / [TapTapUserInfo](api/CloudSave/HttpModels/TapTapUserInfo.md) | 存档元数据 |
| [PlayerInfo](api/CloudSave/HttpModels/PlayerInfo.md) | 玩家信息 |
| [SimplifiedSaveInfo](api/CloudSave/HttpModels/SimplifiedSaveInfo.md) / [PhiCloudObj](api/CloudSave/HttpModels/PhiCloudObj.md) | 简化存档 |

### Login 子命名空间

| 类型 | 说明 |
| --- | --- |
| [LCHelper](api/CloudSave/Login/LCHelper.md) | LeanCloud 登录 |
| [TapTapHelper](api/CloudSave/Login/TapTapHelper.md) | TapTap 登录 |
| [LCCombinedAuthData](api/CloudSave/Login/LCCombinedAuthData.md) | 组合登录数据 |
| [TapTapTokenData](api/CloudSave/Login/TapTapTokenData.md) / [TapTapProfileData](api/CloudSave/Login/TapTapProfileData.md) | token 与资料 |
| [PartialTapTapQRCodeData](api/CloudSave/Login/PartialTapTapQRCodeData.md) / [CompleteQRCodeData](api/CloudSave/Login/CompleteQRCodeData.md) | 扫码登录数据 |
| [CallbackLoginData](api/CloudSave/Login/CallbackLoginData.md) | 回调登录数据 |
| [RequestException](api/CloudSave/Login/RequestException.md) | 登录请求异常 |

## PhiLibrary197.LocalSave

| 类型 | 说明 |
| --- | --- |
| [LocalSave](api/LocalSave/LocalSave.md) | 本地存档解密 |
| [RawScore](api/LocalSave/RawScore.md) | 本地成绩解析 |
