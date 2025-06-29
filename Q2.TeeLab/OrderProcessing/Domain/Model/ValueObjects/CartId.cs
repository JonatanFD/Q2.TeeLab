namespace Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

public record CartId(Guid Value)
{
    public CartId() : this(Guid.NewGuid()) { }
    
    public static implicit operator Guid(CartId cartId) => cartId.Value;
    public static implicit operator CartId(Guid value) => new(value);
    
    public override string ToString() => Value.ToString();
}
