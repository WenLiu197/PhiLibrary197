Key 旗标：类型 + 打包载荷，用于存储插图/头像/收藏的解锁状态。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public struct GameKeyFlag : IPhigrosCustomSerialization<GameKeyFlag>
```

## 构造函数

```csharp
public GameKeyFlag(byte packedFlag, byte[] data)
public GameKeyFlag(byte[] data)
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Type` | `GameKeyFlagType` | 载荷类型（可多类型并存） |
| `Payload` | `ulong` | 打包载荷（用 `ReadPayload`/`WritePayload` 访问，勿直接操作） |
| `PayloadCount` | `byte` | 载荷数量（由 `Type` 置位数决定） |

## 方法

### ReadPayload

```csharp
public readonly byte ReadPayload(GameKeyFlagType position)
```

读取指定类型的载荷值。该位置无载荷抛 `ArgumentException`。

### WritePayload

```csharp
public void WritePayload(GameKeyFlagType position, byte payload)
```

写入指定类型的载荷（自动置位 `Type`）。

### RemovePayload

```csharp
public void RemovePayload(GameKeyFlagType position)
```

清除指定类型的载荷（自动清除 `Type` 位）。

> [!NOTE]
> `position` 参数必须是**单一** `GameKeyFlagType` 位（多位置或空位抛 `ArgumentException`）。

## 示例

```csharp
GameKeyFlag flag = new([0b00001, 42]);   // HasReadCollectionPieceCount = 42
byte count = flag.ReadPayload(GameKeyFlagType.HasReadCollectionPieceCount);
flag.WritePayload(GameKeyFlagType.HasUnlockedIllustration, 7);
```
