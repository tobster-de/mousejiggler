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
        private TaskbarIcon taskbarIcon;

        public bool JiggleActive
        {
            get => this.jiggleActive;
            set
            {
                this.jiggleActive = value;
                this.UpdateTimer();
            }
        }

        private void UpdateTimer()
        {
            if (jiggleTimer == null)
            {
                return;
            }

            this.jiggleTimer.Stop();
            this.jiggleTimer.Interval = TimeSpan.FromSeconds(jigglePeriod);

            if (jiggleActive)
            {
                this.jiggleTimer.Start();
            }

            this.UpdateNotificationAreaText();
        }

        private void UpdateNotificationAreaText()
        {
            if (jiggleActive)
            {
                string ww = this.ZenJiggleEnabled ? "with" : "without";
                this.taskbarIcon.ToolTipText = $"Jiggling mouse every {this.JigglePeriod} s, {ww} Zen.";
            }
            else
            {
                this.taskbarIcon.ToolTipText = "Not jiggling the mouse.";
            }
        }

        public bool ZenJiggleEnabled
        {
            get => this.zenJiggleEnabled;
            set => this.zenJiggleEnabled = value;
        }

        public int JigglePeriod
        {
            get => this.jigglePeriod;
            set 
            {
                this.jigglePeriod = value;
                this.UpdateTimer();
            }
        }

        private void JiggleTimer_Tick(object? sender, EventArgs e)
        {
            if (this.ZenJiggleEnabled)
                Helpers.Jiggle(delta: 0);
            else if (this.zig)
                Helpers.Jiggle(delta: 4);
            else //zag
                Helpers.Jiggle(delta: -4);

            this.zig = !this.zig;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            taskbarIcon = (TaskbarIcon)FindResource("NotifyIcon");

            jiggleTimer = new DispatcherTimer();
            jiggleTimer.Tick += JiggleTimer_Tick;

            this.UpdateTimer();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            jiggleTimer?.Stop();
            jiggleTimer = null;
        }
    }

}
