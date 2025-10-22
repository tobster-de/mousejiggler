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
    private int _jiggleSize;

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
                ? $"Jiggling mouse every {this.JigglePeriod} s, mode: {this.JiggleMode} (△ {this.JiggleSize})."
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
        set
        {
            _jiggleMode = value;
            this.UpdateNotificationAreaText();
        }
    }

    public int JiggleSize
    {
        get => _jiggleSize;
        set
        {
            _jiggleSize = value;

            if (_jiggleSize <= 0)
            {
                this.UpdateNotificationAreaText();
                return;
            }
            
            _circlePoints.Clear();
            
            double radius = _jiggleSize / 2.0f;
            const int pointCount = 8;

            List<Point> points = new List<Point>(pointCount);
            for (int i = 0; i < pointCount; i++)
            {
                double angle = 2.0f * Math.PI * i / pointCount;
                int dx = (int)Math.Round(radius * Math.Cos(angle));
                int dy = (int)Math.Round(radius * Math.Sin(angle));
                points.Add(new Point(dx, dy));
            }

            // Speichere die Differenzen (Deltas) zwischen aufeinanderfolgenden Punkten,
            // inkl. Rücksprung vom letzten zum ersten Punkt
            for (int i = 0; i < pointCount; i++)
            {
                Point current = points[i];
                Point next = points[(i + 1) % pointCount];
                _circlePoints.Add(new Point(next.X - current.X, next.Y - current.Y));
            }

            this.UpdateNotificationAreaText();
        }
    }

    private void JiggleTimer_Tick(object? sender, EventArgs e)
    {
        switch (this.JiggleMode)
        {
            case JiggleMode.Zen:
                Helpers.Jiggle(0);
                break;
            case JiggleMode.ZigZag:
                Helpers.Jiggle(this.JiggleSize);
                Thread.Sleep(5);
                Helpers.Jiggle(-this.JiggleSize);
                break;
            case JiggleMode.Circle:
                foreach (Point p in _circlePoints)
                {
                    Helpers.Jiggle((int)p.X, (int)p.Y);
                    Thread.Sleep(5);
                }

                break;
            case JiggleMode.Smooth:
                int dx = this.JiggleSize / 5;
                for (int i = 0; i < 3; i++)
                {
                    Helpers.Jiggle(dx, 0);
                    Thread.Sleep(5);
                }

                for (int i = 0; i < 6; i++)
                {
                    Helpers.Jiggle(-dx, 0);
                    Thread.Sleep(5);
                }
                
                for (int i = 0; i < 3; i++)
                {
                    Helpers.Jiggle(dx, 0);
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