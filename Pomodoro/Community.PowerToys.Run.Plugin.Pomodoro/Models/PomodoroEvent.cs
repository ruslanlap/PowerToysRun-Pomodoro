namespace Community.PowerToys.Run.Plugin.Pomodoro.Models
{
    /// <summary>
    /// Represents a timer lifecycle event that can trigger media control and custom hooks.
    /// </summary>
    public class PomodoroEvent
    {
        /// <summary>
        /// Gets or sets the event name: "start", "end", "pause", "resume", or "stop".
        /// </summary>
        public string EventName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the session type: "Pomodoro", "Short Break", or "Long Break".
        /// </summary>
        public string SessionType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the session length in minutes.
        /// </summary>
        public int LengthMinutes { get; set; }
    }
}
