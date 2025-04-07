using System;
using System.Collections.Generic;

namespace Community.PowerToys.Run.Plugin.Pomodoro.Models
{
    /// <summary>
    /// Represents settings for the Pomodoro plugin.
    /// </summary>
    public class PomodoroSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show notifications when timers complete.
        /// </summary>
        public bool ShowNotifications { get; set; } = true;

        /// <summary>
        /// Gets or sets the default length of a Pomodoro session in minutes.
        /// </summary>
        public int PomodoroLength { get; set; }

        /// <summary>
        /// Gets or sets the default length of a short break in minutes.
        /// </summary>
        public int ShortBreakLength { get; set; }

        /// <summary>
        /// Gets or sets the default length of a long break in minutes.
        /// </summary>
        public int LongBreakLength { get; set; }

        /// <summary>
        /// Gets or sets the number of pomodoros before a long break.
        /// </summary>
        public int PomodorosBeforeLongBreak { get; set; } = 4;

        /// <summary>
        /// Gets or sets a value indicating whether to automatically start the next pomodoro session after a break.
        /// </summary>
        public bool AutoStartNextSession { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to play a sound when timer completes.
        /// </summary>
        public bool PlaySound { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to show a progress indicator in the system tray.
        /// </summary>
        public bool ShowTrayProgress { get; set; } = true;

        /// <summary>
        /// Gets or sets the sound file to play when Pomodoro timer completes.
        /// </summary>
        public string PomodoroCompletionSound { get; set; } = "default";

        /// <summary>
        /// Gets or sets the sound file to play when break timer completes.
        /// </summary>
        public string BreakCompletionSound { get; set; } = "default";

        /// <summary>
        /// Gets or sets a value indicating whether to keep session statistics.
        /// </summary>
        public bool TrackStatistics { get; set; } = true;

        /// <summary>
        /// Gets or sets the custom color for Pomodoro sessions.
        /// </summary>
        public string PomodoroColor { get; set; } = "#FF5252";

        /// <summary>
        /// Gets or sets the custom color for short break sessions.
        /// </summary>
        public string ShortBreakColor { get; set; } = "#4CAF50";

        /// <summary>
        /// Gets or sets the custom color for long break sessions.
        /// </summary>
        public string LongBreakColor { get; set; } = "#2196F3";

        /// <summary>
        /// Gets or sets a list of recently used timer durations.
        /// </summary>
        public List<int> RecentTimerDurations { get; set; } = new List<int>();

        /// <summary>
        /// Gets or sets the completed pomodoro count for the current day.
        /// </summary>
        public int DailyPomodoroCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the last reset date for the daily pomodoro count.
        /// </summary>
        public DateTime LastResetDate { get; set; } = DateTime.Now.Date;

        /// <summary>
        /// Gets or sets the daily pomodoro target.
        /// </summary>
        public int DailyPomodoroTarget { get; set; } = 8;

        /// <summary>
        /// Resets the daily pomodoro count if needed.
        /// </summary>
        public void ResetDailyCountIfNeeded()
        {
            if (LastResetDate.Date != DateTime.Now.Date)
            {
                DailyPomodoroCount = 0;
                LastResetDate = DateTime.Now.Date;
            }
        }

        // Sound settings
        public bool PlaySounds { get; set; } = true;

        // Current pomodoro count in the cycle
        public int CurrentPomodoroCount { get; set; } = 0;

        /// <summary>
        /// Adds a timer duration to the recent list.
        /// </summary>
        /// <param name="duration">The duration to add in minutes.</param>
        public void AddRecentDuration(int duration)
        {
            // Remove if already exists to avoid duplicates
            RecentTimerDurations.Remove(duration);

            // Add to beginning
            RecentTimerDurations.Insert(0, duration);

            // Keep only the most recent 5
            if (RecentTimerDurations.Count > 5)
            {
                RecentTimerDurations.RemoveAt(RecentTimerDurations.Count - 1);
            }
        }
    }
}