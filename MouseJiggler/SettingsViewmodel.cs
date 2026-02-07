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
    private bool _checkActivity = false;

    internal void LoadSettings()
    {
        this.AutostartJiggle = Settings.Default.AutostartJiggle;
        this.JiggleInterval = Settings.Default.JiggleInterval;
        this.JiggleMode = Settings.Default.JiggleMode;
        this.JiggleSize = Settings.Default.JiggleSize;
        this.CheckActivity = Settings.Default.CheckActivity;
    }

    internal void SaveSettings()
    {
        Settings.Default.AutostartJiggle = this.AutostartJiggle;
        Settings.Default.JiggleInterval = this.JiggleInterval;
        Settings.Default.JiggleMode = this.JiggleMode;
        Settings.Default.JiggleSize = this.JiggleSize;
        Settings.Default.CheckActivity = this.CheckActivity;

        Settings.Default.Save();
    }
}