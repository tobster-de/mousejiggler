using MouseJiggler;
using MouseJiggler.Properties;
using System.Windows;
using System.Windows.Input;

namespace MouseJiggler
{
    class OpenSettingsCommand : ICommand
    {
        static SettingsWindow? _settings;

        public SettingsWindow? SettingsWindow
        {
            get => _settings;
            set
            {
                _settings = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => SettingsWindow == null;

        public void Execute(object? parameter)
        {
            try
            {
                SettingsWindow = new SettingsWindow();

                SettingsWindow.ViewModel.AutostartJiggle = Settings.Default.AutostartJiggle;
                SettingsWindow.ViewModel.JiggleInterval = Settings.Default.JiggleInterval;
                SettingsWindow.ViewModel.ZenJiggle = Settings.Default.ZenJiggle;

                if (SettingsWindow.ShowDialog().GetValueOrDefault())
                {
                    Settings.Default.AutostartJiggle = SettingsWindow.ViewModel.AutostartJiggle;
                    Settings.Default.JiggleInterval = SettingsWindow.ViewModel.JiggleInterval;
                    Settings.Default.ZenJiggle = SettingsWindow.ViewModel.ZenJiggle;
                    Settings.Default.Save();

                    App app = (App)Application.Current;
                    app.JigglePeriod = Settings.Default.JiggleInterval;
                    app.ZenJiggleEnabled = Settings.Default.ZenJiggle;
                }
            }
            finally
            {
                SettingsWindow = null;
            }
        }
    }
}
