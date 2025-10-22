using System.ComponentModel.DataAnnotations;

namespace MouseJiggler.Properties;

public enum JiggleMode
{
    [Display(Name = "Zen")]
    Zen,
    [Display(Name = "Zig Zag")]
    ZigZag,
    [Display(Name = "Circle")]
    Circle,
    [Display(Name = "Smooth")]
    Smooth
}