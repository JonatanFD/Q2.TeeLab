namespace Q2.TeeLab.DesignLab.Interfaces.REST.Resources;

public class ProjectResource
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? PreviewImageUrl { get; set; }
    public string GarmentColor { get; set; } = string.Empty;
    public string GarmentGender { get; set; } = string.Empty;
    public string GarmentSize { get; set; } = string.Empty;
    public IEnumerable<LayerResource> Layers { get; set; } = new List<LayerResource>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}