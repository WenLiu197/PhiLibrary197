---
title: LocalSave
icon: book
---

本地存档解密（`playerPrefsV2.xml` 键值对）。

## 定义

```csharp
namespace PhiLibrary197.LocalSave;

public static class LocalSave
```

## 方法

### DecryptLocalSaveStringNew

```csharp
public static string DecryptLocalSaveStringNew(string base64EncryptedString)
public static string DecryptLocalSaveStringNew(byte[] decodedCipherText)
```

解密本地存档字符串（自定义 AES 变体，密钥/IV 内置）。

| 重载 | 说明 |
| --- | --- |
| `string` 版本 | 输入 Base64 加密字符串 |
| `byte[]` 版本 | 输入解码后的密文字节 |

## 示例

```csharp
string decrypted = LocalSave.DecryptLocalSaveStringNew(WebUtility.UrlDecode(rawValue));
// 结果形如 {"s":992580,"a":99.17,"c":1}，用 RawScore.FromJson 解析
```

> [!NOTE]
> `DecryptLocalSaveStringNew` 是本库（v5 系）方法名，上游更早版本为 `DecryptSaveString`。
> 完整流程见 [本地存档](../../02-本地存档.md)。
