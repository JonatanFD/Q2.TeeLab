using Q2.TeeLab.DesignLab.Domain.Model.Entities;
using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

namespace Q2.TeeLab.DesignLab.Domain.Model.Aggregates;

public class Project
{
    public ProjectId Id { get; private set; }
    public UserId UserId { get; private set; }
    
    public string Title { get; private set; }
    public Uri PreviewImageUrl { get; private set; }
    
    public EGarmentColor GarmentColor { get; private set; }
    public EGarmentGender GarmentGender { get; private set; }
    public EGarmentSize GarmentSize { get; private set; }

    public ICollection<Layer> Layers { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    public Project(UserId userId, string title, EGarmentColor garmentColor, EGarmentGender garmentGender, EGarmentSize garmentSize)
    {
        Id = new ProjectId();
        UserId = userId;
        Title = title;
        GarmentColor = garmentColor;
        GarmentGender = garmentGender;
        GarmentSize = garmentSize;
        
        Layers = new List<Layer>();
        
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

}