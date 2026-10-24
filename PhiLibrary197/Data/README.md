# 内置数据说明

## 宇宙免责声明

> 本目录下的 `info.tsv`、`difficulty.tsv` 与 `tips.tsv` 是从 Phigros 游戏安装包中还原的**曲目与 Tips 文本数据**，仅用于存档解析、曲目查询与 RKS 计算等互操作用途。
>
> - Phigros 游戏本体、曲目、曲绘、音频及相关数据的版权归 **Pigeon Games（鸽游）** 及各自权利人所有。
> - 本仓库的**代码**以 GPL-3.0 发布；本目录的数据**不包含**游戏图片、音频、谱面等资源，仅包含曲名、曲师、画师、谱师、定数、Tips 等文本信息。
> - 本项目与 Pigeon Games 无任何隶属或合作关系。
> - 若权利人认为本目录数据不宜随代码分发，请通过仓库 Issue 联系，我们会在后续版本移除或调整。
>
> 简单说：代码是 GPL 的，曲目数据是鸽游的；我们只是把“查曲目/RKS 需要的那几列”放进包里，不包含游戏资源。

## 版本来源

| 项目 | 值 |
| --- | --- |
| 来源 APK | `Phigros.apk` |
| APK 大小 | `3333003433` 字节 |
| APK SHA-256 | `F59A041F5BC02D0F3EB51A67739EC0271590AC412E68CF6CC6B43621CDF08985` |
| Unity 版本 | `2022.3.62f2` |
| 提取日期 | `2026-10-24` |
| 曲目数量 | `327` |
| Tips 数量（简中） | `191` |
| 数据版本 | `2026.10.24`（见 `SongDatabase.BuiltInDataVersion`） |

> 其中 `WhatdoyouwantmorethanaHappyending...` 的 EZ 定数为 `0.5`，低于普通曲目的最低定数；已按原值保留，解析时不能假设所有定数都 `>= 1.0`。

## 提取方式

游戏没有以 TSV 形式直接存放这些表；曲目数据来自 `assets/bin/Data/data.unity3d` 中 `path_id = 50` 的 `SongInfo` MonoBehaviour 表，Tips 来自 `path_id = 52` 的 `TipsProvider`。解析脚本与中间结果保存在本地 `PhiApk/` 工作目录（不纳入 Git）。

## 文件格式

### info.tsv

每行：

```text
曲目id前缀	显示名	曲师	画师	EZ谱师	HD谱师	IN谱师	[AT谱师]	[Legacy谱师]	[SP谱师]
```

- 第一列不带 `.0` 后缀；`SongDatabase` 会补 `.0`。
- 无 AT 谱的曲目省略 AT 列；无 Legacy/SP 谱的曲目省略后续列。
- 列数可能为 7、8、9 或 10，与社区表格式兼容。

### tips.tsv

每行：

```text
index	text
```

- 简体中文，表头 `index / text`，0 基序号。
- 共 191 条；`TipProvider.GetTip()` 用 `Random.Shared.Next` 随机取一条。

### difficulty.tsv

每行：

```text
曲目id前缀	EZ	HD	IN	[AT]
```

- 只包含 EZ/HD/IN/AT 定数；Legacy/SP 没有定数，不参与 RKS 计算。
- 无 AT 谱的曲目省略 AT 列。

## 更新策略

- 内置数据随 NuGet 包版本发布，更新游戏版本时重新生成并更新 `BuiltInDataVersion`。
- 调用方仍可用 `SongDatabase.LoadFromFile(difficultyTsvPath, infoTsvPath)` 提供自己的表，优先级由调用方决定。
- 与旧社区表的差异（例如 48 首定数变化）以本目录的最新 APK 数据为准。
