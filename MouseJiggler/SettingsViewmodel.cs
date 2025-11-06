using CommunityToolkit.Mvvm.ComponentModel;
using MouseJiggler.Properties;

namespace MouseJiggler;

public class SettingsViewmodel : ObservableObject
{
    private int _jiggleInterval = 15;
    private bool _autostartJiggle = true;
    private JiggleMode _jiggleMode = JiggleMode.ZigZag;
    private int _jiggleSize = 20;
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
    
    public int JiggleInterval
    {
        get => _jiggleInterval;
        set => this.SetProperty(ref _jiggleInterval, value);
    }

    public bool AutostartJiggle
    {
        get => _autostartJiggle;
        set => this.SetProperty(ref _autostartJiggle, value);
    }

    public int JiggleSize
    {
        get => _jiggleSize;
        set => this.SetProperty(ref _jiggleSize, value);
    }

    public JiggleMode JiggleMode
    {
        get => _jiggleMode;
        set => this.SetProperty(ref _jiggleMode, value);
    }

    public bool CheckActivity
    {
        get => _checkActivity; 
        set => this.SetProperty(ref _checkActivity, value);
    }
}