---
title: GameSettings
icon: book
---

玩家的游戏设置。

## 定义

```csharp
namespace PhiLibrary197.CloudSave;

public class GameSettings : IPhigrosCustomSerialization<GameSettings>
```

## 构造函数

```csharp
public GameSettings(
    byte version, bool chordSupport, bool fcApIndicatorOn, bool enableHitSound,
    bool lowResolutionModeOn, string deviceName, float backgroundBrightness,
    float musicVolume, float effectVolume, float hitSoundVolume,
    float soundOffset, float noteScale)
```

## 属性

| 名称 | 类型 | 说明 |
| --- | --- | --- |
| `Version` | `byte` | 版本，最新 1 |
| `ChordSupport` | `bool` | [未知] |
| `FcApIndicatorOn` | `bool` | "FC/AP Indicator" 是否开启 |
| `EnableHitSound` | `bool` | 打击音是否开启 |
| `LowResolutionModeOn` | `bool` | 低分辨率模式是否开启 |
| `DeviceName` | `string` | 设备名称 |
| `BackgroundBrightness` | `float` | 背景亮度（0 ~ 1） |
| `MusicVolume` | `float` | 音乐音量（0 ~ 1） |
| `EffectVolume` | `float` | 音效音量（0 ~ 1） |
| `HitSoundVolume` | `float` | 打击音量（0 ~ 1） |
| `SoundOffset` | `float` | 声音偏移（秒） |
| `NoteScale` | `float` | 音符缩放 |
