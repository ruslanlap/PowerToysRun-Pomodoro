# 🔮 PowerToys Run: Magic 8-Ball Plugin

<div align="center">
  <img src="Assets/Images/demo-magic8ball.gif" alt="Magic 8-Ball Plugin Demo" width="650">
  
  <p align="center">
    <img src="Assets/Images/logo.png" alt="Magic 8-Ball Icon" width="128" height="128">
  </p>
  
  <h1>✨ Magic 8-Ball for PowerToys Run ✨</h1>
  <h3>Ask questions and receive fortune-telling responses directly from PowerToys Run</h3>
  
  <p align="center">
    <img src="Assets/Images/image.png" alt="Magic 8-Ball Plugin Interface" width="650">
  </p>
  
  ![PowerToys Compatible](https://img.shields.io/badge/PowerToys-Compatible-blue)
  ![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  ![Maintenance](https://img.shields.io/maintenance/yes/2025)
  ![C#](https://img.shields.io/badge/C%23-.NET-512BD4)
  ![Version](https://img.shields.io/badge/version-v1.0.2-brightgreen)
  ![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)
  [![GitHub stars](https://img.shields.io/github/stars/ruslanlap/PowerToysRun-Magic8Ball)](https://github.com/ruslanlap/PowerToysRun-Magic8Ball/stargazers)
  [![GitHub issues](https://img.shields.io/github/issues/ruslanlap/PowerToysRun-Magic8Ball)](https://github.com/ruslanlap/PowerToysRun-Magic8Ball/issues)
  [![GitHub release (latest by date)](https://img.shields.io/github/v/release/ruslanlap/PowerToysRun-Magic8Ball)](https://github.com/ruslanlap/PowerToysRun-Magic8Ball/releases/latest)
  [![GitHub all releases](https://img.shields.io/github/downloads/ruslanlap/PowerToysRun-Magic8Ball/total)](https://github.com/ruslanlap/PowerToysRun-Magic8Ball/releases)
  ![Made with Love](https://img.shields.io/badge/Made%20with-❤️-red)
  ![Awesome](https://img.shields.io/badge/Awesome-Yes-orange)
</div>

<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Magic8Ball/releases/download/v1.0.2/Magic8Ball-x64.zip">
    <img src="https://img.shields.io/badge/Download%20Latest%20Release-x64-blue?style=for-the-badge&logo=github" alt="Download Latest Release" />
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Magic8Ball/releases/download/v1.0.2/Magic8Ball-ARM64.zip">
    <img src="https://img.shields.io/badge/Download%20Latest%20Release-ARM64-blue?style=for-the-badge&logo=github" alt="Download Latest Release" />
  </a>
</div>

## 📋 Table of Contents

- [📋 Overview](#-overview)
- [⚡ Easy Install](#-easy-install)
- [✨ Features](#-features)
- [🎬 Demo Gallery](#-demo-gallery)
- [🚀 Installation](#-installation)
- [🔧 Usage](#-usage)
- [📁 Data Storage](#-data-storage)
- [🛠️ Building from Source](#️-building-from-source)
- [📊 Project Structure](#-project-structure)
- [🤝 Contributing](#-contributing)
- [❓ FAQ](#-faq)
- [✨ Why You'll Love Magic 8-Ball Plugin](#-why-youll-love-magic-8-ball-plugin)
- [📄 License](#-license)
- [🙏 Acknowledgements](#-acknowledgements)
- [🛠️ Implementation Details](#-implementation-details)

## 📋 Overview

Magic 8-Ball is a plugin for [Microsoft PowerToys Run](https://github.com/microsoft/PowerToys) that brings the classic fortune-telling toy to your keyboard. Simply type `magic` followed by a yes-or-no question to receive a mystical response directly from your PowerToys Run interface.

<div align="center">
  <img src="Assets/Images/instruction.png" alt="PowerToys" width="850">
</div>

## ⚡ Easy Install

<div align="">
  <a href="https://github.com/ruslanlap/PowerToysRun-Magic8Ball/releases/download/v1.0.2/Magic8Ball-x64.zip">
    <img src="https://img.shields.io/badge/⬇️_DOWNLOAD-MAGIC_8_BALL_PLUGIN-blue?style=for-the-badge&logo=github" alt="Download Magic 8-Ball Plugin">
  </a>
  
  <p>
    <b>Quick Installation Steps:</b><br>
    1. Download using the button above<br>
    2. Extract to <code>%LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\</code><br>
    3. Restart PowerToys<br>
    4. Start using with <code>Alt+Space</code> then type <code>magic</code>
  </p>
</div>

## ✨ Features

- 🎱 **Classic Magic 8-Ball Experience** - Ask yes-or-no questions and receive fortune-telling responses
- 🧠 **Smart Biased Responses** - Optional sentiment analysis to provide more relevant answers
- 🎬 **Animated Shaking Effect** - Watch the Magic 8-Ball shake before revealing your answer
- 🔊 **Sound Effects** - Hear the Magic 8-Ball shake when asking a question
- 🖼️ **Visual Magic 8-Ball Window** - See your response appear in a dedicated window with a visual Magic 8-Ball
- 😀 **Response Type Indicators** - Responses are categorized with emojis as positive, negative, or neutral
- ⚙️ **Customizable Settings** - Enable or disable animations, sound effects, and biased responses
- 🌓 **Theme Support** - Works with both light and dark PowerToys themes
- 🔄 **Ask Again** - Easily ask the same question again for a different response
- 🎨 **Beautiful UI** - Elegant popup window with modern design that works like a mini fortune-telling toy

## 🎬 Demo Gallery

<div align="center">
  <h3>🎱 Ask a Question</h3>
  <p><img src="Assets/Images/demo-magic8ball.gif" width="650" alt="Ask Question Demo"/></p>
  <p><i>Simply type <code>magic</code> followed by your yes-or-no question</i></p>
  
</div>

## 🚀 Installation

### 📋 Prerequisites

- [Microsoft PowerToys](https://github.com/microsoft/PowerToys/releases) installed
- Windows 10 or later

### 📥 Installation Steps

1. Download the latest release from the [Releases page](https://github.com/ruslanlap/PowerToysRun-Magic8Ball/releases/latest)
2. Extract the ZIP file to:
   ```
   %LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\
   ```
3. Restart PowerToys
4. Open PowerToys Run and type `magic` to access the plugin

<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Magic8Ball/releases/latest">
    <img src="https://img.shields.io/badge/⬇️_Download-Latest_Release-blue?style=for-the-badge&logo=github" alt="Download Latest Release">
  </a>
</div>

## 🔧 Usage

1. Open PowerToys Run (default: <kbd>Alt</kbd> + <kbd>Space</kbd>)
2. Use the following commands:

<div align="center">

| Command | Description | Example |
|---------|-------------|---------|
| `magic` | Show Magic 8-Ball instructions | `magic` |
| `magic <question>` | Ask a yes-or-no question | `magic Will I win the lottery?` |

</div>

### 🎯 Quick Tips

- Press <kbd>Enter</kbd> on a question to get a random response
- Press <kbd>Ctrl</kbd> + <kbd>Enter</kbd> to get a biased response based on your question
- Right-click on a question for additional options
- Click "Ask Again" in the Magic 8-Ball window to get another response
- Customize plugin settings in PowerToys Settings

## 📁 Data Storage

The Magic 8-Ball plugin stores the following settings locally:

- Animation preference (enabled/disabled)
- Sound effects preference (enabled/disabled)
- Biased responses preference (enabled/disabled)

All settings are stored securely in the PowerToys settings file.

## 🛠️ Building from Source

1. Clone the repository:
   ```
   git clone https://github.com/ruslanlap/PowerToysRun-Magic8Ball.git
   ```

2. Open the solution in Visual Studio 2022 or later

3. Build the solution:
   ```
   dotnet build Magic8Ball/Magic8Ball.sln
   ```

4. Run the build-and-zip script to create installation packages:
   ```
   ./build-and-zip.sh
   ```

## 📊 Project Structure

```
Magic8Ball/
├── Community.PowerToys.Run.Plugin.Magic8Ball/
│   ├── Images/                  # Plugin icons and animations
│   ├── Sounds/                  # Sound effects
│   ├── Main.cs                  # Main plugin logic
│   ├── Magic8BallResultWindow.xaml # Magic 8-Ball window
│   ├── plugin.json             # Plugin metadata
│   └── ...
├── Services/
│   ├── EightBallApiService.cs   # API service for responses
├── Models/
│   ├── EightBallResponse.cs     # Response data model
└── Magic8Ball.sln              # Solution file
```

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

Please make sure to update tests as appropriate.

### Contributors

- [ruslanlap](https://github.com/ruslanlap) - Project creator and maintainer

## ❓ FAQ

<details>
<summary><b>Does the plugin require internet access?</b></summary>
<p>The plugin works best with internet access to fetch responses from the 8Ball API, but it also has a built-in fallback system that works offline.</p>
</details>

<details>
<summary><b>How do I enable or disable animations?</b></summary>
<p>Open PowerToys Settings, navigate to PowerToys Run > Plugins > Magic 8-Ball, and toggle the "Enable animations" option.</p>
</details>

<details>
<summary><b>What's the difference between regular and biased responses?</b></summary>
<p>Regular responses are completely random. Biased responses analyze your question to provide more contextually relevant answers based on the sentiment of your question.</p>
</details>

<details>
<summary><b>Can I customize the responses?</b></summary>
<p>Not in the current version, but this is planned for a future update.</p>
</details>

<details>
<summary><b>Why does the plugin need sound permissions?</b></summary>
<p>The plugin uses sound effects to enhance the Magic 8-Ball experience. You can disable these in the settings if preferred.</p>
</details>

## ✨ Why You'll Love Magic 8-Ball Plugin

- **Fun Distraction**: Take a break and consult the Magic 8-Ball for life's important (or not so important) decisions
- **Keyboard-Centric**: Perfect for keyboard power users
- **Customizable**: Set your preferred animation and sound settings
- **Fast**: Instant access to fortune-telling wisdom
- **Beautiful**: Clean, modern UI that matches PowerToys style
- **Resource-Efficient**: Lightweight with minimal system impact
- **Nostalgic**: Brings back the classic Magic 8-Ball experience in a modern way

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgements

- [Microsoft PowerToys](https://github.com/microsoft/PowerToys) team for the amazing launcher
- [![EightBallAPI](https://img.shields.io/badge/Powered%20by-EightBallAPI-9cf)](https://www.eightballapi.com/) for providing the fortune-telling API
- All contributors who have helped improve this plugin

## 🛠️ Implementation Details

The Magic 8-Ball plugin is built using:

- C# and .NET
- WPF for the UI components
- HttpClient for API communication
- System.Text.Json for JSON parsing
- Task-based asynchronous pattern for non-blocking operations

The plugin implements several PowerToys Run interfaces:
- `IPlugin` - Core plugin functionality
- `IDelayedExecutionPlugin` - Support for delayed execution
- `IContextMenu` - Right-click context menu
- `IDisposable` - Resource cleanup
- `ISettingProvider` - Settings management

### Roadmap

- [ ] Custom response sets
- [ ] Multiple language support
- [ ] More animation options
- [ ] History of past questions and answers
- [ ] Sharing responses with friends
- [ ] Advanced question analysis
- [ ] Voice input for questions
- [ ] Voice output for responses
- [ ] Custom themes for Magic 8-Ball window
# PowerToysRun-Pomodoro
