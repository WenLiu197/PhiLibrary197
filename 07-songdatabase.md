`SongDatabase` 将 `difficulty.tsv`（定数）与 `info.tsv`（歌名/曲师/画师/谱师）按曲目 id 前缀**合并**为统一数据库，用于曲目信息查询与 RKS 计算。

## 加载

现在有两种方式：

```csharp
using PhiLibrary197.CloudSave;

// 方式一：使用包内内置的曲目数据，不需要任何外部文件
SongDatabase db = SongDatabase.LoadFromBuiltIn();

// 方式二：使用自己的 difficulty.tsv / info.tsv
SongDatabase db2 = SongDatabase.LoadFromFile("difficulty.tsv", "info.tsv");
```

`LoadFromBuiltIn()` 的数据随程序集嵌入，不读写文件，适用于 Native AOT、单文件发布和只读环境；数据版本见 `SongDatabase.BuiltInDataVersion`。旧的 `Load(...)` 仍可用，但已标记为过时，建议改用 `LoadFromFile(...)`。

两表以曲目 id 前缀为键做**并集**合并：任一表存在的曲目都收录，缺失字段为 `null`（引用类型）或空表（定数）。容错规则与 `LoadConstantTable` 一致（跳过注释/脏行，曲名自动补 `.0` 后缀）。

## 查询

```csharp
// 全部曲目（按完整 id 排序）
IReadOnlyList<SongEntry> all = db.Songs;
Console.WriteLine($"曲目总数: {db.Count}");

// 按 id 查询（完整 id 或前缀均可）
SongEntry stasis = db.GetSong("Stasis.Maozon")!;
Console.WriteLine($"{stasis.Name} | 曲师 {stasis.Composer} | 画师 {stasis.Illustrator}");

// Random 等带数字版本后缀的变体：回退到基础曲目，但保留请求的变体 id
SongEntry variant = db.GetSong("Random.SobremSilentroom.3")!;
Console.WriteLine($"{variant.Id} | {variant.Name} | IN {variant.GetConstant(Difficulty.IN)}");

// 按显示名搜索（不区分大小写）
foreach (var hit in db.FindByName("Chronostasis"))
    Console.WriteLine(hit.Id);
```

## SongEntry

```csharp
public sealed record SongEntry(
    string Id,                       // 完整 id，如 "Glaciaxion.SunsetRay.0"
    string? Name,                    // 显示名（info 第 2 列）
    string? Composer,                // 曲师（info 第 3 列）
    string? Illustrator,             // 曲绘画师（info 第 4 列）
    string? EzCharter,               // EZ 谱师
    string? HdCharter,               // HD 谱师
    string? InCharter,               // IN 谱师
    string? AtCharter,               // AT 谱师
    IReadOnlyDictionary<Difficulty, float> Constants)  // 各难度定数
{
    string? LegacyCharter { get; init; }  // Legacy 谱师（info 第 9 列）
    string? SpCharter { get; init; }      // SP 谱师（info 第 10 列）
    float? GetConstant(Difficulty difficulty);  // 查指定难度定数，缺失返回 null
}
```

> [!NOTE]
> info.tsv 后列（画师、谱师）按列存在情况解析，老曲没有的字段为 `null`。无 AT 谱的曲目 `AtCharter` 为 `null`；Legacy/SP 不参与 RKS 计算，也没有定数，仅用于查询。

## 与 RKS 集成

```csharp
// 直接导出两个表，喂给 GetSortedListForRks
var constantMap = db.ToConstantMap();
var nameMap     = db.ToNameMap();

var (phis, other, rks) = (await save.GetSaveContextAsync(0))
    .ReadGameRecord()
    .GetSortedListForRks(constantMap, nameMap);
```

> [!NOTE]
> `GetSortedListForRks` 会对同一曲目、同一难度的数字版本变体（如 `Random.SobremSilentroom.1` ~ `.5`）去重，只保留 RKS 最高的一条，与游戏内“取最高成绩”一致。需要查看全部变体成绩时使用 `GetCompleteScores`。

## 完整示例：按成绩展示曲目信息

```csharp
SongDatabase db = SongDatabase.LoadFromBuiltIn();
var (_, other, rks) = record.GetSortedListForRks(db.ToConstantMap(), db.ToNameMap());

foreach (var s in other.Take(10))
{
    SongEntry song = db.GetSong(s.Score.Id)!;
    Console.WriteLine($"{s.NameOrDefault} — {song.Composer} — IN定数 {song.GetConstant(Difficulty.IN)} — RKS {s.Rks:F4}");
}
```
