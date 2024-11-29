
# TerminalUI (TUI)

<p align="center">
  <img src="https://github.com/user-attachments/assets/99491c72-cecf-4b06-845e-e39ae29226e7" alt="Logo" width="200" />
</p>

<p align="center">
  <img src="https://img.shields.io/badge/platform-Windows-blue?style=for-the-badge" alt="Windows Support" />
  <img src="https://img.shields.io/badge/platform-macOS-lightgrey?style=for-the-badge" alt="macOS Support" />
  <img src="https://img.shields.io/badge/platform-Linux-green?style=for-the-badge" alt="Linux Support" />
</p>

TerminalUI (TUI) is a lightweight terminal UI library built with C#, designed for creating buttons, labels, titles, and other components. It provides high customizability, simple event handling, and supports cross-platform terminal environments. It’s perfect for terminal enthusiasts who want to build efficient and aesthetic terminal interfaces.

---

# Table of Contents
### [中文版文档点我！](#中文版)
- [TerminalUI (TUI)](#terminalui-tui)
- [Features](#features)
- [Cross-Platform Demonstration](#cross-platform-demonstration)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Config](#config)
  - [TButton Parameters](#tbutton-parameters)
  - [TLabel Parameters](#tlabel-parameters)
- [Contribution](#contribution)


## Features

- **Cross-Platform**: Works seamlessly on Windows, macOS, and Linux terminal environments.
- **Rich Components**: Includes buttons (TButton), labels (TLabel), and more to build dynamic terminal UIs.
- **Highly Customizable**: Supports custom border styles, global themes, and gradient designs.
- **Simple and Intuitive**: Build functional terminal UIs with just a few lines of code.
- **Event Support**: Easily handle input events like key presses and button clicks.

---

## Cross-Platform Demonstration

| Windows | macOS | Linux |
|---------|-------|-------|
| ![Windows Demo](https://github.com/user-attachments/assets/9861a6bd-c49d-45c0-8b58-4914e0bf5a35) | ![macOS Demo](https://github.com/user-attachments/assets/cf71a87b-ce85-4cd8-bc86-32cabd8835fb) | ![Linux Demo](https://github.com/user-attachments/assets/9a85e206-84c0-40b6-b7eb-39161cdd5485) |

---

## Installation

Install TerminalUI.TUI via NuGet:
```bash
dotnet add package TerminalUI.TUI
```

## Importing the Library
After installation, import the `TerminalUI.TUI` namespace in your code:
```csharp
using TerminalUI;
```

## Project Configuration

Ensure your project is set up as a console application to support terminal output. Verify your `.csproj` file includes:
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
  </PropertyGroup>

</Project>
```
---

# Quick Start
Here’s a simple example to get started with TerminalUI and experience its core functionality:
```csharp
using TerminalUI;

var tui = new TUI(TUIType.PowerShell, 40, 20, ShowTitle: true, Title: "My First TUI");
var button = new TUI.Component.TButton
{
    X = 10,
    Y = 5,
    Height = 3,
    Width = 12,
    Text = "Click Me!",
};
button.OnClickAction = () => button.Text = "Clicked!";
tui.AddComponent(button);
tui.EnableTUI();
```
Run the above code to start a simple terminal UI with a clickable button.

---

# Config

### TButton Parameters
| Parameter      | Required | Default         | Description                          |
|----------------|----------|-----------------|--------------------------------------|
| `X`           | Yes      | None            | The horizontal position of the button (in characters). |
| `Y`           | Yes      | None            | The vertical position of the button (in characters).   |
| `Text`        | No       | `"Button"`      | The text displayed on the button.    |
| `OnClickAction` | No      | `null`          | Callback function for button clicks. |
| `BorderStyle` | No       | `null`          | Custom border style for the button.  |
| `Width`       | No       | Auto-resize     | The width of the button (overrides auto-resize if set).|
| `Height`      | No       | `1`             | The height of the button.            |

#### Example Code:
```csharp
var button = new TButton
{
    X = 10,
    Y = 5,
    Text = "Submit",
    Width = 15,
    BorderStyle = new BorderStyle('╔', '╗', '╚', '╝', '-', '|')
};
button.OnClickAction = () => Console.WriteLine("Button clicked!");
tui.AddComponent(button);
```
---

### TLabel Parameters

| Parameter      | Required | Default         | Description                          |
|----------------|----------|-----------------|--------------------------------------|
| `X`           | Yes      | None            | The horizontal position of the label (in characters). |
| `Y`           | Yes      | None            | The vertical position of the label (in characters).   |
| `Text`        | Yes      | None            | The text displayed on the label.     |
| `AutoResize`  | No       | `true`          | Automatically resize the label to fit the text.       |
| `Width`       | No       | Auto-resize     | The width of the label (overrides auto-resize if set).|
| `Height`      | No       | `1`             | The height of the label.            |

#### Example Code:
```csharp
var label = new TLabel
{
    X = 5,
    Y = 3,
    Text = "Hello, TUI!",
    AutoResize = true
};
tui.AddComponent(label);
```
---

# Contribution

We welcome contributions to **TerminalUI.TUI**! Whether it's adding new features, fixing bugs, or improving documentation, your help makes this project better.

## How to Contribute

### 1. Fork the Repository
Start by forking the repository to your GitHub account:
```bash
git clone https://github.com/your-username/TerminalUI.TUI.git
```

### 2. Create a Branch
Create a dedicated branch for your changes, following naming conventions like:
- Feature branches: `feature/your-feature-name`
- Bugfix branches: `bugfix/your-bug-description`
```bash
git checkout -b feature/your-feature-name
```

### 3. Write Your Code
- Ensure your code follows the project's coding style (refer to `.editorconfig`).
- Add clear comments, especially for complex logic.
- If fixing a bug, describe the issue and solution in your commit message.

### 4. Commit Your Changes
Before committing, review your changes and clean up debug code:
```bash
git add .
git commit -m "Brief description, e.g., [Feature] Add TInputField support"
```

### 5. Submit a Pull Request
Push your branch to your forked repository and create a Pull Request to the original repository:
```bash
git push origin feature/your-feature-name
```

Include the following in your Pull Request description:
- What changes were made.
- The motivation behind the changes (e.g., what issue was fixed or what feature was added).
- If possible, include screenshots or GIFs of the changes.

---

# 中文版


# TerminalUI (TUI)

<p align="center">
  <img src="https://github.com/user-attachments/assets/99491c72-cecf-4b06-845e-e39ae29226e7" alt="Logo" width="200" />
</p>

<p align="center">
  <img src="https://img.shields.io/badge/platform-Windows-blue?style=for-the-badge" alt="Windows Support" />
  <img src="https://img.shields.io/badge/platform-macOS-lightgrey?style=for-the-badge" alt="macOS Support" />
  <img src="https://img.shields.io/badge/platform-Linux-green?style=for-the-badge" alt="Linux Support" />
</p>

TerminalUI (TUI) 是一个基于 C# 的轻量级终端用户界面库，支持创建按钮、标签、标题等组件，具备高度的自定义能力和简单的事件处理机制。专为终端爱好者设计，帮助开发者快速构建高效、美观的终端界面。

---

# 目录
- [TerminalUI (TUI)](#terminalui-tui)
- [功能特性](#功能特性)
- [跨平台演示](#跨平台演示)
- [安装指南](#安装指南)
- [快速入门](#快速入门)
- [配置参数](#配置参数)
  - [TButton 参数表](#tbutton-参数表)
  - [TLabel 参数表](#tlabel-参数表)
- [贡献指南](#贡献指南)

## 功能特性

- **跨平台支持**：支持 Windows、macOS 和 Linux 的终端环境。
- **组件丰富**：提供按钮（TButton）、标签（TLabel）等多种功能组件。
- **高度自定义**：支持自定义边框样式、全局主题和渐变风格。
- **简单易用**：用几行代码即可创建丰富的终端界面。
- **事件支持**：轻松处理输入事件（如按键、点击）。

---

## 跨平台演示

| Windows | macOS | Linux |
|---------|-------|-------|
| ![Windows Demo](https://github.com/user-attachments/assets/9861a6bd-c49d-45c0-8b58-4914e0bf5a35) | ![macOS Demo](https://github.com/user-attachments/assets/cf71a87b-ce85-4cd8-bc86-32cabd8835fb) | ![Linux Demo](https://github.com/user-attachments/assets/9a85e206-84c0-40b6-b7eb-39161cdd5485) |

---

## 安装指南

通过 NuGet 安装 TerminalUI.TUI：
```bash
dotnet add package TerminalUI.TUI
```

## 在代码中引用
安装后，在代码文件中引用 TerminalUI.TUI 命名空间：
```csharp
using TerminalUI;
```

## 配置项目

确保你的项目设置支持终端输出（通常为控制台应用程序）。可以在 ```.csproj``` 文件中确认：

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
  </PropertyGroup>

</Project>
```

---

# 快速入门

以下是一个简单的示例，让您快速体验 TerminalUI 的核心功能：
```csharp
using TerminalUI;

var tui = new TUI(TUIType.PowerShell, 40, 20, ShowTitle: true, Title: "My First TUI");
var button = new TUI.Component.TButton
{
    X = 10,
    Y = 5,
    Height = 3,
    Width = 12,
    Text = "Click Me!",       
};
button.OnClickAction = () => button.Text = "Clicked!";
tui.AddComponent(button);
tui.EnableTUI();
```
运行以上代码即可启动一个简单的终端界面，包含一个可以点击的按钮。

---

# 配置参数

### TButton 参数表

| 参数名称       | 必须填写 | 默认值            | 描述                                   |
|----------------|----------|-------------------|----------------------------------------|
| `X`           | 是        | 无                | 按钮的水平位置（以字符为单位）。        |
| `Y`           | 是        | 无                | 按钮的垂直位置（以字符为单位）。        |
| `Text`        | 否        | `"Button"`       | 按钮上显示的文本内容。                 |
| `OnClickAction` | 否       | `null`           | 按钮点击时的回调函数（无则无响应）。    |
| `BorderStyle` | 否        | `null`           | 自定义按钮边框样式（如 `BorderStyle` 对象）。 |
| `Width`       | 否        | 根据文本自适应     | 按钮的宽度（如果指定则覆盖默认自适应）。|
| `Height`      | 否        | `1`               | 按钮的高度（默认为 1 , 但 3 为最低可视值）。|

#### 示例代码：

```csharp
var button = new TButton
{
    X = 10,
    Y = 5,
    Text = "Submit",
    Width = 15,
    BorderStyle = new BorderStyle('╔', '╗', '╚', '╝', '-', '|')
};
button.OnClickAction = () => Console.WriteLine("Button clicked!");
tui.AddComponent(button);
```

---

### TLabel 参数表

| 参数名称       | 必须填写 | 默认值            | 描述                                   |
|----------------|----------|-------------------|----------------------------------------|
| `X`           | 是        | 无                | 标签的水平位置（以字符为单位）。        |
| `Y`           | 是        | 无                | 标签的垂直位置（以字符为单位）。        |
| `Text`        | 是        | 无                | 标签上显示的文本内容。                 |
| `AutoResize`  | 否        | `true`           | 是否根据文本内容自动调整标签宽度。     |
| `Width`       | 否        | 根据文本自适应     | 标签的宽度（如果指定则覆盖 `AutoResize`）。|
| `Height`      | 否        | `1`               | 标签的高度（默认为 1 , 但 3 为最低可视值）。|

#### 示例代码：
```csharp
var label = new TLabel
{
    X = 5,
    Y = 3,
    Text = "Hello, TUI!",
    AutoResize = true
};
tui.AddComponent(label);
```

---

# 贡献指南

欢迎为 **TerminalUI.TUI** 做出贡献！无论是新增功能、修复 Bug 还是完善文档，你的参与都将帮助这个项目变得更好！

## 贡献步骤

### 1. Fork 本仓库

首先 Fork 项目到你的 GitHub 账号中：
```bash
git clone https://github.com/your-username/TerminalUI.TUI.git
```

### 2. 创建分支

为你的更改创建一个独立的分支，命名可以遵循以下格式：
- 新功能分支：`feature/your-feature-name`
- 修复 Bug 分支：`bugfix/your-bug-description`

创建分支命令：
```bash
git checkout -b feature/your-feature-name
```

### 3. 编写代码

- 请确保你的代码遵循项目的编码风格（如 `.editorconfig` 中的配置）。
- 写清楚注释，特别是复杂的逻辑。
- 如果你修复了 Bug，请在提交信息中说明问题及解决方法。

### 4. 提交代码

在提交之前，请检查更改并清理调试代码：
```bash
git add .
git commit -m "简要描述更改，例如：[Feature] 新增 TInputField 组件支持"
```

### 5. 发起 Pull Request

将更改 Push 到你的 Fork 仓库，然后在原仓库发起 Pull Request：
```bash
git push origin feature/your-feature-name
```

在发起 Pull Request 时，请在描述中包含以下内容：

 - 你更改的内容。
 - 更改的动机，例如修复了什么问题或新增了什么功能。
 - 如果可能，附上运行结果的截图或 GIF，以便维护者更直观地了解你的更改。
