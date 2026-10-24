曲目数据库：合并 difficulty.tsv（定数）与 info.tsv（歌名/曲师/画师/谱师）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public sealed class SongDatabase
```

## 静态字段

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `BuiltInDataVersion` | `const string` | 内置曲目数据的版本标记（当前 `2026.10.24`） |

## 静态方法

### LoadFromBuiltIn

```csharp
public static SongDatabase LoadFromBuiltIn()
```

从程序集内嵌的曲目数据加载。不读写文件，适用于 Native AOT、单文件发布和只读环境。资源缺失时抛 `InvalidOperationException`。

### LoadFromFile

```csharp
public static SongDatabase LoadFromFile(string difficultyTsvPath, string infoTsvPath)
```

加载并合并两张表。以曲目 id 前缀为键做**并集**：任一表存在的曲目都收录，缺失字段为 `null` / 空表。容错规则与 `ScoreHelper.LoadConstantTable` 一致。

### Load（已过时）

```csharp
[Obsolete("Use LoadFromFile instead.")]
public static SongDatabase Load(string difficultyTsvPath, string infoTsvPath)
```

旧入口，等价于 `LoadFromFile`。

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Songs` | `IReadOnlyList<SongEntry>` | 全部基础曲目（按完整 id 排序，不含变体） |
| `Count` | `int` | 基础曲目数量 |

## 方法

### GetSong

```csharp
public SongEntry? GetSong(string id)
```

按完整 id 查询，也接受不带 `.0` 后缀的前缀形式。带数字版本后缀的变体（如 `Random.SobremSilentroom.3`）会回退到基础曲目，返回的 `SongEntry.Id` 仍为请求的变体 id。不存在返回 `null`。

### FindByName

```csharp
public IEnumerable<SongEntry> FindByName(string name)
```

按显示名搜索（不区分大小写），返回所有匹配的基础曲目。

### ToConstantMap

```csharp
public Dictionary<ChartConstantKey, float> ToConstantMap()
```

导出定数表，供 `GameRecord.GetSortedListForRks` 使用。只包含基础曲目的定数，变体成绩在计算时会回退到基础 id。

### ToNameMap

```csharp
public Dictionary<string, string> ToNameMap()
```

导出歌名表（显示名为 `null` 的曲目不纳入，RKS 输出回退显示 id）。只包含基础曲目的名称。

## 示例

```csharp
SongDatabase db = SongDatabase.LoadFromBuiltIn();

SongEntry stasis = db.GetSong("Stasis.Maozon")!;
Console.WriteLine($"{stasis.Name} | {stasis.Composer} | IN定数 {stasis.GetConstant(Difficulty.IN)}");

// Random 变体查询
SongEntry variant = db.GetSong("Random.SobremSilentroom.3")!;
Console.WriteLine($"{variant.Id} | {variant.Name} | IN {variant.GetConstant(Difficulty.IN)}");

// RKS
var (phis, other, rks) = record.GetSortedListForRks(db.ToConstantMap(), db.ToNameMap());
```

> [!NOTE]
> Legacy/SP 难度不参与 RKS 计算，也没有定数，仅用于查询；`SongEntry.LegacyCharter` / `SpCharter` 保存对应谱师。
