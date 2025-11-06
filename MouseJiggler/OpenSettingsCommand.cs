using MouseJiggler.Properties;
using System.Windows;
using System.Windows.Input;

namespace MouseJiggler;

class OpenSettingsCommand : ICommand
{
    static SettingsWindow? _settings;

    public SettingsWindow? SettingsWindow
    {
        get => _settings;
        set
        {
            _settings = value;
            this.CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => this.SettingsWindow == null;

    public void Execute(object? parameter)
    {
        try
        {
            this.SettingsWindow = new SettingsWindow();

            if (this.SettingsWindow.ShowDialog().GetValueOrDefault())
            {
                this.SettingsWindow.ViewModel.SaveSettings();

                ((App)Application.Current).ApplySettings();
            }
        }
        finally
        {
            this.SettingsWindow = null;
        }
    }
}