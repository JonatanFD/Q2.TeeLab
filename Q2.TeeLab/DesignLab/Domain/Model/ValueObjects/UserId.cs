namespace Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

public record UserId(Guid Id)
{
    public UserId() : this(Guid.NewGuid())
    {
    }
}