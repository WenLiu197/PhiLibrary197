复杂对象的自定义二进制序列化接口。存档二进制序列化类型共同实现的契约。

## 定义

```csharp
namespace PhiLibrary197;

public interface IPhigrosCustomSerialization<out TSelf>
```

## 实现类型

| 类型 | 说明 |
| --- | --- |
| `GameRecord` | 成绩容器 |
| `GameProgress` | 解锁进度 |
| `GameSettings` | 游戏设置 |
| `GameUserInfo` | 用户信息 |
| `GameKey` / `GameKeyFlag` | Key 数据 |
| `Summary` / `PlayCountSummary` | 游玩统计 |
| `Challenge` | 挑战码 |
| `Money` | 货币 |

## 方法

### FromReader

```csharp
static abstract TSelf FromReader(BinaryReader reader, byte objectVersion)
```

从 `BinaryReader` 构造对象。读取后 reader 应位于对象末尾。

### Serialize

```csharp
void Serialize(BinaryWriter writer, out byte objectVersion)
```

将当前对象写入 `BinaryWriter`。`objectVersion` 输出对象版本；无版本概念的对象输出 `byte.MaxValue`。

## 备注

一般不需要直接调用这些方法——通过 `SaveContext` 的 `ReadXxx` / `SaveXxx` 方法读写（见 [SaveContext](../CloudSave/SaveContext.md)）。
