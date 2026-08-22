挑战码：等级与关卡号的编码。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public struct Challenge : IPhigrosCustomSerialization<Challenge>
```

## 编码规则

挑战码 `RawCode` 由两位组成：**等级 × 100 + 关卡号**。

- `Rank` = `RawCode / 100`（[ChallengeRank](enums.md) 枚举）
- `Level` = `RawCode % 100`
- 例：`548` → Rank=5（Rainbow）、Level=48

## 构造函数

```csharp
public Challenge(ushort code)
public Challenge(short code)
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `RawCode` | `short` | 原始码（如 548、446、114、514） |
| `Rank` | `ChallengeRank` | 挑战等级（可读写，与 RawCode 联动） |
| `Level` | `byte` | 关卡号（可读写，与 RawCode 联动） |
| `HasEverDone` | `bool` | 是否做过挑战（RawCode != 0） |
