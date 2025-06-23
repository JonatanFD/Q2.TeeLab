namespace Q2.TeeLab.DesignLab.Domain.Model.ValueObjects;

public record LayerId(Guid Id)
{
    public LayerId() : this(Guid.NewGuid())
    {
    }
}