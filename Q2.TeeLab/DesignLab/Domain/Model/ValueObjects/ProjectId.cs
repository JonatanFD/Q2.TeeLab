namespace Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

public record ProjectId(Guid Id)
{
    public ProjectId() : this(Guid.NewGuid())
    {
    }
}
