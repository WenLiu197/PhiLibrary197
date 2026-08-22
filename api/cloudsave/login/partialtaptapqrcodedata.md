TapTap 扫码登录的原始响应数据（内部中间类型）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.Login;

public class PartialTapTapQRCodeData
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Data` | `QRCodeData` | 扫码数据（JSON 键 `data`） |

## 嵌套类型 QRCodeData

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `DeviceCode` | `string`（`device_code`） | 设备码 |
| `ExpiresIn` | `int`（`expires_in`） | 过期秒数 |
| `Url` | `string`（`qrcode_url`） | 二维码 URL |
| `Interval` | `int`（`interval`） | 轮询间隔秒数 |

> [!NOTE]
> 一般不需要直接使用本类型——`TapTapHelper.RequestLoginQrCode` 已封装为 [CompleteQRCodeData](CompleteQRCodeData.md)。
