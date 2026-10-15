<div align="center">
<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX OperationClipboard (支付宝小游戏)

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame)](https://github.com/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame)](https://github.com/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

独立游戏前后端一体化解决方案 · 独立游戏开发者的圆梦大使

<br />

[文档](https://gameframex.doc.alianblank.com) · [快速开始](#快速开始) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 目录导航

- [项目简介](#项目简介)
- [快速开始](#快速开始)
- [使用示例](#使用示例)
- [文档与资源](#文档与资源)
- [社区与支持](#社区与支持)
- [开源协议](#开源协议)

---

## 项目简介

GameFrameX OperationClipboard（支付宝小游戏）是面向支付宝小游戏平台的粘贴板适配渠道包。本包实现 `IOperationClipboardManager`（`AliPayMiniGameOperationClipboardManager`），并通过 `AliPayMiniGameOperationClipboardBootstrap` 在 `BeforeSceneLoad` 阶段自注册进主包 `OperationClipboardRegistry`（宏 `UNITY_WEBGL && ENABLE_ALIPAY_MINI_GAME && ENABLE_ALIPAY_MINI_GAME_OPERATIONCLIPBOARD` 未启用时代码被裁剪，注册不会发生；引用即注册）。粘贴板 API 仅收文本，无需任何启动期配置。

### 核心特性

- **支付宝小游戏粘贴板** - 原生 SDK 集成，回调式读写直通平台
- **自注册** - `BeforeSceneLoad` 阶段自注册进 `OperationClipboardRegistry`（引用即注册）
- **零耦合** - 主包不反向引用渠道 SDK；SDK 由目标工程自带

### 系统要求

- Unity 2019.4 或更高版本

---

## 快速开始

### 安装

选择以下任一方式：

1. 编辑 Unity 项目的 `Packages/manifest.json`，添加 `scopedRegistries` 部分：
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

   `scopes` 控制哪些包通过此注册表解析。只有以 `com.gameframex` 开头的包才会从这个注册表获取。

2. 直接在 `manifest.json` 的 `dependencies` 节点下添加以下内容：
   ```json
   {
      "com.gameframex.unity.operationclipboard.alipayminigame": "https://github.com/gameframex/com.gameframex.unity.operationclipboard.alipayminigame.git"
   }
   ```
3. 在 Unity 的 `Package Manager` 中使用 `Git URL` 的方式添加库，地址为： `https://github.com/gameframex/com.gameframex.unity.operationclipboard.alipayminigame.git`
4. 直接下载仓库放置到 Unity 项目的 `Packages` 目录下，会自动加载识别。

### 渠道注册（Bootstrap 自注册）

无需任何启动期配置。本包与支付宝小游戏 SDK（`com.alipay.alipaysdk`）一并安装后渠道自动注册，游戏侧照常使用静态门面 `OperationClipboard` 即可。

```csharp
using GameFrameX.OperationClipboard.Runtime;

// 游戏侧无需平台分支 —— 门面自动转发已注册渠道。
OperationClipboard.SetValue(text, success => { /* 结果 */ });
OperationClipboard.GetValue((success, content) => { /* 内容 */ });
```

渠道 `ChannelId` 常量值为 `"alipayminigame"`；`OperationClipboardRegistry.ResolveManager()` 按注册序解析渠道，也可通过 `GetManager("alipayminigame")` 显式获取本渠道。
## 使用示例

### 项目结构

```
Runtime/
├── AlipayMiniGame/
│   ├── AliPayMiniGameOperationClipboardManager.cs  # 支付宝小游戏 clipboard manager
│   └── AliPayMiniGameOperationClipboardBootstrap.cs  # channel bootstrap (self-registration)
└── GameFrameXOperationClipboardAlipayMiniGameCroppingHelper.cs  # code cropping helper
```

### 主要类

- **`AliPayMiniGameOperationClipboardManager`**：实现了支付宝小游戏粘贴板 SDK 的 `IOperationClipboardManager` 接口（`public const string ChannelId`）。
- **`AliPayMiniGameOperationClipboardBootstrap`**：渠道启动注册，`BeforeSceneLoad` 阶段自注册进主包 `OperationClipboardRegistry`。
- **`GameFrameXOperationClipboardAlipayMiniGameCroppingHelper`**：用于构建优化的代码裁剪辅助类。

### 注意事项

- 支付宝粘贴板 API 为 JSON 字符串回调语义；读取粘贴板仅企业主体小游戏可用。

---

## 文档与资源

- **完整文档**: [https://gameframex.doc.alianblank.com](https://gameframex.doc.alianblank.com)
- **问题反馈**: [GitHub Issues](https://github.com/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame/issues)

---

## 社区与支持

- **QQ 讨论群**: [467608841](https://qm.qq.com/cgi-bin/qm/qr?k=sYFd1nv6m2KZIWFLorZ5pBR0AE5ZhbuL&jump_from=webapi&authKey=oCu+uoL3n35fT5SEt7iLgGtROPxh31n/rHUxRlp0w1f+j38W4tKBuWyRH3KEdwHN)
- **功能建议**: [GitHub Discussions](https://github.com/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame/discussions)

---

## 依赖

| 包 | 说明 |
|----|------|
| com.gameframex.unity.operationclipboard | GameFrameX OperationClipboard 主包 |
| com.gameframex.unity | GameFrameX 核心 |
| com.alipay.alipaysdk | 目标工程自带（不在本包声明） |


## 更新日志

查看 [Releases](https://github.com/GameFrameX/com.gameframex.unity.operationclipboard.alipayminigame/releases) 了解更新日志。

## 开源协议

详见 [LICENSE.md](LICENSE.md) 文件。
