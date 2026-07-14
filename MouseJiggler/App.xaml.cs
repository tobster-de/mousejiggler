using System.Globalization;
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using MouseJiggler.Properties;

namespace MouseJiggler;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private int _jiggleCountdown;

    private DispatcherTimer? _jiggleTimer;
    private TaskbarIcon? _taskbarIcon;
    private MenuItem? _menuItemActive;
    private JigglePattern? _jigglePattern;

    public bool JiggleActive { get; set; }

    public int JigglePeriod { get; set; }

    public JiggleMode JiggleMode { get; set; }

    public int JiggleSize { get; set; }

    public ActivityDetectionMode ActivityDetectionMode { get; set; }

    public void ApplySettings()
    {
        this.JigglePeriod = Settings.Default.JiggleInterval;
        this.JiggleMode = Settings.Default.JiggleMode;
        this.JiggleSize = Settings.Default.JiggleSize;
        this.ActivityDetectionMode = Settings.Default.ActivityDetectionMode;

        this.UpdateNotificationAreaText();
        this.UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (_jiggleTimer == null)
        {
            return;
        }

        _jiggleTimer.Stop();

        if (this.JiggleActive)
        {
            _jigglePattern = new JigglePattern(this.JiggleMode, this.JiggleSize);
            _jiggleCountdown = this.JigglePeriod;
            _jiggleTimer.Start();
        }
    }

    private void UpdateNotificationAreaText()
    {
        if (_taskbarIcon == null)
        {
            return;
        }

        string? jiggleMode
            = EnumToDisplayAttribConverter.Instance.Convert(this.JiggleMode, typeof(string), null, CultureInfo.CurrentCulture) as string;

        string jiggleToolTip = this.ActivityDetectionMode != ActivityDetectionMode.Off
                                   ? MouseJiggler.Properties.Resources.TrayToolTip_JigglingWhenInactive
                                   : MouseJiggler.Properties.Resources.TrayToolTip_Jiggling;

        _taskbarIcon.ToolTipText = this.JiggleActive
                                       ? string.Format(jiggleToolTip, this.JigglePeriod, jiggleMode, this.JiggleSize)
                                       : MouseJiggler.Properties.Resources.TrayToolTip_NotJiggling;

        _menuItemActive?.IsChecked = this.JiggleActive;
    }

    private void JiggleTimer_Tick(object? sender, EventArgs e)
    {
        if (this.ActivityDetectionMode != ActivityDetectionMode.Off && this.CheckUserActivity())
        {
            return;
        }

        _jiggleCountdown -= (int)_jiggleTimer!.Interval.TotalSeconds;
        if (_jiggleCountdown <= 0)
        {
            _jiggleCountdown = this.JigglePeriod;
            _jigglePattern?.Perform();
        }
    }

    private bool CheckUserActivity()
    {
        switch (this.ActivityDetectionMode)
        {
            case ActivityDetectionMode.Off:
                return false;
            case ActivityDetectionMode.MouseMovement:
                if (Helpers.CheckMovement())
                {
                    _jiggleCountdown = this.JigglePeriod;
                    return true;
                }

                return false;
            case ActivityDetectionMode.WinApiIdleTime:

                if (!Helpers.TryGetIdleTimeInSeconds(out double idleTimeInSeconds))
                {
                    // handle as activity, when idle state cannot be determined
                    _jiggleCountdown = this.JigglePeriod;
                    return true;
                }

                if (idleTimeInSeconds < this.JigglePeriod)
                {
                    _jiggleCountdown = this.JigglePeriod;
                    return true;
                }

                return false;
            default:
                throw new ArgumentOutOfRangeException(nameof(this.ActivityDetectionMode), this.ActivityDetectionMode, null);
        }
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // ReSharper disable once AssignNullToNotNullAttribute - it throws if not found
        _taskbarIcon = (TaskbarIcon)this.FindResource("NotifyIcon");
        _menuItemActive = _taskbarIcon?.ContextMenu?.Items
                                      .OfType<MenuItem>()
                                      .FirstOrDefault(x => Equals(x.Name, "MenuItemActivate"));

        _jiggleTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(5)
        };
        _jiggleTimer.Tick += this.JiggleTimer_Tick;

        this.UpdateNotificationAreaText();
        this.UpdateTimer();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        _jiggleTimer?.Stop();
        _jiggleTimer = null;
    }
}