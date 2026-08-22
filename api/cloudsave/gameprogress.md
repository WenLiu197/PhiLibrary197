玩家的解锁进度，含链式扩展节点（Node2 → Node3 → Node4）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public class GameProgress : IPhigrosCustomSerialization<GameProgress>
```

## 构造函数

```csharp
public GameProgress(
    byte version, bool isFirstRun, bool legacyChapterFinished,
    bool alreadyShowCollectionTip, bool alreadyShowAutoUnlockINTip,
    string completed, int songUpdateInfo, Challenge challengeModeRank,
    Money money,
    DifficultyUnlockFlag unlockFlagOfSpasmodic,
    DifficultyUnlockFlag unlockFlagOfIgallta,
    DifficultyUnlockFlag unlockFlagOfRrharil,
    SongRecordFlag flagOfSongRecordKey,
    GameProgressNodeVersion2? node2)
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Version` | `byte` | 版本，最新 4 |
| `IsFirstRun` | `bool` | 是否首次运行 |
| `LegacyChapterFinished` | `bool` | 旧章节是否完成 |
| `AlreadyShowCollectionTip` | `bool` | 收藏提示是否显示过 |
| `AlreadyShowAutoUnlockINTip` | `bool` | IN 自动解锁提示是否显示过 |
| `GameCompleted` | `string` | 如 `"3.0"`（非空时解锁旧曲选择） |
| `SongUpdateInfo` | `int` | 歌曲更新信息计数 |
| `ChallengeModeRank` | `Challenge` | 挑战模式等级 |
| `Money` | `Money` | 货币 |
| `UnlockFlagOfSpasmodic` | `DifficultyUnlockFlag` | Spasmodic 难度解锁 |
| `UnlockFlagOfIgallta` | `DifficultyUnlockFlag` | Igallta 难度解锁 |
| `UnlockFlagOfRrharil` | `DifficultyUnlockFlag` | Rrharil 难度解锁 |
| `FlagOfSongRecordKey` | `SongRecordFlag` | 歌曲记录状态 |
| `Node2` | `GameProgressNodeVersion2?` | 版本 2+ 节点（可空） |

## 扩展节点

### GameProgressNodeVersion2

```csharp
public class GameProgressNodeVersion2
{
    public RandomVersionFlag RandomVersionUnlocked { get; set; }  // Random 版本解锁
    public GameProgressNodeVersion3? Node3 { get; set; }
}
```

### GameProgressNodeVersion3

```csharp
public class GameProgressNodeVersion3
{
    public Chapter8UnlockFlag Chapter8UnlockFlag { get; set; }      // 第八章进度
    public DifficultyUnlockFlag Chapter8SongUnlockFlag { get; set; } // 第八章歌曲解锁
    public GameProgressNodeVersion4? Node4 { get; set; }
}
```

### GameProgressNodeVersion4

```csharp
public class GameProgressNodeVersion4
{
    public TakumiUnlockFlag FlagOfSongRecordKeyTakumi { get; set; }  // Takumi 曲目解锁
}
```

## 示例

```csharp
GameProgress progress = ctx.ReadGameProgress();
Console.WriteLine($"货币: {progress.Money}");
Console.WriteLine($"Spasmodic 解锁: {progress.UnlockFlagOfSpasmodic}");
if (progress.Node2 is not null)
    Console.WriteLine($"Random R 版本解锁: {progress.Node2.RandomVersionUnlocked.HasFlag(RandomVersionFlag.R)}");
```
