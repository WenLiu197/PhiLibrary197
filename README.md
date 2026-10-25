# PhiLibrary197

PhiLibrary197 是一个 C# 实现的 Phigros 数据访问库，支持本地存档解析、云存档访问、TapTap 登录认证、成绩查询与 RKS 计算，并兼容 **Native AOT** 和 **单文件发布**。

[项目文档](https://wenliu197.github.io/PhiLibrary197/) · [示例项目](https://github.com/WenLiu197/PhiLibrary197/tree/master/samples/PhiLibrary197.Samples) · [CHANGELOG](https://github.com/WenLiu197/PhiLibrary197/blob/master/CHANGELOG.MD)

## 安装

```bash
dotnet add package PhiLibrary197
```

目标框架：`net10.0`
当前版本：`1.0.0`

## 快速开始

不需要 token，也不需要外部文件：

```csharp
using PhiLibrary197.Scores;
using PhiLibrary197.Songs;
using PhiLibrary197.Tips;

SongDatabase db = SongDatabase.LoadFromBuiltIn();

SongEntry? song = db.GetSong("Glaciaxion.SunsetRay");
Console.WriteLine($"{song?.Name} / {song?.Composer}");
Console.WriteLine($"IN 定数: {song?.GetConstant(Difficulty.IN)}");
Console.WriteLine($"IN 谱师: {song?.GetCharter(Difficulty.IN)}");

Console.WriteLine(TipProvider.GetTip());
```

运行完整示例：

```bash
dotnet run --project samples/PhiLibrary197.Samples -- builtin
```

## 功能

- 📁 **本地存档**：解密 `playerPrefsV2.xml`，解析 `RawScore`
- ☁️ **云存档**：查询、下载、解密、读取、修改并导出 zip
- 🔑 **登录认证**：TapTap QRCode 扫码、localhost 回调、手动凭证
- 🎵 **曲目数据库**：内置 327 首曲目的定数、曲师、画师、谱师
- 🧮 **RKS/B30**：定数表、成绩排序、Random 变体去重
- 💡 **内置 Tips**：191 条简体中文随机 Tips
- ⚡ **AOT/单文件**：System.Text.Json 源生成、嵌入资源、零反射

## 文档

| 文档 | 内容 |
| --- | --- |
| [安装与配置](https://github.com/WenLiu197/PhiLibrary197/blob/master/doc/01-安装与配置.md) | NuGet、代理、AES、AOT |
| [本地存档](https://github.com/WenLiu197/PhiLibrary197/blob/master/doc/02-本地存档.md) | `playerPrefsV2.xml` 解密与成绩解析 |
| [云存档](https://github.com/WenLiu197/PhiLibrary197/blob/master/doc/03-云存档.md) | 查询、下载、读取、回写 zip |
| [登录认证](https://github.com/WenLiu197/PhiLibrary197/blob/master/doc/04-登录认证.md) | QRCode、回调、session token |
| [数据模型](https://github.com/WenLiu197/PhiLibrary197/blob/master/doc/05-数据模型.md) | 全部公开模型和枚举 |
| [定数表与 RKS](https://github.com/WenLiu197/PhiLibrary197/blob/master/doc/06-定数表与RKS.md) | 定数、B30、变体去重 |
| [SongDatabase](https://github.com/WenLiu197/PhiLibrary197/blob/master/doc/07-SongDatabase.md) | 曲目查询与内置数据 |
| [序列化与 AOT](https://github.com/WenLiu197/PhiLibrary197/blob/master/doc/08-序列化与AOT.md) | 源生成、发布 |
| [API 参考](https://github.com/WenLiu197/PhiLibrary197/blob/master/doc/10-API参考.md) | 命名空间与类型速查 |

## 许可

本项目是 [yt6983138/PhigrosLibraryCSharp](https://github.com/yt6983138/PhigrosLibraryCSharp) 的派生作品。

原项目版权归原作者所有，原 MIT 许可全文见 [LICENSE.MIT](LICENSE.MIT)。本项目的修改部分版权归 WenLiu197 所有，整体以 GPL-3.0 许可证发布（[LICENSE](LICENSE)）。本项目与原作者及原项目无任何关联，为独立维护的修改版本。
