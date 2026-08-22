`SongDatabase` 将 `difficulty.tsv`（定数）与 `info.tsv`（歌名/曲师/画师/谱师）按曲目 id 前缀**合并**为统一数据库，用于曲目信息查询与 RKS 计算。

## 加载

```csharp
using PhiLibrary197.CloudSave;

SongDatabase db = SongDatabase.Load("difficulty.tsv", "info.tsv");
```

两表以曲目 id 前缀为键做**并集**合并：任一表存在的曲目都收录，缺失字段为 `null`（引用类型）或空表（定数）。容错规则与 `LoadConstantTable` 一致（跳过注释/脏行，曲名自动补 `.0` 后缀）。

## 查询

```csharp
// 全部曲目（按完整 id 排序）
IReadOnlyList<SongEntry> all = db.Songs;
Console.WriteLine($"曲目总数: {db.Count}");

// 按 id 查询（完整 id 或前缀均可）
SongEntry stasis = db.GetSong("Stasis.Maozon")!;
Console.WriteLine($"{stasis.Name} | 曲师 {stasis.Composer} | 画师 {stasis.Illustrator}");

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
    float? GetConstant(Difficulty difficulty);  // 查指定难度定数，缺失返回 null
}
```

> [!NOTE]
> info.tsv 后列（画师、谱师）按列存在情况解析，老曲没有的字段为 `null`。无 AT 谱的曲目 `AtCharter` 为 `null`。

## 与 RKS 集成

```csharp
// 直接导出两个表，喂给 GetSortedListForRks
var constantMap = db.ToConstantMap();
var nameMap     = db.ToNameMap();

var (phis, other, rks) = (await save.GetSaveContextAsync(0))
    .ReadGameRecord()
    .GetSortedListForRks(constantMap, nameMap);
```

## 完整示例：按成绩展示曲目信息

```csharp
SongDatabase db = SongDatabase.Load("difficulty.tsv", "info.tsv");
var (_, other, rks) = record.GetSortedListForRks(db.ToConstantMap(), db.ToNameMap());

foreach (var s in other.Take(10))
{
    SongEntry song = db.GetSong(s.Score.Id)!;
    Console.WriteLine($"{s.NameOrDefault} — {song.Composer} — IN定数 {song.GetConstant(Difficulty.IN)} — RKS {s.Rks:F4}");
}
```
