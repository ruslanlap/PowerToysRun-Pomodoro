using System;
using System.Runtime.InteropServices;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.Pomodoro.Services
{
    /// <summary>
    /// Controls system-wide media playback (play/pause) using Windows media keys.
    /// Works with any media application that listens for media keys: Spotify,
    /// YouTube (in browsers), Windows Media Player, foobar2000, etc.
    /// </summary>
    public class MediaControlService
    {
        private readonly Type _callingType;

        // Virtual key codes for media keys
        private const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        private const int VK_MEDIA_NEXT_TRACK = 0xB0;
        private const int VK_MEDIA_PREV_TRACK = 0xB1;
        private const int VK_MEDIA_STOP = 0xB2;

        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, nuint dwExtraInfo);

        public MediaControlService(Type callingType)
        {
            _callingType = callingType;
        }

        /// <summary>
        /// Toggles media play/pause (sends the media play/pause key).
        /// </summary>
        public void TogglePlayPause()
        {
            try
            {
                SendMediaKey(VK_MEDIA_PLAY_PAUSE);
                Log.Info("Media play/pause toggled", _callingType);
            }
            catch (Exception ex)
            {
                Log.Exception("Error toggling media play/pause", ex, _callingType);
            }
        }

        /// <summary>
        /// Sends the media stop key.
        /// </summary>
        public void Stop()
        {
            try
            {
                SendMediaKey(VK_MEDIA_STOP);
                Log.Info("Media stop sent", _callingType);
            }
            catch (Exception ex)
            {
                Log.Exception("Error stopping media", ex, _callingType);
            }
        }

        /// <summary>
        /// Sends the next track key.
        /// </summary>
        public void NextTrack()
        {
            try
            {
                SendMediaKey(VK_MEDIA_NEXT_TRACK);
                Log.Info("Media next track sent", _callingType);
            }
            catch (Exception ex)
            {
                Log.Exception("Error sending next track", ex, _callingType);
            }
        }

        /// <summary>
        /// Sends the previous track key.
        /// </summary>
        public void PreviousTrack()
        {
            try
            {
                SendMediaKey(VK_MEDIA_PREV_TRACK);
                Log.Info("Media previous track sent", _callingType);
            }
            catch (Exception ex)
            {
                Log.Exception("Error sending previous track", ex, _callingType);
            }
        }

        private void SendMediaKey(int keyCode)
        {
            keybd_event((byte)keyCode, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event((byte)keyCode, 0, KEYEVENTF_KEYUP, 0);
        }
    }
}
