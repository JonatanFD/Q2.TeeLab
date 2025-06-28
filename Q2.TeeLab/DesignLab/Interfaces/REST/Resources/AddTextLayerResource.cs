using System.ComponentModel.DataAnnotations;

namespace Q2.TeeLab.DesignLab.Interfaces.REST.Resources;

public class AddTextLayerResource
{
    [Required(ErrorMessage = "Text is required")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "Text must be between 1 and 500 characters")]
    public string Text { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Font family is required")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Font family must be a valid number")]
    public string FontFamily { get; set; } = string.Empty;
    
    [Range(8, 72, ErrorMessage = "Font size must be between 8 and 72")]
    public int FontSize { get; set; }
    
    [Required(ErrorMessage = "Font color is required")]
    public string FontColor { get; set; } = string.Empty;
    
    public bool IsBold { get; set; }
    public bool IsUnderlined { get; set; }
    public bool IsItalic { get; set; }
}
