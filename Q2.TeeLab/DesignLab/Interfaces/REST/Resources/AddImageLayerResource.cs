using System.ComponentModel.DataAnnotations;

namespace Q2.TeeLab.DesignLab.Interfaces.REST.Resources;

public class AddImageLayerResource
{
    [Required(ErrorMessage = "Image URL is required")]
    [Url(ErrorMessage = "Image URL must be a valid URL")]
    public string ImageUrl { get; set; } = string.Empty;
    
    [Range(0.1, float.MaxValue, ErrorMessage = "Width must be greater than 0")]
    public float Width { get; set; }
    
    [Range(0.1, float.MaxValue, ErrorMessage = "Height must be greater than 0")]
    public float Height { get; set; }
}
