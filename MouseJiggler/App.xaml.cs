using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using MouseJiggler.Properties;

namespace MouseJiggler;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private DispatcherTimer? _jiggleTimer;
    private int _jiggleCountdown;
    private TaskbarIcon? _taskbarIcon;
    private MenuItem? _menuItemActive;
    private JigglePattern? _jigglePattern;

    public void ApplySettings()
    {
        this.JigglePeriod = Settings.Default.JiggleInterval;
        this.JiggleMode = Settings.Default.JiggleMode;
        this.JiggleSize = Settings.Default.JiggleSize;
        this.CheckActivity = Settings.Default.CheckActivity;
        this.UpdateTimer();
    }

    public bool JiggleActive
    {
        get;
        set
        {
            field = value;
            this.UpdateTimer();
        }
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
            _jigglePattern = JigglePattern.Create(this.JiggleMode, this.JiggleSize);
            _jiggleCountdown = this.JigglePeriod;
            _jiggleTimer.Start();
        }

        this.UpdateNotificationAreaText();
    }

    private void UpdateNotificationAreaText()
    {
        if (_taskbarIcon == null)
        {
            return;
        }

        _taskbarIcon.ToolTipText =
            this.JiggleActive
                ? this.CheckActivity
                      ? string.Format(MouseJiggler.Properties.Resources.TrayToolTip_JigglingWhenInactive, this.JigglePeriod, this.JiggleMode, this.JiggleSize)
                      : string.Format(MouseJiggler.Properties.Resources.TrayToolTip_Jiggling, this.JigglePeriod, this.JiggleMode, this.JiggleSize)
                : MouseJiggler.Properties.Resources.TrayToolTip_NotJiggling;

        if (_menuItemActive != null)
        {
            _menuItemActive.IsChecked = this.JiggleActive;
        }
    }

    public int JigglePeriod
    {
        get;
        set
        {
            field = value;
            this.UpdateTimer();
        }
    }

    public JiggleMode JiggleMode
    {
        get;
        set
        {
            field = value;
            this.UpdateNotificationAreaText();
        }
    }

    public int JiggleSize
    {
        get;
        set
        {
            field = value;
            this.UpdateNotificationAreaText();
        }
    }

    public bool CheckActivity
    {
        get;
        set
        {
            field = value;
            this.UpdateNotificationAreaText();
        }
    }

    private void JiggleTimer_Tick(object? sender, EventArgs e)
    {
        if (this.CheckActivity && Helpers.CheckMovement())
        {
            _jiggleCountdown = this.JigglePeriod;
            return;
        }

        _jiggleCountdown -= (int)_jiggleTimer!.Interval.TotalSeconds;
        if (_jiggleCountdown <= 0)
        {
            _jiggleCountdown = this.JigglePeriod;
            _jigglePattern?.Perform();
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

        this.UpdateTimer();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        _jiggleTimer?.Stop();
        _jiggleTimer = null;
    }
}