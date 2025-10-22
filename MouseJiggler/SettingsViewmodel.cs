using CommunityToolkit.Mvvm.ComponentModel;
using MouseJiggler.Properties;

namespace MouseJiggler;

public class SettingsViewmodel : ObservableObject
{
    private int _jiggleInterval = 15;
    private bool _autostartJiggle = true;
    private JiggleMode _jiggleMode = JiggleMode.ZigZag;
    private int _jiggleSize = 20;

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
}