using System;
using System.IO;
using System.Media;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.Pomodoro.Services
{
    public class SoundService
    {
        private const string StartSoundPath = "Sounds/startnewphase.wav";
        private const string EndSoundPath = "Sounds/endphase.wav";

        private readonly Type _callingType;
        private SoundPlayer? _player;

        public SoundService(Type callingType)
        {
            _callingType = callingType;
        }

        public void PlaySound(string soundName)
        {
            try
            {
                // Get the plugin dll directory first
                string pluginDirectory = Path.GetDirectoryName(typeof(SoundService).Assembly.Location) ?? 
                    AppDomain.CurrentDomain.BaseDirectory;

                // Use path based on the assembly location instead of hardcoded path
                string soundsFolder = Path.Combine(pluginDirectory, "Sounds");

                // Add debug logging to see the actual path
                Log.Info($"Looking for sound at: {soundsFolder}", _callingType);

                string soundPath = Path.Combine(soundsFolder, $"{soundName}.wav");

                if (!File.Exists(soundPath))
                {
                    Log.Error($"Sound file not found: {soundPath}", _callingType);
                    return;
                }

                _player?.Dispose();
                _player = new SoundPlayer(soundPath);
                _player.Play(); // This plays asynchronously without blocking
            }
            catch (Exception ex)
            {
                Log.Exception($"Error playing sound {soundName}", ex, _callingType);
            }
        }

        public void Dispose()
        {
            _player?.Dispose();
            _player = null;
        }
    }
}
