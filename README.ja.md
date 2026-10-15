<div align="center">
<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX OperationClipboard (Alipay ミニゲーム)

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame)](https://github.com/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame)](https://github.com/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援

<br />

[ドキュメント](https://gameframex.doc.alianblank.com) · [クイックスタート](#クイックスタート) · QQグループ: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

</div>

## 目次

- [プロジェクト概要](#プロジェクト概要)
- [クイックスタート](#クイックスタート)
- [使用例](#使用例)
- [ドキュメントとリソース](#ドキュメントとリソース)
- [コミュニティとサポート](#コミュニティとサポート)
- [ライセンス](#ライセンス)

---

## プロジェクト概要

GameFrameX OperationClipboard（Alipay ミニゲーム）はAlipay ミニゲームプラットフォーム向けのクリップボード適応チャネルパッケージです。`IOperationClipboardManager`（`AliPayMiniGameOperationClipboardManager`）を実装し、`AliPayMiniGameOperationClipboardBootstrap` により `BeforeSceneLoad` ステージでメインパッケージの `OperationClipboardRegistry` に自己登録します（マクロ `UNITY_WEBGL && ENABLE_ALIPAY_MINI_GAME && ENABLE_ALIPAY_MINI_GAME_OPERATIONCLIPBOARD` が有効でない場合コードは除去され、登録は行われません。参照即登録）。クリップボードAPIはテキストのみを受け取るため、起動時の設定は不要です。

### 主な機能

- **Alipay ミニゲームクリップボード** - ネイティブSDK統合、コールバック式の読み書きをプラットフォームへ直通
- **自己登録** - `BeforeSceneLoad` ステージで `OperationClipboardRegistry` に自己登録（参照即登録）
- **ゼロ結合** - メインパッケージはチャネルSDKを逆参照しません。SDKは対象プロジェクトが提供します

### システム要件

- Unity 2019.4以上

---

## クイックスタート

### インストール

以下のいずれかの方法を選択してください：

1. Unity プロジェクトの `Packages/manifest.json` を編集し、`scopedRegistries` セクションを追加してください：
   ```json
   {
     "scopedRegistries": [
       {
         "name": "GameFrameX",
         "url": "https://gameframex.upm.alianblank.uk",
         "scopes": [
           "com.gameframex"
         ]
       }
     ],
     "dependencies": {
       "com.gameframex.unity.operationclipboard.alipayminigame": "1.0.0"
     }
   }
   ```

   `scopes` はどのパッケージがこのレジストリから解決されるかを制御します。`com.gameframex` で始まるパッケージのみこのレジストリから取得されます。

2. `manifest.json` の `dependencies` に直接以下を追加してください：
   ```json
   {
      "com.gameframex.unity.operationclipboard.alipayminigame": "https://github.com/gameframex/com.gameframex.unity.operationclipboard.alipayminigame.git"
   }
   ```
3. Unity の `Package Manager` で **Git URL** を使って追加してください。URL： `https://github.com/gameframex/com.gameframex.unity.operationclipboard.alipayminigame.git`
4. リポジトリを Unity プロジェクトの `Packages` ディレクトリに配置すると自動的に読み込まれます。

### チャネル登録（Bootstrap 自己登録）

起動時の設定は不要です。本パッケージをAlipay ミニゲーム SDK（`com.alipay.alipaysdk`）とともにインストールするとチャネルが自動登録され、ゲーム側は静的ファサード `OperationClipboard` をそのまま使用できます。

```csharp
using GameFrameX.OperationClipboard.Runtime;

// ゲーム側にプラットフォーム分岐は不要 —— ファサードが登録済みチャネルへ自動転送します。
OperationClipboard.SetValue(text, success => { /* 結果 */ });
OperationClipboard.GetValue((success, content) => { /* 内容 */ });
```

チャネルの `ChannelId` 定数は `"alipayminigame"` です。`OperationClipboardRegistry.ResolveManager()` は登録順でチャネルを解決し、`GetManager("alipayminigame")` で明示的に取得することもできます。
## 使用例

### プロジェクト構成

```
Runtime/
├── AlipayMiniGame/
│   ├── AliPayMiniGameOperationClipboardManager.cs  # Alipay ミニゲーム clipboard manager
│   └── AliPayMiniGameOperationClipboardBootstrap.cs  # channel bootstrap (self-registration)
└── GameFrameXOperationClipboardAlipayMiniGameCroppingHelper.cs  # code cropping helper
```

### 主要クラス

- **`AliPayMiniGameOperationClipboardManager`**：Alipay ミニゲームクリップボードSDK向けに `IOperationClipboardManager` を実装（`public const string ChannelId`）。
- **`AliPayMiniGameOperationClipboardBootstrap`**：チャネル起動登録。`BeforeSceneLoad` ステージでメインパッケージの `OperationClipboardRegistry` に自己登録します。
- **`GameFrameXOperationClipboardAlipayMiniGameCroppingHelper`**：ビルド最適化用のコード除去ヘルパー。

### 注意事項

- Alipay のクリップボードAPIは JSON 文字列コールバック方式です。読み取りは企業主体のミニゲームのみ利用可能です。

---

## ドキュメントとリソース

- **ドキュメント**: [https://gameframex.doc.alianblank.com](https://gameframex.doc.alianblank.com)
- **Issue トラッカー**: [GitHub Issues](https://github.com/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame/issues)

---

## コミュニティとサポート

- **QQ グループ**: [467608841](https://qm.qq.com/cgi-bin/qm/qr?k=sYFd1nv6m2KZIWFLorZ5pBR0AE5ZhbuL&jump_from=webapi&authKey=oCu+uoL3n35fT5SEt7iLgGtROPxh31n/rHUxRlp0w1f+j38W4tKBuWyRH3KEdwHN)
- **機能リクエスト**: [GitHub Discussions](https://github.com/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame/discussions)

---

## 依存関係

| パッケージ | 説明 |
|----|------|
| com.gameframex.unity.operationclipboard | GameFrameX OperationClipboard コア |
| com.gameframex.unity | GameFrameX コア |
| com.alipay.alipaysdk | 対象プロジェクトが提供 |


## 変更履歴

更新履歴は [Releases](https://github.com/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame/releases) をご参照ください。

## ライセンス

詳しくは [LICENSE.md](LICENSE.md) をご参照ください。
