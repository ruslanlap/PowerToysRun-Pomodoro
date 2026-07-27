# 🍅 PowerToys Run: Pomodoro Plugin

<div align="center">
  <img src="Assets/demo.png" alt="Pomodoro Plugin Demo" width=800">
  
  <p align="center">
    <img src="Assets/logo.png" alt="Pomodoro Icon" width="128" height="128">
  </p>
  
  <h1>⏱️ Pomodoro for PowerToys Run ⏱️</h1>
  <h3>Manage your productivity sessions directly from PowerToys Run</h3>

  
  ![PowerToys Compatible](https://img.shields.io/badge/PowerToys-Compatible-blue)
  ![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  ![Maintenance](https://img.shields.io/maintenance/yes/2026)
  [![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/9.0)
  ![Version](https://img.shields.io/github/v/release/ruslanlap/PowerToysRun-Pomodoro)
  [![CI](https://img.shields.io/github/actions/workflow/status/ruslanlap/PowerToysRun-Pomodoro/build-and-release.yml?label=CI)](https://github.com/ruslanlap/PowerToysRun-Pomodoro/actions/workflows/build-and-release.yml)
  ![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)
  [![GitHub stars](https://img.shields.io/github/stars/ruslanlap/PowerToysRun-Pomodoro)](https://github.com/ruslanlap/PowerToysRun-Pomodoro/stargazers)
  [![GitHub issues](https://img.shields.io/github/issues/ruslanlap/PowerToysRun-Pomodoro)](https://github.com/ruslanlap/PowerToysRun-Pomodoro/issues)
  [![GitHub release (latest by date)](https://img.shields.io/github/v/release/ruslanlap/PowerToysRun-Pomodoro)](https://github.com/ruslanlap/PowerToysRun-Pomodoro/releases/latest)
  [![GitHub all releases](https://img.shields.io/github/downloads/ruslanlap/PowerToysRun-Pomodoro/total)](https://github.com/ruslanlap/PowerToysRun-Pomodoro/releases)
  ![Last Commit](https://img.shields.io/github/last-commit/ruslanlap/PowerToysRun-Pomodoro)
  [![Conventional Commits](https://img.shields.io/badge/Conventional%20Commits-1.0-FE719A?logo=conventionalcommits)](https://conventionalcommits.org)
</div>

<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Pomodoro/releases/latest/download/Pomodoro-x64.zip">
    <img src="https://img.shields.io/badge/Download%20Latest%20Release-x64-blue?style=for-the-badge&logo=github" alt="Download Latest Release x64" />
  </a>
  <a href="https://github.com/ruslanlap/PowerToysRun-Pomodoro/releases/latest/download/Pomodoro-ARM64.zip">
    <img src="https://img.shields.io/badge/Download%20Latest%20Release-ARM64-blue?style=for-the-badge&logo=github" alt="Download Latest Release ARM64" />
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
- [✨ Why You'll Love Pomodoro Plugin](#-why-youll-love-pomodoro-plugin)
- [📄 License](#-license)
- [🙏 Acknowledgements](#-acknowledgements)
- [🛠️ Implementation Details](#-implementation-details)
- [🚶‍♂️ My Pomodoro Journey](#-my-pomodoro-journey)

## 📋 Overview

Pomodoro is a plugin for [Microsoft PowerToys Run](https://github.com/microsoft/PowerToys) that brings the popular Pomodoro Technique to your keyboard. Simply type `pomodoro` followed by a command like `start`, `pause`, or `status` to manage your productivity sessions directly from your PowerToys Run interface.


## ⚡ Easy Install

<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Pomodoro/releases/latest/download/Pomodoro-x64.zip">
    <img src="https://img.shields.io/badge/⬇️_DOWNLOAD-POMODORO_PLUGIN-blue?style=for-the-badge&logo=github" alt="Download Pomodoro Plugin">
  </a>
  
  <p>
    <b>Quick Installation Steps:</b><br>
    1. Download using the button above<br>
    2. Extract to <code>%LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\</code><br>
    3. Restart PowerToys<br>
    4. Start using with <code>Alt+Space</code> then type <code>pomodoro</code>
  </p>
</div>

## ✨ Features

- ⏱️ **Start, Pause, and Reset Pomodoro Sessions** - Manage your work sessions with simple commands
- 🍅 **Visual Countdown** - See time remaining in your current session
- 🔔 **End-of-Session Alerts** - Get notified when your session ends with sound or visual cues
- 🌙 **Break Management** - Automatically switch between work sessions and breaks
- ⚙️ **Configurable Session Length** - Customize work and break durations to fit your workflow
- 🪟 **Resizable & Minimizable Timer Window** - Resize the timer window with the corner grip, minimize it to the taskbar, and it remembers your preferred size and position between sessions
- 🎵 **Media Control** - Automatically play/pause media (Spotify, YouTube, etc.) when sessions start and end
- 🪝 **CLI Hooks** - Run arbitrary CLI commands on timer events (start, end, pause, resume, stop), including dedicated break start/end hooks

## 🎬 Demo Gallery

<div align="center">
  <h3>🍅 Start a Pomodoro Session</h3>
  <p><img src="Assets/demo.png" width="650" alt="Start Pomodoro Demo"/></p>
  <p><i>Simply type <code>pomodoro start</code> to begin a focused work session</i></p>
  
</div>

## 🚀 Installation

### 📋 Prerequisites

- [Microsoft PowerToys](https://github.com/microsoft/PowerToys/releases) installed
- Windows 10 or later

### 📥 Installation Steps

1. Download the latest release from the [Releases page](https://github.com/ruslanlap/PowerToysRun-Pomodoro/releases/latest)
2. Extract the ZIP file to:
   ```
   %LOCALAPPDATA%\Microsoft\PowerToys\PowerToys Run\Plugins\
   ```
3. Restart PowerToys
4. Open PowerToys Run and type `pomodoro` to access the plugin

<div align="center">
  <a href="https://github.com/ruslanlap/PowerToysRun-Pomodoro/releases/latest">
    <img src="https://img.shields.io/badge/⬇️_Download-Latest_Release-blue?style=for-the-badge&logo=github" alt="Download Latest Release">
  </a>
</div>

## 🔧 Usage

1. Open PowerToys Run (default: <kbd>Alt</kbd> + <kbd>Space</kbd>)
2. Use the following commands:

<div align="center">

| Command | Description | Example |
|---------|-------------|---------|
| `pomodoro` | Show Pomodoro instructions | `pomodoro` |
| `pomodoro start` | Start a new Pomodoro session | `pomodoro start` |
| `pomodoro pause` | Pause the current timer | `pomodoro pause` |
| `pomodoro resume` | Resume a paused timer | `pomodoro resume` |
| `pomodoro stop` | Stop and reset the timer | `pomodoro stop` |
| `pomodoro status` | Show remaining time and state | `pomodoro status` |
| `pomodoro break` | Start a short break | `pomodoro break` |
| `pomodoro longbreak` | Start a long break | `pomodoro longbreak` |

</div>

### 🎯 Quick Tips

- Press <kbd>Enter</kbd> on a command to execute it
- Right-click on a command for additional options
- Customize plugin settings in PowerToys Settings
- Long breaks automatically trigger after a configurable number of completed Pomodoros

## 📁 Data Storage

The Pomodoro plugin stores the following settings locally:

- Session length preferences (Pomodoro, short break, long break)
- Sound notification preference (enabled/disabled)
- Auto-start next session preference (enabled/disabled)
- Completed session history and statistics

All settings are stored securely in the PowerToys settings file.

## 🛠️ Building from Source

1. Clone the repository:
   ```
   git clone https://github.com/ruslanlap/PowerToysRun-Pomodoro.git
   ```

2. Open the solution in Visual Studio 2022 or later

3. Build the solution:
   ```
   dotnet build Pomodoro/Pomodoro.sln
   ```

4. Run the build-and-zip script to create installation packages:
   ```
   ./build-and-zip.sh
   ```

## 📊 Project Structure

```
Pomodoro/
├── Community.PowerToys.Run.Plugin.Pomodoro/
│   ├── Images/                  # Plugin icons and animations
│   ├── Sounds/                  # Sound effects
│   ├── Main.cs                  # Main plugin logic
│   ├── PomodoroResultWindow.xaml # Pomodoro timer window
│   ├── plugin.json             # Plugin metadata
│   └── ...
├── Services/
│   ├── TickCounterApiService.cs # Timer service
│   ├── SoundService.cs          # Sound notification service
├── Models/
│   ├── PomodoroSession.cs       # Session data model
│   ├── PomodoroSettings.cs      # Settings data model
└── Pomodoro.sln                # Solution file
```

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

### Conventional Commits & Automatic Releases

This project uses [release-please](https://github.com/googleapis/release-please) bot for automated versioning and releases. Use [Conventional Commits](https://conventionalcommits.org/) format:

| Commit type | Bump | Example |
|-------------|------|---------|
| `fix:` | Patch (1.0.**0** → 1.0.**1**) | `fix: correct timer display` |
| `feat:` | Minor (1.**0**.0 → 1.**1**.0) | `feat: add break reminders` |
| `BREAKING CHANGE:` | Major (**1**.0.0 → **2**.0.0) | `feat!: new config format` |

When you push to `master`, the bot opens a "release PR" with a changelog and version bump. Merge it to publish a new release automatically.

Please make sure to update tests as appropriate.

### Contributors

- [ruslanlap](https://github.com/ruslanlap) - Project creator and maintainer

## ❓ FAQ

<details>
<summary><b>How do I customize the Pomodoro session length?</b></summary>
<p>Open PowerToys Settings, navigate to PowerToys Run > Plugins > Pomodoro, and adjust the "Pomodoro length (minutes)" setting.</p>
</details>

<details>
<summary><b>Can I disable sound notifications?</b></summary>
<p>Yes, open PowerToys Settings, navigate to PowerToys Run > Plugins > Pomodoro, and toggle the "Play sound notifications" option.</p>
</details>

<details>
<summary><b>What happens when a Pomodoro session ends?</b></summary>
<p>By default, you'll receive a notification. If you've enabled "Auto-start next phase," the plugin will automatically start a short break after a work session, or a work session after a break.</p>
</details>

<details>
<summary><b>How many Pomodoros before a long break?</b></summary>
<p>By default, a long break occurs after 4 completed Pomodoro sessions. This can be customized in the settings.</p>
</details>

<details>
<summary><b>Can I view my productivity history?</b></summary>
<p>Yes, this feature is available in the plugin. Your completed sessions are tracked and can be viewed through the plugin interface.</p>
</details>

## 🎵 Media Control

The plugin can automatically control media playback (play/pause) when your focus sessions start and end. This works with **any** application that responds to Windows media keys — Spotify, YouTube (in your browser), Windows Media Player, foobar2000, and more.

### Enable Media Control

Open PowerToys Settings → PowerToys Run → Plugins → Pomodoro:

| Setting | Description |
|---------|-------------|
| **▶ Play media on session start** | Toggles play/pause when a **Pomodoro** (focus) session starts |
| **⏸ Pause media on session end** | Toggles play/pause when a **Pomodoro** (focus) session ends |

> **Note:** Media control only triggers for **Pomodoro** (focus) sessions, not for breaks. This way your music starts when you begin working and pauses when your focus session is over — exactly the behavior described in [issue #2](https://github.com/ruslanlap/PowerToysRun-Pomodoro/issues/2).

## 🪝 CLI Hooks (Advanced)

For power users, the plugin can run **arbitrary CLI commands** on timer lifecycle events. This enables integrations like:

- 🏠 **Smart home**: Turn lights on during breaks, dim them during focus
- 📊 **Time tracking**: Log sessions to Toggl, Clockify, or a spreadsheet
- 🔔 **Notifications**: Send a Slack/Discord/Teams message when sessions end
- 🎵 **Custom media control**: Use `nircmd` or AutoHotkey for advanced media management

### Hook Events

| Event | Trigger | Setting Key |
|-------|---------|-------------|
| **Pomodoro Start** | When a Pomodoro focus session begins | `HookOnPomodoroStart` |
| **Pomodoro End** | When a Pomodoro focus session completes | `HookOnPomodoroEnd` |
| **Break Start** | When any break (short or long) begins | `HookOnBreakStart` |
| **Break End** | When any break completes | `HookOnBreakEnd` |
| **Pause** | When the timer is paused | `HookOnPause` |
| **Resume** | When the timer is resumed | `HookOnResume` |
| **Stop** | When the timer is manually stopped | `HookOnStop` |

### Token Replacement

Command strings support the following tokens, which are replaced with event details before execution:

| Token | Description | Example value |
|-------|-------------|---------------|
| `{event}` | Event name | `start`, `end`, `pause`, `resume`, `stop` |
| `{type}` | Session type | `Pomodoro`, `Short Break`, `Long Break` |
| `{minutes}` | Session length in minutes | `25` |

### Hook Examples

#### Smart Lights (Hue CLI)

```
# On break start — turn lights on
HookOnBreakStart: hue lights on --brightness 100

# On Pomodoro start — dim lights for focus
HookOnPomodoroStart: hue lights dim --brightness 30
```

#### Time Tracking (Toggl)

```
# On Pomodoro start — start a Toggl timer
HookOnPomodoroStart: toggl start --description "Pomodoro ({minutes} min)"

# On Pomodoro end — stop the Toggl timer
HookOnPomodoroEnd: toggl stop
```

#### Desktop Notification (BurntToast PowerShell)

```
# On Pomodoro end
HookOnPomodoroEnd: powershell -Command "New-BurntToastNotification -Text 'Pomodoro Complete!', 'Time for a break.'"
```

#### Custom Sound

```
# On break start — play a custom sound
HookOnBreakStart: powershell -Command "(New-Object Media.SoundPlayer 'C:\Sounds\chime.wav').PlaySync()"
```

> **Note:** Hooks are executed via `cmd.exe /c <command>` in a hidden window, fire-and-forget (the plugin does not wait for them to complete).

## ✨ Why You'll Love Pomodoro Plugin

- **Helps Maintain Focus**: Structure your work with dedicated focus periods
- **Encourages Healthy Break Patterns**: Reminds you to take regular breaks
- **Tracks Daily Performance**: Monitor your productivity patterns
- **Keyboard-Centric**: Perfect for keyboard power users
- **Customizable**: Set your preferred session lengths and notification settings
- **Fast**: Instant access to timer controls
- **Beautiful**: Clean, modern UI that matches PowerToys style
- **Resource-Efficient**: Lightweight with minimal system impact

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgements

- [Microsoft PowerToys](https://github.com/microsoft/PowerToys) team for the amazing launcher
- All contributors who have helped improve this plugin
- The Pomodoro Technique® by Francesco Cirillo for the time management method

## 🛠️ Implementation Details

The Pomodoro plugin is built using:

- C# and .NET 9
- WPF for the UI components
- System.Timers.Timer for countdown functionality
- Windows notification API for alerts
- Task-based asynchronous pattern for non-blocking operations

The plugin implements several PowerToys Run interfaces:
- `IPlugin` - Core plugin functionality
- `IDelayedExecutionPlugin` - Support for delayed execution
- `IContextMenu` - Right-click context menu
- `IDisposable` - Resource cleanup
- `ISettingProvider` - Settings management

### Roadmap

- [x] Media control (play/pause on session start/end) — [#2](https://github.com/ruslanlap/PowerToysRun-Pomodoro/issues/2)
- [x] CLI hooks for custom integrations — [#2](https://github.com/ruslanlap/PowerToysRun-Pomodoro/issues/2)
- [ ] Custom notification sounds
- [ ] Weekly productivity analytics
- [ ] Task labeling for Pomodoro sessions

### 🚶‍♂️ My Pomodoro Journey

I created this plugin because the Pomodoro Technique transformed my own productivity. As a developer juggling multiple projects, I found myself constantly distracted and struggling to maintain focus for extended periods. That's when I discovered the power of structured work intervals.

The problem? I needed a tool that integrated seamlessly with my workflow - no separate apps to launch or browser tabs to keep open. PowerToys Run was already part of my daily routine, so building a Pomodoro plugin felt like the perfect solution.

This plugin represents hundreds of hours of focused work (ironically, managed using the Pomodoro Technique itself!). It's designed by a developer, for developers, with the features I personally needed to stay productive:

- **Zero Friction**: Two keystrokes (Alt+Space) and I'm managing my time
- **Minimal Interruption**: Notifications that don't break concentration
- **Rhythm Building**: The consistent work/break pattern helped me develop better focus habits

I hope this plugin helps you as much as the technique has helped me. Happy focusing! 🍅

With love from [ruslanlap](https://github.com/ruslanlap) 🌟