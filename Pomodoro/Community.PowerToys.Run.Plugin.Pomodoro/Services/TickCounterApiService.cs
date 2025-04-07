using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Community.PowerToys.Run.Plugin.Pomodoro.Models;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.Pomodoro.Services
{
    /// <summary>
    /// Service for managing timers locally instead of using external API.
    /// </summary>
    public class TickCounterApiService : IDisposable
    {
        private readonly Type _callingType;
        private readonly Dictionary<string, Timer> _activeTimers = new();
        private bool _disposed;

        private class Timer
        {
            public int RemainingAtPause { get; set; }
            public string? Id { get; set; }
            public string? Title { get; set; }
            public int TotalSeconds { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime? PauseTime { get; set; }
            public TimeSpan PausedDuration { get; set; } = TimeSpan.Zero;
            public bool IsPaused { get; set; }

            public bool IsCompleted => !IsPaused && RemainingSeconds <= 0;

            public int RemainingSeconds
            {
                get
                {
                    if (IsPaused)
                    {
                        return RemainingAtPause;
                    }

                    var elapsed = DateTime.Now - StartTime - PausedDuration;
                    return Math.Max(0, TotalSeconds - (int)elapsed.TotalSeconds);
                }
            }
        }

        public TickCounterApiService(Type callingType)
        {
            _callingType = callingType ?? throw new ArgumentNullException(nameof(callingType));
        }

        public Task<TimerResponse?> CreateTimerAsync(string title, int durationSeconds)
        {
            try
            {
                var timerId = Guid.NewGuid().ToString();
                var timer = new Timer
                {
                    Id = timerId,
                    Title = title,
                    TotalSeconds = durationSeconds,
                    StartTime = DateTime.Now,
                    IsPaused = false
                };

                _activeTimers[timerId] = timer;

                return Task.FromResult<TimerResponse?>(new TimerResponse
                {
                    TimerId = timerId,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                Log.Exception("Error creating timer", ex, _callingType);
                return Task.FromResult<TimerResponse?>(null);
            }
        }

        public Task<bool> PauseTimerAsync(string timerId)
        {
            try
            {
                if (_activeTimers.TryGetValue(timerId, out var timer))
                {
                    timer.IsPaused = true;
                    timer.PauseTime = DateTime.Now;
                    timer.RemainingAtPause = timer.RemainingSeconds;
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Log.Exception("Error pausing timer", ex, _callingType);
                return Task.FromResult(false);
            }
        }

        public Task<bool> ResumeTimerAsync(string timerId)
        {
            try
            {
                if (_activeTimers.TryGetValue(timerId, out var timer) && timer.PauseTime.HasValue)
                {
                    // Get exact elapsed time in the paused state
                    TimeSpan pauseDuration = DateTime.Now - timer.PauseTime.Value;

                    // Simply adjust the start time by adding the pause duration
                    // This is the most reliable approach
                    timer.StartTime = timer.StartTime.Add(pauseDuration);

                    // Clear pause state
                    timer.IsPaused = false;
                    timer.PauseTime = null;

                    // We don't need to track cumulative pause duration with this approach
                    // since we're directly adjusting the start time

                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Log.Exception("Error resuming timer", ex, _callingType);
                return Task.FromResult(false);
            }
        }
        
        public Task<bool> StopTimerAsync(string timerId)
        {
            try
            {
                var result = _activeTimers.Remove(timerId);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                Log.Exception("Error stopping timer", ex, _callingType);
                return Task.FromResult(false);
            }
        }
        public Task<TimerStatus?> GetTimerStatusAsync(string timerId)
        {
            try
            {
                if (_activeTimers.TryGetValue(timerId, out var timer))
                {
                    return Task.FromResult<TimerStatus?>(new TimerStatus
                    {
                        Id = timer.Id ?? string.Empty,
                        Title = timer.Title ?? string.Empty,
                        TotalSeconds = timer.TotalSeconds,
                        RemainingSeconds = timer.RemainingSeconds,
                        IsPaused = timer.IsPaused,
                        IsCompleted = timer.IsCompleted
                    });
                }

                return Task.FromResult<TimerStatus?>(null);
            }
            catch (Exception ex)
            {
                Log.Exception("Error getting timer status", ex, _callingType);
                return Task.FromResult<TimerStatus?>(null);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _activeTimers.Clear();
            _disposed = true;
        }
    }

    public class TimerStatus
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public int TotalSeconds { get; set; }
        public int RemainingSeconds { get; set; }
        public bool IsPaused { get; set; }
        public bool IsCompleted { get; set; }
    }
}
