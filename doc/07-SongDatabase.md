---
title: SongDatabase 曲目数据库
icon: database
order: 40
---

`SongDatabase` 把 `difficulty.tsv`（定数）和 `info.tsv`（歌名、曲师、画师、谱师）合并成统一的曲目数据库。库内已经嵌入一份 **327 首**的简体中文数据，开箱即用。

## 加载

### 内置数据（推荐）

```csharp
using PhiLibrary197.Songs;

SongDatabase db = SongDatabase.LoadFromBuiltIn();
Console.WriteLine(db.Count);                       // 327
Console.WriteLine(SongDatabase.BuiltInDataVersion); // 2026.10.24
```

内置数据随程序集嵌入，不读写文件，适用于 AOT、单文件和只读环境。

### 文件 / 内存 / 网络

```csharp
using PhiLibrary197.Songs;

// 文件
SongDatabase fromFile = SongDatabase.LoadFromFile("difficulty.tsv", "info.tsv");

// TextReader（例如自己从 HTTP 响应读取）
using StringReader difficulty = new(difficultyTsvText);
using StringReader info = new(infoTsvText);
SongDatabase fromReaders = SongDatabase.LoadFromFile(difficulty, info);

// Stream（HTTP 响应、压缩包条目等）
using Stream difficultyStream = await httpResponse.Content.ReadAsStreamAsync();
using Stream infoStream = await httpResponse2.Content.ReadAsStreamAsync();
SongDatabase fromStreams = SongDatabase.LoadFromFile(difficultyStream, infoStream);
```

`LoadFromFile` 的 TextReader/Stream 重载不会释放调用方传入的读取器/数据流；调用方自己 `using`。

旧方法 `Load(...)` 已标记 `[Obsolete]`，请改用 `LoadFromFile(...)`。

## 查询

```csharp
using PhiLibrary197.Scores;
using PhiLibrary197.Songs;

SongDatabase db = SongDatabase.LoadFromBuiltIn();

SongEntry? song = db.GetSong("Glaciaxion.SunsetRay");
Console.WriteLine(song?.Name);
Console.WriteLine(song?.Composer);
Console.WriteLine(song?.Illustrator);
Console.WriteLine(song?.GetConstant(Difficulty.IN));
Console.WriteLine(song?.GetCharter(Difficulty.IN));
```

`GetSong` 接受：

- 完整 id：`Glaciaxion.SunsetRay.0`
- 不带 `.0` 的前缀：`Glaciaxion.SunsetRay`
- 数字版本变体：`Random.SobremSilentroom.3`

变体查询会回退到基础条目，但保留请求的 `Id`：

```csharp
SongEntry? variant = db.GetSong("Random.SobremSilentroom.3");
Console.WriteLine(variant?.Id);        // Random.SobremSilentroom.3
Console.WriteLine(variant?.BaseId);    // Random.SobremSilentroom.0
Console.WriteLine(variant?.IsVariant); // True
```

普通曲目：

```csharp
SongEntry? normal = db.GetSong("Glaciaxion.SunsetRay");
Console.WriteLine(normal?.BaseId == normal?.Id); // True
Console.WriteLine(normal?.IsVariant);            // False
```

### 按显示名查找

```csharp
foreach (SongEntry hit in db.FindByFullName("Glaciaxion"))
    Console.WriteLine(hit.Id);
```

`FindByFullName` 是**完整显示名**、不区分大小写的匹配；不支持模糊搜索。需要模糊匹配时自己对 `db.Songs` 做 LINQ。

### 全部曲目

```csharp
IReadOnlyList<SongEntry> all = db.Songs;
```

`Songs` 只包含基础曲目（完整 `.0` id），不包含 Random 等数字变体；按完整 id 排序。

## SongEntry

```csharp
public sealed record SongEntry(
    string Id,
    string? Name,
    string? Composer,
    string? Illustrator,
    string? EzCharter,
    string? HdCharter,
    string? InCharter,
    string? AtCharter,
    IReadOnlyDictionary<Difficulty, float> Constants)
{
    public string? LegacyCharter { get; init; }
    public string? SpCharter { get; init; }

    public string BaseId { get; }
    public bool IsVariant { get; }

    public float? GetConstant(Difficulty difficulty);
    public string? GetCharter(Difficulty difficulty);
}
```

说明：

- `Name`/`Composer`/`Illustrator` 可能为 `null`，取决于 info.tsv 是否有对应列；
- `LegacyCharter`/`SpCharter` 是查询字段，Legacy/SP 没有定数；
- `GetConstant` 缺失返回 `null`；
- `GetCharter` 支持 EZ/HD/IN/AT/Legacy/SP。

## 与 RKS 集成

```csharp
using PhiLibrary197.CloudSave;
using PhiLibrary197.Scores;
using PhiLibrary197.Songs;

using Save save = new(token, isInternational: false);
SaveContext context = await save.GetSaveContextAsync(0);
GameRecord record = context.ReadGameRecord();

SongDatabase db = SongDatabase.LoadFromBuiltIn();
var (phis, sorted, rks) = record.GetSortedListForRks(
    db.ToConstantMap(),
    db.ToNameMap());

Console.WriteLine($"RKS: {rks:F4}");
```

`ToConstantMap()` / `ToNameMap()` 只导出基础曲目的数据；变体成绩计算时会自动回退到基础 id。

## 完整可运行示例

```csharp
using PhiLibrary197.Scores;
using PhiLibrary197.Songs;
using PhiLibrary197.Tips;

SongDatabase db = SongDatabase.LoadFromBuiltIn();

Console.WriteLine($"songs: {db.Count}");
Console.WriteLine($"version: {SongDatabase.BuiltInDataVersion}");

SongEntry song = db.GetSong("Glaciaxion.SunsetRay")
    ?? throw new InvalidOperationException("song not found");

Console.WriteLine($"{song.Name} / {song.Composer} / {song.Illustrator}");
Console.WriteLine($"IN: {song.GetConstant(Difficulty.IN)} by {song.GetCharter(Difficulty.IN)}");

SongEntry variant = db.GetSong("Random.SobremSilentroom.3")
    ?? throw new InvalidOperationException("variant not found");

Console.WriteLine($"{variant.Id} -> {variant.BaseId}, {variant.IsVariant}");
Console.WriteLine(TipProvider.GetTip());
```

运行：

```bash
dotnet run --project samples/PhiLibrary197.Samples -- builtin
```

## 数据来源与更新

内置数据来自 `Phigros.apk` 的 Unity 数据，提取方式、APK 哈希、数据版本和免责声明见：

- [Data/README.md](https://github.com/WenLiu197/PhiLibrary197/blob/master/PhiLibrary197/Data/README.md)

数据更新后需要同步更新 `SongDatabase.BuiltInDataVersion`。

## 常见问题

### 为什么 `GetSong` 返回 null？

检查 id 是否拼写正确。`GetSong` 不做模糊搜索；可以先用 `FindByFullName` 或 `db.Songs` 枚举确认真实 id。

### 变体为什么没有独立定数？

游戏内 Random 等变体共用基础曲目的定数和谱师；`SongDatabase` 对变体返回基础元数据，`BaseId` 指向基础 `.0` id。

### 变体会不会被 RKS 重复计算？

不会。`GetSortedListForRks` 会按基础曲目 + 难度去重，只保留 RKS 最高的一条；`GetCompleteScores` 会保留所有变体，供你自己展示。

### 数据里有 Legacy/SP 吗？

有。它们只用于查询；没有定数，不计入 RKS。
