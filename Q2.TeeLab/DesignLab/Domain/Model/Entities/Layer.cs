using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

namespace Q2.TeeLab.DesignLab.Domain.Model.Entities;

public abstract class Layer
{
    public LayerId Id { get; private set; }
    public ProjectId ProjectId { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Z { get; private set; }
    public ELayerType Type { get; private set; }
    public float Opacity { get; private set; }
    public bool IsVisible { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    
    public Layer(ProjectId projectId, ELayerType type, int z)
    {
        Id = new LayerId();
        ProjectId = projectId;
        X = 0;
        Y = 0;
        Z = z;
        Type = type;
        Opacity = 1.0f;
        IsVisible = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
}