> [!IMPORTANT] 版权声明
> 本项目是 [yt6983138/PhigrosLibraryCSharp](https://github.com/yt6983138/PhigrosLibraryCSharp) 的派生作品。
> 原项目版权归原作者所有，原 MIT 许可全文见 [LICENSE.MIT](https://github.com/WenLiu197/PhiLibrary197/blob/master/LICENSE.MIT)。
> 本项目的修改部分版权归 WenLiu197 所有，整体以 **GPL-3.0** 许可证发布（[LICENSE](https://github.com/WenLiu197/PhiLibrary197/blob/master/LICENSE)）。
> 本项目与原作者及原项目无任何关联，为独立维护的修改版本。

PhiLibrary197 是一个 C# 实现的 Phigros 数据访问库，支持本地存档解析、云存档访问、TapTap 登录认证、成绩查询与 RKS 计算，并兼容 **Native AOT** 和 **单文件发布**。

当前版本：**1.0.0**
目标框架：**net10.0**

## 功能概览

| 能力 | 说明 | 文档 |
| --- | --- | --- |
| 📦 安装配置 | NuGet、可选 HttpClient/AES/MD5 配置 | [安装与配置](01-安装与配置.md) |
| 📁 本地存档 | 解密 `playerPrefsV2.xml` 并解析成绩 | [本地存档](02-本地存档.md) |
| ☁️ 云存档 | 查询、下载、解密、修改并导出 zip | [云存档](03-云存档.md) |
| 🔑 登录认证 | QRCode 扫码、回调 URL、手动组装凭证 | [登录认证](04-登录认证.md) |
| 📊 数据模型 | 难度、成绩状态、账号与存档模型 | [数据模型](05-数据模型.md) |
| 🧮 定数表与 RKS | 定数加载、成绩排序、B30/RKS | [定数表与 RKS](06-定数表与RKS.md) |
| 🎵 曲目数据库 | 内置 327 首曲目、变体查询、曲目信息 | [SongDatabase](07-SongDatabase.md) |
| ⚡ 序列化与 AOT | System.Text.Json 源生成、AOT 发布 | [序列化与 AOT](08-序列化与AOT.md) |
| 💡 内置 Tips | 191 条简体中文随机 Tips | [数据模型](05-数据模型.md) |
| 🧪 示例项目 | 可直接运行的完整示例 | [示例项目](11-示例项目.md) |
| 🕘 更新记录 | 版本变化与破坏性调整 | [更新记录](12-更新记录.md) |

## 30 秒快速开始

不需要 token，不需要文件，直接使用内置曲目数据：

```csharp
using PhiLibrary197.Scores;
using PhiLibrary197.Songs;
using PhiLibrary197.Tips;

SongDatabase db = SongDatabase.LoadFromBuiltIn();

SongEntry? song = db.GetSong("Glaciaxion.SunsetRay");
Console.WriteLine($"{song?.Name} / {song?.Composer}");
Console.WriteLine($"IN 定数: {song?.GetConstant(Difficulty.IN)}");
Console.WriteLine($"IN 谱师: {song?.GetCharter(Difficulty.IN)}");

// Random 变体：Id 保留变体，BaseId 指向基础曲目
SongEntry? variant = db.GetSong("Random.SobremSilentroom.3");
Console.WriteLine($"{variant?.Id} -> {variant?.BaseId}, variant={variant?.IsVariant}");

// 随机一条内置 Tip
Console.WriteLine(TipProvider.GetTip());
```

完整项目在 [samples/PhiLibrary197.Samples](https://github.com/WenLiu197/PhiLibrary197/tree/master/samples/PhiLibrary197.Samples)：

```bash
dotnet run --project samples/PhiLibrary197.Samples -- builtin
```

## 命名空间

| 命名空间 | 内容 |
| --- | --- |
| `PhiLibrary197` | `ScoreHelper`、JSON 上下文、异常 |
| `PhiLibrary197.Authentication` | TapTap/LeanCloud 登录、QRCode、回调服务器 |
| `PhiLibrary197.CloudSave` | `Save`、`SaveContext` |
| `PhiLibrary197.CloudSave.HttpModels` | 存档列表、玩家信息、文件元数据 |
| `PhiLibrary197.LocalSave` | 本地存档解密、`RawScore` |
| `PhiLibrary197.Scores` | `Difficulty`、`SongScore`、`CompleteScore`、`GameRecord`、`ChartConstantKey` |
| `PhiLibrary197.Songs` | `SongDatabase`、`SongEntry` |
| `PhiLibrary197.Tips` | `TipProvider` |

## 选一个入口

- 只想查曲目/RKS：看 [SongDatabase](07-SongDatabase.md) 和 [定数表与 RKS](06-定数表与RKS.md)。
- 有 session token：看 [云存档](03-云存档.md)。
- 还没有 token：看 [登录认证](04-登录认证.md)。
- 只有 `playerPrefsV2.xml`：看 [本地存档](02-本地存档.md)。
- 想直接跑代码：[示例项目](11-示例项目.md)。

> [!TIP]
> 文档中的完整示例都对应 `samples/PhiLibrary197.Samples` 里的真实代码，可以直接复制或运行。
