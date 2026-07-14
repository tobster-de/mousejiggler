using CommunityToolkit.Mvvm.ComponentModel;
using MouseJiggler.Properties;

namespace MouseJiggler;

public partial class SettingsViewmodel : ObservableObject
{
    [ObservableProperty]
    private int _jiggleInterval = 15;
    
    [ObservableProperty]
    private bool _autostartJiggle = true;
    
    [ObservableProperty]
    private JiggleMode _jiggleMode = JiggleMode.ZigZag;
    
    [ObservableProperty]
    private int _jiggleSize = 20;
    
    [ObservableProperty]
    private ActivityDetectionMode _activityDetectionMode = ActivityDetectionMode.Off;

    internal void LoadSettings()
    {
        this.AutostartJiggle = Settings.Default.AutostartJiggle;
        this.JiggleInterval = Settings.Default.JiggleInterval;
        this.JiggleMode = Settings.Default.JiggleMode;
        this.JiggleSize = Settings.Default.JiggleSize;
        this.ActivityDetectionMode = Settings.Default.ActivityDetectionMode;
    }

    internal void SaveSettings()
    {
        Settings.Default.AutostartJiggle = this.AutostartJiggle;
        Settings.Default.JiggleInterval = this.JiggleInterval;
        Settings.Default.JiggleMode = this.JiggleMode;
        Settings.Default.JiggleSize = this.JiggleSize;
        Settings.Default.ActivityDetectionMode = this.ActivityDetectionMode;

        Settings.Default.Save();
    }
}