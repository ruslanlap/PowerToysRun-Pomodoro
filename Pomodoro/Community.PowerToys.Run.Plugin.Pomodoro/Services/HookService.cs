using System;
using System.Collections.Generic;
using System.Diagnostics;
using Community.PowerToys.Run.Plugin.Pomodoro.Models;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.Pomodoro.Services
{
    /// <summary>
    /// Executes arbitrary CLI commands (hooks) on Pomodoro timer events.
    /// Users can define commands for session start, end, pause, resume, and stop,
    /// separately for each session type (Pomodoro, Short Break, Long Break).
    /// </summary>
    public class HookService
    {
        private readonly Type _callingType;

        /// <summary>
        /// Placeholder variables that are replaced in command strings before execution.
        /// </summary>
        /// <remarks>
        /// Available tokens:
        /// <list type="bullet">
        /// <item>{event} — the event name (start, end, pause, resume, stop)</item>
        /// <item>{type} — the session type (Pomodoro, Short Break, Long Break)</item>
        /// <item>{minutes} — the session length in minutes</item>
        /// </list>
        /// </remarks>
        public HookService(Type callingType)
        {
            _callingType = callingType;
        }

        /// <summary>
        /// Executes a hook command for the given event, if a command is configured.
        /// Tokens in the command string are replaced with values from the event.
        /// </summary>
        /// <param name="command">The raw command string (may be null or empty).</param>
        /// <param name="evt">The Pomodoro event details.</param>
        public void ExecuteHook(string? command, PomodoroEvent evt)
        {
            if (string.IsNullOrWhiteSpace(command))
                return;

            try
            {
                string expanded = ExpandTokens(command, evt);

                Log.Info($"Executing hook: {expanded}", _callingType);

                var psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {expanded}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                };

                var process = new Process { StartInfo = psi };
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                // Fire-and-forget: don't block the UI thread. The process runs
                // asynchronously and we don't wait for its exit.
            }
            catch (Exception ex)
            {
                Log.Exception($"Error executing hook: {command}", ex, _callingType);
            }
        }

        /// <summary>
        /// Replaces placeholder tokens in the command string with event values.
        /// </summary>
        private static string ExpandTokens(string command, PomodoroEvent evt)
        {
            return command
                .Replace("{event}", evt.EventName, StringComparison.OrdinalIgnoreCase)
                .Replace("{type}", evt.SessionType ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("{minutes}", evt.LengthMinutes.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
