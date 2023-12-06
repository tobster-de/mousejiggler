using CommunityToolkit.Mvvm.ComponentModel;

namespace MouseJiggler;

public class SettingsViewmodel : ObservableObject
{
    private int jiggleInterval = 15;
    private bool startMinimized;
    private bool autostartJiggle = true;
    private bool zenJiggle;

    public int JiggleInterval
    {
        get => jiggleInterval;
        set => SetProperty(ref jiggleInterval, value);
    }

    public bool StartMinimized
    {
        get => startMinimized;
        set => SetProperty(ref startMinimized, value);
    }

    public bool AutostartJiggle
    {
        get => autostartJiggle;
        set => SetProperty(ref autostartJiggle, value);
    }

    public bool ZenJiggle
    {
        get => zenJiggle;
        set => SetProperty(ref zenJiggle, value);
    }
}
