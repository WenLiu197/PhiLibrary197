# PhiLibrary197 示例

这个控制台项目引用仓库内的 `PhiLibrary197`，用于验证文档示例可运行。

```bash
dotnet run --project samples/PhiLibrary197.Samples -- help
```

| 命令 | 需要什么 | 说明 |
| --- | --- | --- |
| `builtin` | 无 | 内置曲目、变体、RKS、Tips |
| `local <path>` | `playerPrefsV2.xml` | 解密并解析本地成绩 |
| `cloud` | `PHIGROS_SESSION_TOKEN` | 云存档、Summary、B30/RKS |
| `qr` | 网络 + TapTap App | QRCode 登录 |
| `callback` | 网络 + 浏览器 | localhost 回调登录 |
| `json` | 无 | System.Text.Json 源生成示例 |

环境变量：

```text
PHIGROS_SESSION_TOKEN   云存档 session token（25 位）
PHIGROS_INTERNATIONAL   1/true 时使用国际服
PHIGROS_PREFS_PATH      本地 playerPrefsV2.xml 路径
```

示例不会把 token 写入文件；运行输出会包含 session token，请勿公开日志。
