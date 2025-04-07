using System; // Add this line at the top of the file

namespace Community.PowerToys.Run.Plugin.Pomodoro.Models
{
    /// <summary>
    /// Represents a Pomodoro session.
    /// </summary>
    public class PomodoroSession
    {
        /// <summary>
        /// Gets or sets the timer ID from the API.
        /// </summary>
        public string TimerId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of session (Pomodoro, Short Break, Long Break).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the length of the session in minutes.
        /// </summary>
        public int LengthMinutes { get; set; }

        /// <summary>
        /// Gets or sets the start time of the session.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the session.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the timer is paused.
        /// </summary>
        public bool IsPaused { get; set; }
    }

    /// <summary>
    /// Represents a response from the TickCounter API.
    /// </summary>
    public class TimerResponse
    {
        /// <summary>
        /// Gets or sets the timer ID.
        /// </summary>
        public string TimerId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the request was successful.
        /// </summary>
        public bool Success { get; set; }
    }
}