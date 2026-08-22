---
title: 欢迎使用 PhiLibrary197！
icon: sparkle
order: 1000
---

> [!IMPORTANT] 版权声明
> 本项目是 [yt6983138/PhigrosLibraryCSharp](https://github.com/yt6983138/PhigrosLibraryCSharp) 的派生作品。
> 原项目版权归 原作者 所有，原 MIT 许可全文见 [LICENSE.MIT](https://github.com/WenLiu197/PhiLibrary197/blob/master/LICENSE.MIT)。
> 本项目的修改部分版权归 WenLiu197 所有，整体以 **GPL-3.0** 许可证发布（[LICENSE](https://github.com/WenLiu197/PhiLibrary197/blob/master/LICENSE)）。
> 本项目与原作者及原项目无任何关联，为独立维护的修改版本。

PhiLibrary197 是一个 C# 实现的 Phigros 数据访问库，支持本地存档解析、云存档访问、登录认证与 RKS 计算，并完全兼容 **Native AOT** 发布。

## 功能概览

| 功能 | 说明 | 文档 |
| --- | --- | --- |
| 📦 安装配置 | NuGet 引用、可选配置项 | [安装与配置](01-安装与配置.md) |
| 📁 本地存档 | `playerPrefsV2.xml` 解密与成绩解析 | [本地存档](02-本地存档.md) |
| ☁️ 云存档 | 存档查询、下载、解密、修改回写 | [云存档](03-云存档.md) |
| 🔑 登录认证 | TapTap 扫码 / 回调登录，换取 session token | [登录认证](04-登录认证.md) |
| 📊 数据模型 | 全部枚举、数据类、异常说明 | [数据模型](05-数据模型.md) |
| 🧮 定数表与 RKS | 定数表加载、RKS 计算、成绩排序 | [定数表与 RKS](06-定数表与RKS.md) |
| 🎵 曲目数据库 | difficulty.tsv + info.tsv 合并查询 | [SongDatabase](07-SongDatabase.md) |
| ⚡ 序列化与 AOT | 源生成 JSON、AOT 发布配置 | [序列化与 AOT](08-序列化与AOT.md) |
| 📜 许可证与发布 | 许可说明、NuGet 发布流程 | [许可证与发布](09-许可证与发布.md) |

## 快速开始

```bash
dotnet add package PhiLibrary197
```

```csharp
using PhiLibrary197.CloudSave;

// 用 Phigros 的 session token 访问云存档
Save save = new("你的25位token", isInternational: false);

// 取最新存档并读取游玩统计
SaveContext ctx = await save.GetSaveContextAsync(0);
var summary = ctx.ReadSummary();
Console.WriteLine($"挑战等级: {summary.Challenge.Rank}, 游戏内 RKS: {summary.Rks}");
```

## 项目信息

- **目标框架**：net10.0
- **当前版本**：0.2.0
- **NuGet 包**：`PhiLibrary197`
- **命名空间**：`PhiLibrary197.*`
- **源码仓库**：https://github.com/WenLiu197/PhiLibrary197
