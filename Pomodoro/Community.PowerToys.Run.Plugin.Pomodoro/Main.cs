using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Community.PowerToys.Run.Plugin.Pomodoro.Models;
using Community.PowerToys.Run.Plugin.Pomodoro.Services;
using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using Wox.Plugin;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.Pomodoro
{
    /// <summary>
    /// Main class of the Pomodoro plugin that implements all required interfaces.
    /// </summary>
    public class Main : IPlugin, IDelayedExecutionPlugin, IContextMenu, ISettingProvider, IDisposable
    {
        /// <summary>
        /// ID of the plugin.
        /// </summary>
        public static string PluginID => "6884550EBA0A4A82B090AA19C01F9B38";

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name => "Pomodoro";
        public string Id => PluginID;

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description => "Start and manage Pomodoro technique timers";

        // New properties for enhanced features
        private SoundService? SoundService { get; set; }
        public bool PlaySounds { get; private set; } = true;
        public bool AutoStartNextSession { get; private set; } = false;
        public int PomodorosBeforeLongBreak { get; private set; } = 4;
        private int CurrentPomodoroCount { get; set; } = 0;

        /// <summary>
        /// Exposes additional plugin settings in the PowerToys Settings UI.
        /// </summary>
        public IEnumerable<PluginAdditionalOption> AdditionalOptions => new List<PluginAdditionalOption>
        {
            new PluginAdditionalOption
            {
                Key = nameof(ShowNotifications),
                DisplayLabel = "Show notifications",
                DisplayDescription = "Show notifications when timers complete",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Checkbox,
                Value = ShowNotifications
            },
            new PluginAdditionalOption
            {
                Key = nameof(PlaySounds),
                DisplayLabel = "Play sound notifications",
                DisplayDescription = "Play sounds at the beginning and end of phases",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Checkbox,
                Value = PlaySounds
            },
            new PluginAdditionalOption
            {
                Key = nameof(AutoStartNextSession),
                DisplayLabel = "Auto-start next phase",
                DisplayDescription = "Automatically start the next phase when current one ends",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Checkbox,
                Value = AutoStartNextSession
            },
            new PluginAdditionalOption
            {
                Key = nameof(PomodoroLength),
                DisplayLabel = "Pomodoro length (minutes)",
                DisplayDescription = "Set the default length of a Pomodoro session",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                TextValue = PomodoroLength.ToString()
            },
            new PluginAdditionalOption
            {
                Key = nameof(ShortBreakLength),
                DisplayLabel = "Short break length (minutes)",
                DisplayDescription = "Set the default length of a short break",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                TextValue = ShortBreakLength.ToString()
            },
            new PluginAdditionalOption
            {
                Key = nameof(LongBreakLength),
                DisplayLabel = "Long break length (minutes)",
                DisplayDescription = "Set the default length of a long break",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                TextValue = LongBreakLength.ToString()
            },
            new PluginAdditionalOption
            {
                Key = nameof(PomodorosBeforeLongBreak),
                DisplayLabel = "Work phases before long break",
                DisplayDescription = "Number of work phases before taking a long break",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                TextValue = PomodorosBeforeLongBreak.ToString()
            }
        };

        // Defaults (will be overridden by user settings if present)
        public bool ShowNotifications { get; private set; } = true;
        public int PomodoroLength { get; private set; } = 25;
        public int ShortBreakLength { get; private set; } = 5;
        public int LongBreakLength { get; private set; } = 15;

        private PluginInitContext? Context { get; set; }
        private string? IconPath { get; set; }
        private TickCounterApiService? ApiService { get; set; }
        private PomodoroSession? CurrentSession { get; set; }

        private bool Disposed { get; set; }

        private readonly Dictionary<string, Func<string, bool>> _commands = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initialize the plugin with the given <see cref="PluginInitContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="PluginInitContext"/> for this plugin.</param>
        public void Init(PluginInitContext context)
        {
            Log.Info("Init", GetType());
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(Context.API.GetCurrentTheme());

            // Initialize the sound service
            SoundService = new SoundService(GetType());

            // Initialize API service
            ApiService = new TickCounterApiService(GetType());

            // Initialize commands
            _commands.Add("start", StartPomodoro);
            _commands.Add("break", StartBreak);
            _commands.Add("longbreak", StartLongBreak);
            _commands.Add("pause", PauseTimer);
            _commands.Add("resume", ResumeTimer);
            _commands.Add("stop", StopTimer);
            _commands.Add("status", ShowStatus);
        }

        /// <summary>
        /// Return a filtered list, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query)
        {
            Log.Info("Query: " + query.Search, GetType());

            if (string.IsNullOrWhiteSpace(query.Search))
            {
                return new List<Result>
                {
                    new Result
                    {
                        QueryTextDisplay = string.Empty,
                        IcoPath = IconPath,
                        Title = "Pomodoro Timer",
                        SubTitle = "Type a command (start, break, pause, resume, stop, status)",
                        Score = 100,
                    }
                };
            }

            var args = query.Search.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var command = args.FirstOrDefault()?.ToLowerInvariant() ?? string.Empty;
            var parameters = args.Length > 1 ? string.Join(' ', args.Skip(1)) : string.Empty;

            if (_commands.TryGetValue(command, out var action))
            {
                string subtitle = command switch
                {
                    "start"     => "Start a new Pomodoro session",
                    "break"     => "Start a short break",
                    "longbreak" => "Start a long break",
                    "pause"     => "Pause the current timer",
                    "resume"    => "Resume the paused timer",
                    "stop"      => "Stop the current timer",
                    "status"    => "Show the current timer status",
                    _           => "Execute command"
                };

                return new List<Result>
                {
                    new Result
                    {
                        QueryTextDisplay = query.Search,
                        IcoPath = IconPath,
                        Title = $"Pomodoro: {command}",
                        SubTitle = subtitle,
                        Score = 100,
                        Action = _ => action(parameters),
                        ContextData = query.Search,
                    }
                };
            }

            // If it's not a known command, suggest available commands
            return new List<Result>
            {
                new Result
                {
                    QueryTextDisplay = query.Search,
                    IcoPath = IconPath,
                    Title = "Unknown Pomodoro command",
                    SubTitle = "Try: start, break, pause, resume, stop, status",
                    Score = 50,
                }
            };
        }

        /// <summary>
        /// Return a filtered list for delayed execution, based on the given query.
        /// </summary>
        /// <param name="query">The query to filter the list.</param>
        /// <param name="delayedExecution">Indicates if this is a delayed execution.</param>
        /// <returns>A filtered list, can be empty when nothing was found.</returns>
        public List<Result> Query(Query query, bool delayedExecution)
        {
            if (delayedExecution && !string.IsNullOrWhiteSpace(query.Search))
            {
                return Query(query);
            }

            return new List<Result>();
        }

        /// <summary>
        /// Return a list context menu entries for a given <see cref="Result"/> (shown at the right side of the result).
        /// </summary>
        /// <param name="selectedResult">The <see cref="Result"/> for the list with context menu entries.</param>
        /// <returns>A list context menu entries.</returns>
        public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
        {
            // Return an empty list or other non-dialog options
            return new List<ContextMenuResult>();
        }

        /// <summary>
        /// Creates setting panel (Optional: if you do not want a custom panel, just return a blank panel).
        /// </summary>
        /// <returns>The control.</returns>
        public Control CreateSettingPanel()
        {
            var panel = new StackPanel
            {
                Margin = new Thickness(10)
            };

            panel.Children.Add(new TextBlock
            {
                Text = "No custom settings UI. Please use the built-in Additional Options in PowerToys."
            });

            var contentControl = new ContentControl
            {
                Content = panel
            };

            return contentControl;
        }

        /// <summary>
        /// Handle timer completion and potentially start the next phase
        /// </summary>
        /// <param name="session">The completed session</param>
        public void OnTimerCompleted(PomodoroSession session)
        {
            try
            {
                // Play sound immediately when timer completes
                if (PlaySounds)
                {
                    SoundService?.PlaySound("endphase");
                }

                // Show a non-modal notification if enabled
                if (ShowNotifications && Context != null)
                {
                    Context.API.ShowMsg($"{session.Type} Completed", 
                        $"Your {session.Type.ToLower()} timer has completed!", 
                        IconPath);
                }

                if (AutoStartNextSession)
                {
                    // Determine next phase
                    if (session.Type == "Pomodoro")
                    {
                        // Increment pomodoro count
                        CurrentPomodoroCount++;

                        // Check if it's time for a long break
                        if (CurrentPomodoroCount >= PomodorosBeforeLongBreak)
                        {
                            CurrentPomodoroCount = 0;
                            StartLongBreak(string.Empty);
                        }
                        else
                        {
                            StartBreak(string.Empty);
                        }
                    }
                    else
                    {
                        // After any break, start a new pomodoro
                        StartPomodoro(string.Empty);
                    }
                }
                else
                {
                    // Clean up if not auto-starting
                    CurrentSession = null;
                }
            }
            catch (Exception ex)
            {
                Log.Exception("Error handling timer completion", ex, GetType());
            }
        }

        /// <summary>
        /// Play a sound to indicate the start of a new phase
        /// </summary>
        private void PlayStartSound()
        {
            if (PlaySounds)
            {
                SoundService?.PlaySound("startnewphase");
            }
        }

        /// <summary>
        /// Updates settings from the PowerToys Settings UI.
        /// </summary>
        /// <param name="settings">The plugin settings.</param>
        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
            Log.Error($"UpdateSettings called! PomodoroLength in settings: {settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(PomodoroLength))?.TextValue}", GetType());

            // Створюємо новий екземпляр налаштувань
            var pomodoroSettings = new PomodoroSettings();

            // Оновлення ShowNotifications
            pomodoroSettings.ShowNotifications = settings.AdditionalOptions
                .SingleOrDefault(x => x.Key == nameof(ShowNotifications))?.Value ?? pomodoroSettings.ShowNotifications;

            // Оновлення довжини помодоро - FIX: proper syntax for conditional
            var pomodoroOption = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(PomodoroLength));
            Log.Error($"PomodoroOption: {pomodoroOption?.TextValue ?? "null"}", GetType());

            if (pomodoroOption != null &&
                !string.IsNullOrWhiteSpace(pomodoroOption.TextValue) &&
                int.TryParse(pomodoroOption.TextValue, out var pomodoroLength) &&
                pomodoroLength > 0)
            {
                pomodoroSettings.PomodoroLength = pomodoroLength;
            }

            // Оновлення короткої перерви
            if (settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(ShortBreakLength)) is var shortBreakOption &&
                shortBreakOption != null &&
                !string.IsNullOrWhiteSpace(shortBreakOption.TextValue) &&
                int.TryParse(shortBreakOption.TextValue, out var shortBreakLength) &&
                shortBreakLength > 0)
            {
                pomodoroSettings.ShortBreakLength = shortBreakLength;
            }

            // Оновлення довгої перерви
            if (settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(LongBreakLength)) is var longBreakOption &&
                longBreakOption != null &&
                !string.IsNullOrWhiteSpace(longBreakOption.TextValue) &&
                int.TryParse(longBreakOption.TextValue, out var longBreakLength) &&
                longBreakLength > 0)
            {
                pomodoroSettings.LongBreakLength = longBreakLength;
            }

            // Sound settings
            pomodoroSettings.PlaySounds = settings.AdditionalOptions
                .SingleOrDefault(x => x.Key == nameof(PlaySounds))?.Value ?? pomodoroSettings.PlaySounds;

            // Auto-start settings
            pomodoroSettings.AutoStartNextSession = settings.AdditionalOptions
                .SingleOrDefault(x => x.Key == nameof(AutoStartNextSession))?.Value ?? pomodoroSettings.AutoStartNextSession;

            // Pomodoros before long break
            if (settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(PomodorosBeforeLongBreak)) is var pomodorosOption &&
                pomodorosOption != null &&
                !string.IsNullOrWhiteSpace(pomodorosOption.TextValue) &&
                int.TryParse(pomodorosOption.TextValue, out var pomodorosBeforeLongBreak) &&
                pomodorosBeforeLongBreak > 0)
            {
                pomodoroSettings.PomodorosBeforeLongBreak = pomodorosBeforeLongBreak;
            }

            // Тепер оновлюємо поля в Main, щоб використовувати нові значення
            ShowNotifications = pomodoroSettings.ShowNotifications;
            PomodoroLength = pomodoroSettings.PomodoroLength;
            ShortBreakLength = pomodoroSettings.ShortBreakLength;
            LongBreakLength = pomodoroSettings.LongBreakLength;
            PlaySounds = pomodoroSettings.PlaySounds;
            AutoStartNextSession = pomodoroSettings.AutoStartNextSession;
            PomodorosBeforeLongBreak = pomodoroSettings.PomodorosBeforeLongBreak;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Log.Info("Dispose", GetType());

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Wrapper method for <see cref="Dispose()"/> that disposes additional objects and events from the plugin itself.
        /// </summary>
        /// <param name="disposing">Indicate that the plugin is disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed || !disposing)
            {
                return;
            }

            if (Context?.API != null)
            {
                Context.API.ThemeChanged -= OnThemeChanged;
            }

            ApiService?.Dispose();
            SoundService?.Dispose();
            CurrentSession = null;

            Disposed = true;
        }

        private void UpdateIconPath(Theme theme)
        {
            if (Context is null) return;

            IconPath = theme == Theme.Light || theme == Theme.HighContrastWhite
                ? Context.CurrentPluginMetadata.IcoPathLight
                : Context.CurrentPluginMetadata.IcoPathDark;
        }

        private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);

        private bool StartPomodoro(string parameters)
        {
            Log.Error($"StartPomodoro called with current PomodoroLength: {PomodoroLength}", GetType());
            if (Context == null || ApiService == null)
            {
                Log.Error("Context or ApiService is null", GetType());
                return false;
            }

            try
            {
                // Play start sound immediately
                PlayStartSound();

                // Use the *current* PomodoroLength from settings (default or user-set).
                int length = PomodoroLength;

                // If the user typed e.g. "pomodoro start 2", override the default:
                if (!string.IsNullOrEmpty(parameters) && int.TryParse(parameters, out var customLength) && customLength > 0)
                {
                    length = customLength;
                }

                // Show notification without requiring user interaction
                ShowNonModalNotification("Starting Pomodoro", $"{length} minute Pomodoro session");

                Task.Run(async () =>
                {
                    try
                    {
                        var response = await ApiService.CreateTimerAsync("Pomodoro", length * 60);
                        if (response != null)
                        {
                            CurrentSession = new PomodoroSession
                            {
                                TimerId = response.TimerId,
                                Type = "Pomodoro",
                                LengthMinutes = length,
                                StartTime = DateTime.Now,
                                EndTime = DateTime.Now.AddMinutes(length)
                            };

                            ShowPomodoroWindow(CurrentSession);
                        }
                        else
                        {
                            Context.API.ShowMsg("Pomodoro Error", "Failed to create timer", IconPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Exception("Error starting Pomodoro", ex, GetType());
                        Context.API.ShowMsg("Pomodoro Error", "Error: " + ex.Message, IconPath);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                Log.Exception("Error in StartPomodoro", ex, GetType());
                Context.API.ShowMsg("Pomodoro Error", "Error: " + ex.Message, IconPath);
                return false;
            }
        }

        private bool StartBreak(string parameters)
        {
            if (Context == null || ApiService == null)
            {
                Log.Error("Context or ApiService is null", GetType());
                return false;
            }

            try
            {
                // Play start sound
                PlayStartSound();

                int length = ShortBreakLength; // Use the current value from settings
                if (!string.IsNullOrEmpty(parameters) && int.TryParse(parameters, out var customLength) && customLength > 0)
                {
                    length = customLength;
                }

                // Show notification without requiring user interaction
                ShowNonModalNotification("Starting Break", $"{length} minute short break");

                Task.Run(async () =>
                {
                    try
                    {
                        var response = await ApiService.CreateTimerAsync("Short Break", length * 60);
                        if (response != null)
                        {
                            CurrentSession = new PomodoroSession
                            {
                                TimerId = response.TimerId,
                                Type = "Short Break",
                                LengthMinutes = length,
                                StartTime = DateTime.Now,
                                EndTime = DateTime.Now.AddMinutes(length)
                            };

                            ShowPomodoroWindow(CurrentSession);
                        }
                        else
                        {
                            Context.API.ShowMsg("Pomodoro Error", "Failed to create timer", IconPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Exception("Error starting break", ex, GetType());
                        Context.API.ShowMsg("Pomodoro Error", "Error: " + ex.Message, IconPath);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                Log.Exception("Error in StartBreak", ex, GetType());
                Context.API.ShowMsg("Pomodoro Error", "Error: " + ex.Message, IconPath);
                return false;
            }
        }

        private bool StartLongBreak(string parameters)
        {
            if (Context == null || ApiService == null)
            {
                Log.Error("Context or ApiService is null", GetType());
                return false;
            }

            try
            {
                // Play start sound
                PlayStartSound();

                int length = LongBreakLength; // Use the current value from settings
                if (!string.IsNullOrEmpty(parameters) && int.TryParse(parameters, out var customLength) && customLength > 0)
                {
                    length = customLength;
                }

                // Show notification without requiring user interaction
                ShowNonModalNotification("Starting Long Break", $"{length} minute long break");

                Task.Run(async () =>
                {
                    try
                    {
                        var response = await ApiService.CreateTimerAsync("Long Break", length * 60);
                        if (response != null)
                        {
                            CurrentSession = new PomodoroSession
                            {
                                TimerId = response.TimerId,
                                Type = "Long Break",
                                LengthMinutes = length,
                                StartTime = DateTime.Now,
                                EndTime = DateTime.Now.AddMinutes(length)
                            };

                            ShowPomodoroWindow(CurrentSession);
                        }
                        else
                        {
                            Context.API.ShowMsg("Pomodoro Error", "Failed to create timer", IconPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Exception("Error starting long break", ex, GetType());
                        Context.API.ShowMsg("Pomodoro Error", "Error: " + ex.Message, IconPath);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                Log.Exception("Error in StartLongBreak", ex, GetType());
                Context.API.ShowMsg("Pomodoro Error", "Error: " + ex.Message, IconPath);
                return false;
            }
        }

        private bool PauseTimer(string parameters)
        {
            if (Context == null || ApiService == null || CurrentSession == null)
            {
                Log.Error("Context, ApiService, or CurrentSession is null", GetType());
                Context?.API.ShowMsg("Pomodoro Error", "No active timer to pause", IconPath);
                return false;
            }

            try
            {
                Task.Run(async () =>
                {
                    try
                    {
                        var success = await ApiService.PauseTimerAsync(CurrentSession.TimerId);
                        if (success)
                        {
                            CurrentSession.IsPaused = true;
                            Context.API.ShowMsg("Timer Paused", $"{CurrentSession.Type} timer paused", IconPath);
                        }
                        else
                        {
                            Context.API.ShowMsg("Pomodoro Error", "Failed to pause timer", IconPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Exception("Error pausing timer", ex, GetType());
                        Context.API.ShowMsg("Pomodoro Error", "Error: " + ex.Message, IconPath);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                Log.Exception("Error in PauseTimer", ex, GetType());
                Context.API.ShowMsg("Pomodoro Error", "Error: " + ex.Message, IconPath);
                return false;
            }
        }

        private bool ResumeTimer(string parameters)
        {
            if (Context == null || ApiService == null || CurrentSession == null)
            {
                Log.Error("Context, ApiService, or CurrentSession is null", GetType());
                Context?.API.ShowMsg("Pomodoro Error", "No paused timer to resume", IconPath);
                return false;
            }

            try
            {
                Task.Run(async () =>
                {
                    try
                    {
                        var success = await ApiService.ResumeTimerAsync(CurrentSession.TimerId);
                        if (success)
                        {
                            CurrentSession.IsPaused = false;
                            Context.API.ShowMsg("Timer Resumed", $"{CurrentSession.Type} timer resumed", IconPath);
                        }
                        else
                        {
                            Context.API.ShowMsg("Pomodoro Error", "Failed to resume timer", IconPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Exception("Error resuming timer", ex, GetType());
                        Context.API.ShowMsg("Pomodoro Error", "Error: " + ex.Message, IconPath);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                Log.Exception("Error in ResumeTimer", ex, GetType());
                Context.API.ShowMsg("Pomodoro Error", "Error: " + ex.Message, IconPath);
                return false;
            }
        }

        private bool StopTimer(string parameters)
        {
            if (Context == null || ApiService == null || CurrentSession == null)
            {
                Log.Error("Context, ApiService, or CurrentSession is null", GetType());
                Context?.API.ShowMsg("Pomodoro Error", "No active timer to stop", IconPath);
                return false;
            }

            try
            {
                Task.Run(async () =>
                {
                    try
                    {
                        var success = await ApiService.StopTimerAsync(CurrentSession.TimerId);
                        if (success)
                        {
                            string message = $"{CurrentSession.Type} timer stopped";
                            Context.API.ShowMsg("Timer Stopped", message, IconPath);
                            CurrentSession = null;
                        }
                        else
                        {
                            Context.API.ShowMsg("Pomodoro Error", "Failed to stop timer", IconPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Exception("Error stopping timer", ex, GetType());
                        Context.API.ShowMsg("Pomodoro Error", "Error: " + ex.Message, IconPath);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                Log.Exception("Error in StopTimer", ex, GetType());
                Context.API.ShowMsg("Pomodoro Error", "Error: " + ex.Message, IconPath);
                return false;
            }
        }

        private bool ShowStatus(string parameters)
        {
            if (Context == null)
            {
                Log.Error("Context is null", GetType());
                return false;
            }

            try
            {
                if (CurrentSession == null)
                {
                    Context.API.ShowMsg("Pomodoro Status", "No active timer", IconPath);
                    return true;
                }

                var remainingTime = CurrentSession.EndTime - DateTime.Now;
                string status = CurrentSession.IsPaused ? "paused" : "active";
                string remainingStr = remainingTime.TotalMinutes > 0
                    ? $"{Math.Floor(remainingTime.TotalMinutes)}m {remainingTime.Seconds}s remaining"
                    : "Time's up!";

                Context.API.ShowMsg(
                    $"{CurrentSession.Type} - {status}",
                    $"{remainingStr}",
                    IconPath);

                return true;
            }
            catch (Exception ex)
            {
                Log.Exception("Error in ShowStatus", ex, GetType());
                Context.API.ShowMsg("Pomodoro Error", "Error: " + ex.Message, IconPath);
                return false;
            }
        }

        private void ShowPomodoroWindow(PomodoroSession session)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Create and show the Pomodoro window
                    var window = new PomodoroResultWindow(this);
                    window.SetSession(session);
                    window.Show();
                });
            }
            catch (Exception ex)
            {
                Log.Exception("Error showing Pomodoro window", ex, GetType());
                Context?.API.ShowMsg("Pomodoro Error", "An error occurred: " + ex.Message, IconPath);
            }
        }

        private void ShowNonModalNotification(string title, string message)
        {
            // Only show a notification if ShowNotifications is enabled
            if (!ShowNotifications || Context == null)
                return;

            // Use Task.Run to ensure this doesn't block the main UI thread
            Task.Run(() => 
            {
                try
                {
                    // Use PowerToys notification system, but ensure it's non-blocking
                    Context.API.ShowMsg(title, message, IconPath);
                }
                catch (Exception ex)
                {
                    Log.Exception("Error showing notification", ex, GetType());
                }
            });
        }
    }
}