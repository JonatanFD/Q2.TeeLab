using System.ComponentModel.DataAnnotations;

namespace Q2.TeeLab.DesignLab.Interfaces.REST.Resources;

public class CreateProjectResource
{
    [Required(ErrorMessage = "User ID is required")]
    public Guid UserId { get; set; }
    
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Garment color is required")]
    public string GarmentColor { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Garment gender is required")]
    public string GarmentGender { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Garment size is required")]
    public string GarmentSize { get; set; } = string.Empty;
}