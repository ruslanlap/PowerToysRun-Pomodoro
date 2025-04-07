using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Community.PowerToys.Run.Plugin.Pomodoro.Models;
using Wox.Plugin;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.Pomodoro
{
    /// <summary>
    /// Interaction logic for PomodoroResultWindow.xaml
    /// </summary>
    public partial class PomodoroResultWindow : Window
    {
        private string _currentSessionType = "Pomodoro"; // Track current session type
        private readonly Main _parent = null!; // Using null-forgiving operator
        private PomodoroSession? _session;
        private readonly DispatcherTimer _timer = null!; // Using null-forgiving operator
        private DateTime _endTime;
        private TimeSpan _totalDuration;
        private SolidColorBrush _timerBrush = null!; // Using null-forgiving operator
        private readonly List<Ellipse> _dots = new();
        private const int MaxDots = 20; // Reduced number of dots for Windows 11 style
        private int _totalSeconds;
        private int _remainingSeconds;
        private bool _isClosing = false;

        // Improved method to get local image path
        private string GetLocalImagePath(string filename)
        {
            try
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                string assemblyDirectory = System.IO.Path.GetDirectoryName(assemblyLocation) ?? "";
                string imagePath = System.IO.Path.Combine(assemblyDirectory, "Images", filename);

                // Ensure the path is absolute
                return System.IO.Path.GetFullPath(imagePath);
            }
            catch (Exception ex)
            {
                Log.Exception($"Error creating path for {filename}", ex, GetType());
                return "";
            }
        }

        // Helper method to properly set image sources with fallback options
        // Helper method to properly set image sources with fallback options
        private void SetImageWithFallback(string primaryImage, string fallbackImage)
        {
            try
            {
                // Try to load the primary image
                string primaryPath = GetLocalImagePath(primaryImage);
                Log.Info($"[SetImageWithFallback] Attempting to load image from: {primaryPath}", GetType());

                if (File.Exists(primaryPath))
                {
                    Log.Info($"[SetImageWithFallback] File exists at {primaryPath}", GetType());

                    // Create a new BitmapImage with cache busting
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; // Force immediate loading
                    bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache; // Ignore cache
                    bitmap.UriSource = new Uri(primaryPath, UriKind.Absolute);
                    bitmap.EndInit();
                    bitmap.Freeze(); // Optimize rendering

                    // Set the source after the image is fully initialized
                    Dispatcher.Invoke(() => {
                        Log.Info($"[SetImageWithFallback] Setting source to {primaryPath}", GetType());
                        // Clear old source first
                        StaticBallImage.Source = null;
                        StaticBallImage.Source = bitmap;
                    });

                    Log.Info($"[SetImageWithFallback] Successfully loaded image: {primaryPath}", GetType());
                    return;
                }
                else
                {
                    Log.Warn($"[SetImageWithFallback] Primary image file not found at: {primaryPath}", GetType());
                }

                // If primary image doesn't exist, try fallback
                string fallbackPath = GetLocalImagePath(fallbackImage);
                Log.Info($"[SetImageWithFallback] Trying fallback image at: {fallbackPath}", GetType());

                if (File.Exists(fallbackPath))
                {
                    Log.Info($"[SetImageWithFallback] Fallback file exists at {fallbackPath}", GetType());

                    // Create a new BitmapImage
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bitmap.UriSource = new Uri(fallbackPath, UriKind.Absolute);
                    bitmap.EndInit();
                    bitmap.Freeze();

                    // Set the source on the UI thread
                    Dispatcher.Invoke(() => {
                        Log.Info($"[SetImageWithFallback] Setting source to fallback {fallbackPath}", GetType());
                        StaticBallImage.Source = null;
                        StaticBallImage.Source = bitmap;
                    });

                    Log.Info($"[SetImageWithFallback] Loaded fallback image: {fallbackPath}", GetType());
                }
                else
                {
                    Log.Error($"[SetImageWithFallback] Both primary and fallback images not found", GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Exception($"[SetImageWithFallback] Error loading image {primaryImage}", ex, GetType());
            }
        }

        private void AutoDismissStartupDialogs()
        {
            try
            {
                // Find all open message boxes by their window title
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != this && (window.Title == "Starting Pomodoro" ||
                                             window.Title == "Starting Break" ||
                                             window.Title == "Starting Long Break"))
                    {
                        // Close the dialog window
                        window.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Exception("Error auto-dismissing dialogs", ex, GetType());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PomodoroResultWindow"/> class.
        /// </summary>
        /// <param name="parent">The parent Main plugin instance.</param>
        public PomodoroResultWindow(Main parent)
        {
            try
            {
                _parent = parent ?? throw new ArgumentNullException(nameof(parent));
                _timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                _timer.Tick += Timer_Tick;

                // Set default values
                _endTime = DateTime.MinValue;
                _totalDuration = TimeSpan.Zero;
                _totalSeconds = 0;
                _remainingSeconds = 0;

                // Default timer brush - Changed to requested green color
                _timerBrush = new SolidColorBrush(Color.FromRgb(166, 218, 149)); // #a6da95

                // Initialize component after setting up properties
                InitializeComponent();

                AutoDismissStartupDialogs();

                // Set progress bar color
                ProgressBarTimer.Foreground = _timerBrush;
                StatusBarTimer.Foreground = _timerBrush;

                Closing += PomodoroResultWindow_Closing;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing Pomodoro window: {ex.Message}", "Pomodoro Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Exception("Error initializing PomodoroResultWindow", ex, GetType());
                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStaticImage();
            ApplyWindowTransparency();
        }

        /// <summary>
        /// Loads the tomato image for the timer using a hardcoded local path.
        /// </summary>
        private void LoadStaticImage()
        {
            try
            {
                // Don't override the session-specific image if we already have an active session
                if (_session != null)
                {
                    Log.Info($"[LoadStaticImage] Skipping default image load because session is active: {_currentSessionType}", GetType());
                    return; // Skip loading the default image if we have an active session
                }

                // Only load the default tomato image for initial startup
                SetImageWithFallback("tomato.dark.png", "pomodoro.dark.png");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading static image: {ex.Message}");
                Log.Exception("Error loading static image", ex, GetType());
            }
        }

        /// <summary>
        /// Applies transparency effect to the window.
        /// </summary>
        private void ApplyWindowTransparency()
        {
            try
            {
                // Create a fade animation
                DoubleAnimation opacityAnimation = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0.9,
                    Duration = new Duration(TimeSpan.FromSeconds(5)),
                    AutoReverse = false
                };

                // Apply the animation to the window
                this.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            }
            catch (Exception ex)
            {
                Log.Exception("Error applying window transparency", ex, GetType());
            }
        }

        /// <summary>
        /// Updates the UI based on session type using hardcoded local image paths.
        /// </summary>
        /// <param name="sessionType">The type of session</param>
        private void UpdateSessionTypeUI(string sessionType)
        {
            if (TxtTimerType == null || ProgressBarTimer == null || StatusBarTimer == null || BtnStop == null || StaticBallImage == null)
                return;

            try
            {
                // Update the session type field
                _currentSessionType = sessionType;
                TxtTimerType.Text = sessionType;

                Log.Info($"[UpdateSessionTypeUI] Setting UI for session type: {sessionType}", GetType());

                // Set colors based on session type and load the appropriate icon
                switch (sessionType)
                {
                    case "Pomodoro":
                        ProgressBarTimer.Foreground = (SolidColorBrush)FindResource("PomodoroColor");
                        StatusBarTimer.Foreground = (SolidColorBrush)FindResource("PomodoroColor");
                        BtnStop.Background = (SolidColorBrush)FindResource("PomodoroColor");
                        SetImageWithFallback("pomodoro.dark.png", "tomato.dark.png");
                        break;

                    case "Short Break":
                        ProgressBarTimer.Foreground = (SolidColorBrush)FindResource("ShortBreakColor");
                        StatusBarTimer.Foreground = (SolidColorBrush)FindResource("ShortBreakColor");
                        BtnStop.Background = (SolidColorBrush)FindResource("ShortBreakColor");
                        SetImageWithFallback("shortbreak.png", "pomodoro.dark.png");
                        break;

                    case "Long Break":
                        ProgressBarTimer.Foreground = (SolidColorBrush)FindResource("LongBreakColor");
                        StatusBarTimer.Foreground = (SolidColorBrush)FindResource("LongBreakColor");
                        BtnStop.Background = (SolidColorBrush)FindResource("LongBreakColor");
                        SetImageWithFallback("longbreak.png", "pomodoro.dark.png");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Exception("Error updating session type UI", ex, GetType());
            }
        }

        /// <summary>
        /// Sets the Pomodoro session information.
        /// </summary>
        /// <param name="session">The Pomodoro session to display.</param>
        public void SetSession(PomodoroSession session)
        {
            if (ProgressBarTimer != null)
            {
                ProgressBarTimer.Maximum = 100;
                ProgressBarTimer.Foreground = _timerBrush;
            }

            if (StatusBarTimer != null)
            {
                StatusBarTimer.Maximum = 100;
                StatusBarTimer.Foreground = _timerBrush;
            }

            if (_isClosing)
                return;

            try
            {
                if (session == null)
                    throw new ArgumentNullException(nameof(session));

                _session = session;

                // Update UI elements safely
                Dispatcher.Invoke(() =>
                {
                    TxtTimerType.Text = session.Type;

                    _endTime = session.EndTime;
                    _totalDuration = TimeSpan.FromMinutes(session.LengthMinutes);
                    _totalSeconds = (int)_totalDuration.TotalSeconds;
                    _remainingSeconds = _totalSeconds;

                    // Set color based on session type - Updated with new colors
                    Color colorValue;
                    switch (session.Type)
                    {
                        case "Pomodoro":
                            colorValue = Color.FromRgb(166, 218, 149); // #a6da95 (green)
                            break;
                        case "Short Break":
                            colorValue = Color.FromRgb(38, 160, 218); // Windows 11 blue
                            break;
                        case "Long Break":
                            colorValue = Color.FromRgb(94, 91, 236); // Windows 11 purple
                            break;
                        default:
                            colorValue = Color.FromRgb(166, 218, 149); // Default green
                            break;
                    }

                    _timerBrush = new SolidColorBrush(colorValue);

                    if (ProgressBarTimer != null)
                        ProgressBarTimer.Foreground = _timerBrush;

                    if (StatusBarTimer != null)
                        StatusBarTimer.Foreground = _timerBrush;

                    Title = $"PowerToys Run - {session.Type}";

                    // Update window title and UI based on session type using local paths
                    UpdateSessionTypeUI(session.Type);

                    // Initialize the dots
                    InitializeDots();

                    // Start the timer
                    _timer.Start();
                    UpdateTimerDisplay();

                    // Apply styling to buttons based on session type
                    if (BtnPauseResume != null && BtnStop != null)
                    {
                        BtnStop.Background = _timerBrush;
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Exception("Error setting session", ex, GetType());
                MessageBox.Show($"Error setting up timer: {ex.Message}", "Pomodoro Error", MessageBoxButton.OK, MessageBoxImage.Error);

                if (!_isClosing)
                    Close();
            }
        }

        private void InitializeDots()
        {
            if (_isClosing || DotsContainer == null)
                return;

            try
            {
                // Clear any existing dots
                DotsContainer.Items.Clear();
                _dots.Clear();

                // Create dots with the timer color - Windows 11 smaller dots
                for (int i = 0; i < MaxDots; i++)
                {
                    var dot = new Ellipse
                    {
                        Width = 4, // Smaller dots for Windows 11 style
                        Height = 4,
                        Fill = _timerBrush,
                        Margin = new Thickness(2, 0, 2, 0),
                        Opacity = 0.8
                    };

                    // Add subtle pulsing animation to dots
                    var animation = new DoubleAnimation
                    {
                        From = 0.5,
                        To = 1.0,
                        Duration = new Duration(TimeSpan.FromSeconds(1.5)),
                        AutoReverse = true,
                        RepeatBehavior = RepeatBehavior.Forever
                    };

                    // Each dot pulses at a slightly different time
                    animation.BeginTime = TimeSpan.FromMilliseconds(75 * i);

                    dot.BeginAnimation(OpacityProperty, animation);

                    _dots.Add(dot);
                    DotsContainer.Items.Add(dot);
                }

                UpdateDotsVisibility();
            }
            catch (Exception ex)
            {
                Log.Exception("Error initializing dots", ex, GetType());
            }
        }

        private void UpdateDotsVisibility()
        {
            if (_isClosing || _dots == null || _dots.Count == 0 || _totalSeconds <= 0)
                return;

            try
            {
                // Calculate how many dots should be visible based on time remaining
                int visibleDots = (int)Math.Ceiling(((double)_remainingSeconds / _totalSeconds) * MaxDots);

                // Update visibility of each dot
                for (int i = 0; i < _dots.Count; i++)
                {
                    if (_dots[i] != null)
                        _dots[i].Visibility = i < visibleDots ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Log.Exception("Error updating dot visibility", ex, GetType());
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_isClosing || _session == null)
                return;

            try
            {
                if (_session.IsPaused)
                    return;

                // Force an immediate update of the timer display
                UpdateTimerDisplay();

                // Check if timer has completed
                if (DateTime.Now >= _endTime)
                {
                    _timer.Stop();

                    // Update UI on the UI thread
                    Dispatcher.Invoke(() =>
                    {
                        if (ProgressBarTimer != null)
                            ProgressBarTimer.Value = 0;

                        if (StatusBarTimer != null)
                            StatusBarTimer.Value = 0;

                        if (TxtTimeRemaining != null)
                            TxtTimeRemaining.Text = "00:00";

                        _remainingSeconds = 0;
                        UpdateDotsVisibility();

                        if (!_isClosing)
                        {
                            try
                            {
                                // Notify parent of completion for auto-start next phase
                                _parent.OnTimerCompleted(_session);

                                // Keep window open but update UI
                                if (BtnPauseResume != null)
                                {
                                    BtnPauseResume.IsEnabled = false;
                                    BtnPauseResume.Content = "Completed";
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Exception("Error handling timer completion", ex, GetType());
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Exception("Error in timer tick", ex, GetType());
                _timer.Stop();
            }
        }

        public void UpdateTimeAfterResume()
        {
            if (_session == null || _isClosing)
                return;

            try
            {
                // Recalculate the end time based on current remaining time
                var currentRemaining = _remainingSeconds;
                _endTime = DateTime.Now.AddSeconds(currentRemaining);

                // Force UI update
                UpdateTimerDisplay();
            }
            catch (Exception ex)
            {
                Log.Exception("Error updating timer after resume", ex, GetType());
            }
        }

        private void UpdateTimerDisplay()
        {
            if (_isClosing || TxtTimeRemaining == null || ProgressBarTimer == null || StatusBarTimer == null)
                return;

            try
            {
                // Calculate remaining time
                TimeSpan remaining = _endTime - DateTime.Now;
                if (remaining.TotalSeconds <= 0)
                {
                    remaining = TimeSpan.Zero;
                }

                // Update display on UI thread
                Dispatcher.Invoke(() =>
                {
                    TxtTimeRemaining.Text = $"{Math.Floor(remaining.TotalMinutes):00}:{remaining.Seconds:00}";
                    _remainingSeconds = (int)remaining.TotalSeconds;
                    UpdateDotsVisibility();

                    double percentRemaining = (remaining.TotalSeconds / _totalDuration.TotalSeconds) * 100;
                    ProgressBarTimer.Value = Math.Max(0, Math.Min(100, percentRemaining));
                    StatusBarTimer.Value = Math.Max(0, Math.Min(100, percentRemaining));
                });
            }
            catch (Exception ex)
            {
                Log.Exception("Error updating timer display", ex, GetType());
            }
        }

        private void BtnPauseResume_Click(object sender, RoutedEventArgs e)
        {
            if (_isClosing || _session == null || _parent == null)
                return;

            try
            {
                if (_session.IsPaused)
                {
                    _session.IsPaused = false;
                    BtnPauseResume.Content = "Pause";
                    _parent.Query(new Query("resume"));
                    UpdateTimeAfterResume();
                }
                else
                {
                    _session.IsPaused = true;
                    BtnPauseResume.Content = "Resume";
                    _parent.Query(new Query("pause"));
                }
            }
            catch (Exception ex)
            {
                Log.Exception("Error in pause/resume", ex, GetType());
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            if (_isClosing)
                return;

            try
            {
                _timer.Stop();
                _parent?.Query(new Query("stop"));
                _isClosing = true;
                Close();
            }
            catch (Exception ex)
            {
                Log.Exception("Error stopping timer", ex, GetType());
                _isClosing = true;
                Close();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            BtnStop_Click(sender, e);
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_isClosing && e.ChangedButton == MouseButton.Left)
            {
                try
                {
                    this.DragMove();
                }
                catch (Exception ex)
                {
                    Log.Exception("Error in drag move", ex, GetType());
                }
            }
        }

        private void PomodoroResultWindow_Closing(object? sender, CancelEventArgs e)
        {
            _isClosing = true;
            _timer.Stop();
        }
    }

    // ProgressBar width converter needed for the custom style
    public class ProgressBarWidthConverter : System.Windows.Data.IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (values.Length < 4)
                    return 0.0;

                double value = (double)values[0];
                double minimum = (double)values[1];
                double maximum = (double)values[2];
                double actualWidth = (double)values[3];

                if (maximum == minimum)
                    return 0.0;

                return ((value - minimum) / (maximum - minimum)) * actualWidth;
            }
            catch
            {
                return 0.0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}