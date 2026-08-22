完整的扫码登录数据（`RequestLoginQrCode` 的返回值）。

## 定义

```csharp
namespace PhiLibrary197.CloudSave.Login;

public class CompleteQRCodeData
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `DeviceID` | `string` | 随机设备 GUID |
| `DeviceCode` | `string` | 设备码 |
| `ExpiresInSeconds` | `int` | 二维码过期秒数 |
| `Url` | `string` | 登录链接（用于生成二维码展示） |
| `Interval` | `int` | 轮询间隔秒数 |
