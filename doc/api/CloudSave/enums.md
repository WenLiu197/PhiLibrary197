---
title: 枚举
icon: book
---

`PhiLibrary197.CloudSave` 命名空间的全部枚举。

## Difficulty（难度）

| 值 | 名称 | 说明 |
| --- | --- | --- |
| 0 | `EZ` | 简单 |
| 1 | `HD` | 困难 |
| 2 | `IN` | 疯狂（Insane） |
| 3 | `AT` | 另一种（Another） |
| 4 | `Legacy` | 旧谱遗留（仅序列化） |
| 5 | `SP` | 特殊谱（仅序列化） |

## ScoreStatus（成绩状态）

| 值 | 名称 | 判定 |
| --- | --- | --- |
| -1 | `Bugged` | 异常成绩 |
| 0 | `NotFc` | 未 FC（仅原始解析） |
| 1 | `Fc` | Full Combo |
| 2 | `Phi` | 全 P（acc=100 且满分） |
| 3 | `Vu` | ≥ 960,000 |
| 4 | `S` | ≥ 920,000 |
| 5 | `A` | ≥ 880,000 |
| 6 | `B` | ≥ 820,000 |
| 7 | `C` | ≥ 700,000 |
| 8 | `False` | < 700,000 |

## ChallengeRank（挑战等级）

| 值 | 名称 | 平均分要求 |
| --- | --- | --- |
| 0 | `WhiteOrNone` | 白（无挑战码） |
| 1 | `Green` | ≥ 820,000 |
| 2 | `Blue` | ≥ 900,000 |
| 3 | `Red` | ≥ 950,000 |
| 4 | `Gold` | ≥ 980,000 |
| 5 | `Rainbow` | = 1,000,000 |

## GameKeyFlagType（Key 载荷类型，`[Flags]`）

| 位 | 名称 | 说明 |
| --- | --- | --- |
| 1<<0 | `HasReadCollectionPieceCount` | 已记录已读收藏数 |
| 1<<1 | `HasUnlockedSingle` | 已解锁单曲收藏 |
| 1<<2 | `HasUnlockedCollectionPieceCount` | 已记录已解锁收藏数 |
| 1<<3 | `HasUnlockedIllustration` | 已解锁插图 |
| 1<<4 | `HasUnlockedAvatar` | 已解锁头像 |

## DifficultyUnlockFlag（难度解锁，`[Flags]`）

`EZ`(1<<0) / `HD`(1<<1) / `IN`(1<<2) / `AT`(1<<3) 各一位。

## RandomVersionFlag（Random 版本，`[Flags]`）

`Normal`(0) / `R` / `A` / `N` / `D` / `O` / `M` 各一位。

## SongRecordFlag（歌曲记录，`[Flags]`）

| 位 | 名称 | 说明 |
| --- | --- | --- |
| 1<<0 | `YATMINSGrade` | You are the Miserable IN S 评价 |
| 1<<1 | `StasisINSGrade` | Stasis IN S 评价 |
| 1<<2 | `ShadowINSGrade` | Shadow IN S 评价 |
| 1<<3 | `XinZhiSuoXiangINSGrade` | 心之所向 IN S 评价 |
| 1<<4 | `InferiorINSGrade` | Inferior IN S 评价 |
| 1<<5 | `Destruction321INSGrade` | DESTRUCTION 3,2,1 IN S 评价 |
| 1<<6 | `DistortedFateINSGrade` | Distorted Fate IN S 评价 |

## Chapter8UnlockFlag（第八章，`[Flags]`）

`None`(0) / `UnlockBegin`(1<<0) / `UnlockSecondPhase`(1<<1)。

## TakumiUnlockFlag（Takumi 曲目，`[Flags]`）

| 位 | 名称 | 说明 |
| --- | --- | --- |
| 1<<0 | `CuvismINSGrade` | Cuvism IN S 评价 |
| 1<<1 | `ILArtifactINSGrade` | iL-Artifact IN S 评价 |
| 1<<2 | `ATruthSeekerINSGrade` | a truth seeker IN S 评价 |
