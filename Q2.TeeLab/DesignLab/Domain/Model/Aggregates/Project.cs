using Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

namespace Q2.TeeLab.DesignLab.Domain.Model.Aggregates;

public class Project
{
    public ProjectId Id { get; private set; }
    public string Title { get; private set; }
    public EGarmentColor GarmentColor { get; private set; }
    public EGarmentGender GarmentGender { get; private set; }
    public EGarmentSize GarmentSize { get; private set; }
    
    
    
    
}