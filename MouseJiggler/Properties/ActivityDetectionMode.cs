using System.ComponentModel.DataAnnotations;

namespace MouseJiggler.Properties;

public enum ActivityDetectionMode
{
    [Display(Name = "ActivityDetection_Off", ResourceType = typeof(XamlResources))]
    Off,

    [Display(Name = "ActivityDetection_MouseMovement", ResourceType = typeof(XamlResources))]
    MouseMovement,

    [Display(Name = "ActivityDetection_WinApiIdleTime", ResourceType = typeof(XamlResources))]
    WinApiIdleTime,
}

