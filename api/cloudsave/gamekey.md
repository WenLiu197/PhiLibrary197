玩家的 Key 数据：插图、头像、收藏解锁状态等，含链式扩展节点。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public class GameKey : IPhigrosCustomSerialization<GameKey>
```

## 构造函数

```csharp
public GameKey(byte version, Dictionary<string, GameKeyFlag> keys, byte lanotaReadKeys, GameKeyNodeVersion2? node2)
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Version` | `byte` | 版本，最新 1 |
| `Keys` | `Dictionary<string, GameKeyFlag>` | 键 → 旗标映射。键为歌曲名/头像名（数字后缀变体合并）/收藏 id |
| `LanotaReadKeys` | `byte` | Lanota 阅读状态（前 6 位对应 `Lanota{0}` 收藏） |
| `Node2` | `GameKeyNodeVersion2?` | 版本 2+ 节点（可空） |

## 扩展节点

### GameKeyNodeVersion2

```csharp
public class GameKeyNodeVersion2
{
    public bool CamelliaReadKey { get; set; }      // bassareusEgg 收藏是否已读
    public GameKeyNodeVersion3? Node3 { get; set; }
}
```

### GameKeyNodeVersion3

```csharp
public class GameKeyNodeVersion3
{
    public bool SideStory4BeginReadKey { get; set; }  // investigatewuxiang 收藏是否已读
    public bool OldScoreClearedV390 { get; set; }     // 3.9.0 版本 bug 成绩是否已清理
}
```

## 示例

```csharp
GameKey gameKey = ctx.ReadGameKey();
foreach (var (name, flag) in gameKey.Keys)
{
    Console.WriteLine($"{name}: {flag.Type}");
}
```
