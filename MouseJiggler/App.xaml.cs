using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using System.Windows.Threading;
using MouseJiggler.Properties;

namespace MouseJiggler;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly List<Point> _circlePoints =
    [
        new Point(3, 2),
        new Point(2, 3),
        new Point(-2, 3),
        new Point(-3, 2),
        new Point(-3, -2),
        new Point(-2, -3),
        new Point(2, -3),
        new Point(3, -2)
    ];
        
    private DispatcherTimer? _jiggleTimer;
    private bool _jiggleActive;
    private int _jigglePeriod;
    private TaskbarIcon? _taskbarIcon;
    private JiggleMode _jiggleMode;

    public bool JiggleActive
    {
        get => _jiggleActive;
        set
        {
            _jiggleActive = value;
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
        _jiggleTimer.Interval = TimeSpan.FromSeconds(_jigglePeriod);

        if (_jiggleActive)
        {
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
            _jiggleActive
                ? $"Jiggling mouse every {this.JigglePeriod} s, mode: {this.JiggleMode}."
                : "Not jiggling the mouse.";
    }

    public int JigglePeriod
    {
        get => _jigglePeriod;
        set
        {
            _jigglePeriod = value;
            this.UpdateTimer();
        }
    }

    public JiggleMode JiggleMode
    {
        get => _jiggleMode;
        set => _jiggleMode = value;
    }

    private void JiggleTimer_Tick(object? sender, EventArgs e)
    {
        switch (this.JiggleMode)
        {
            case JiggleMode.Zen:
                Helpers.Jiggle(0);
                break;
            case JiggleMode.ZigZag:
                Helpers.Jiggle(4);
                Thread.Sleep(5);
                Helpers.Jiggle(-4);
                break;
            case JiggleMode.Circle:
                foreach (Point p in _circlePoints)
                {
                    Helpers.Jiggle((int)p.X, (int)p.Y);
                    Thread.Sleep(5);
                }

                break;
        }
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // ReSharper disable once AssignNullToNotNullAttribute - it throws if not found
        _taskbarIcon = (TaskbarIcon)this.FindResource("NotifyIcon");

        _jiggleTimer = new DispatcherTimer();
        _jiggleTimer.Tick += this.JiggleTimer_Tick;

        this.UpdateTimer();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        _jiggleTimer?.Stop();
        _jiggleTimer = null;
    }
}