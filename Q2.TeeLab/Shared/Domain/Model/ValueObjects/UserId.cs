namespace Q2.TeeLab.Shared.Domain.Model.ValueObjects;

public record UserId(Guid Value)
{
    public UserId() : this(Guid.NewGuid()) { }
    
    public static implicit operator Guid(UserId userId) => userId.Value;
    public static implicit operator UserId(Guid value) => new(value);
    
    public override string ToString() => Value.ToString();
}
