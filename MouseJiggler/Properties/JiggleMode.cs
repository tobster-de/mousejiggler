using System.ComponentModel.DataAnnotations;

namespace MouseJiggler.Properties;

public enum JiggleMode
{
    [Display(Name = "Mode_Zen", ResourceType = typeof(XamlResources))]
    Zen,
    [Display(Name = "Mode_ZigZag", ResourceType = typeof(XamlResources))]
    ZigZag,
    [Display(Name = "Mode_Circle", ResourceType = typeof(XamlResources))]
    Circle,
    [Display(Name = "Mode_Smooth", ResourceType = typeof(XamlResources))]
    Smooth
}