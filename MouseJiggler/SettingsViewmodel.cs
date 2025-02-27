using CommunityToolkit.Mvvm.ComponentModel;
using MouseJiggler.Properties;

namespace MouseJiggler;

public class SettingsViewmodel : ObservableObject
{
    private int _jiggleInterval = 15;
    private bool _startMinimized;
    private bool _autostartJiggle = true;
    private JiggleMode _jiggleMode = JiggleMode.ZigZag;

    public int JiggleInterval
    {
        get => _jiggleInterval;
        set => this.SetProperty(ref _jiggleInterval, value);
    }

    public bool StartMinimized
    {
        get => _startMinimized;
        set => this.SetProperty(ref _startMinimized, value);
    }

    public bool AutostartJiggle
    {
        get => _autostartJiggle;
        set => this.SetProperty(ref _autostartJiggle, value);
    }

    public bool JiggleModeZen
    {
        get => _jiggleMode == JiggleMode.Zen;
        set 
        {
            if (value) this.SetProperty(ref _jiggleMode, JiggleMode.Zen);
        }
    }

    public bool JiggleModeJump
    {
        get => _jiggleMode == JiggleMode.ZigZag;
        set
        {
            if (value) this.SetProperty(ref _jiggleMode, JiggleMode.ZigZag);
        }
    }

    public bool JiggleModeCircle
    {
        get => _jiggleMode == JiggleMode.Circle;
        set
        {
            if (value) this.SetProperty(ref _jiggleMode, JiggleMode.Circle);
        }
    }

    public JiggleMode JiggleMode
    {
        get => _jiggleMode;
        set
        {
            if (this.SetProperty(ref _jiggleMode, value))
            {
                this.OnPropertyChanged(nameof(this.JiggleModeJump));
                this.OnPropertyChanged(nameof(this.JiggleModeCircle));
            }
        }
    }
}
