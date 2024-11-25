using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using System.Windows.Threading;

namespace MouseJiggler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private DispatcherTimer? jiggleTimer;
        private bool zig;
        private bool jiggleActive;
        private bool zenJiggleEnabled;
        private int jigglePeriod;
        private TaskbarIcon? taskbarIcon;

        public bool JiggleActive
        {
            get => jiggleActive;
            set
            {
                jiggleActive = value;
                this.UpdateTimer();
            }
        }

        private void UpdateTimer()
        {
            if (jiggleTimer == null)
            {
                return;
            }

            jiggleTimer.Stop();
            jiggleTimer.Interval = TimeSpan.FromSeconds(jigglePeriod);

            if (jiggleActive)
            {
                jiggleTimer.Start();
            }

            this.UpdateNotificationAreaText();
        }

        private void UpdateNotificationAreaText()
        {
            if (taskbarIcon == null)
            {
                return;
            }

            taskbarIcon.ToolTipText =
                jiggleActive
                    ? $"Jiggling mouse every {this.JigglePeriod} s, {(this.ZenJiggleEnabled ? "with" : "without")} Zen."
                    : "Not jiggling the mouse.";
        }

        public bool ZenJiggleEnabled
        {
            get => zenJiggleEnabled;
            set
            {
                zenJiggleEnabled = value;

                this.UpdateNotificationAreaText();
            }
        }

        public int JigglePeriod
        {
            get => jigglePeriod;
            set
            {
                jigglePeriod = value;
                this.UpdateTimer();
            }
        }

        private void JiggleTimer_Tick(object? sender, EventArgs e)
        {
            if (this.ZenJiggleEnabled)
                Helpers.Jiggle(delta: 0);
            else if (zig)
                Helpers.Jiggle(delta: 4);
            else //zag
                Helpers.Jiggle(delta: -4);

            zig = !zig;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // ReSharper disable once AssignNullToNotNullAttribute - it throws if not found
            taskbarIcon = (TaskbarIcon)this.FindResource("NotifyIcon");

            jiggleTimer = new DispatcherTimer();
            jiggleTimer.Tick += this.JiggleTimer_Tick;

            this.UpdateTimer();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            jiggleTimer?.Stop();
            jiggleTimer = null;
        }
    }
}