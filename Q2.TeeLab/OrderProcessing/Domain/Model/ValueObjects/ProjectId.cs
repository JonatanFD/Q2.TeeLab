namespace Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

public record ProjectId(Guid Value)
{
    public ProjectId() : this(Guid.NewGuid()) { }
    
    public static implicit operator Guid(ProjectId projectId) => projectId.Value;
    public static implicit operator ProjectId(Guid value) => new(value);
    
    public override string ToString() => Value.ToString();
}
