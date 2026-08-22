---
title: Money
icon: book
---

游戏货币：KiB / MiB / GiB / TiB / PiB 五级计数。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public class Money :
    IPhigrosCustomSerialization<Money>, IEquatable<Money>,
    IEqualityOperators<Money, Money, bool>, IComparable<Money>
```

## 静态属性

```csharp
public static Money Zero { get; }   // 全零实例，每次返回新对象
```

## 构造函数

```csharp
public Money(int kiB, int miB, int giB, int tiB, int piB)
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `KiB` | `int` | KiB 计数 |
| `MiB` | `int` | MiB 计数 |
| `GiB` | `int` | GiB 计数 |
| `TiB` | `int` | TiB 计数 |
| `PiB` | `int` | PiB 计数 |

## 方法

- `ToString()`：按最高有效单位格式化（如 `"3 GiB, 5 MiB, 12 KiB"`）
- `Equals` / `==` / `!=`：按五级计数比较
- `CompareTo`：从 PiB 到 KiB 逐级比较

## 示例

```csharp
Money money = new(12, 5, 3, 0, 0);
Console.WriteLine(money.ToString());   // 3 GiB, 5 MiB, 12 KiB
```
