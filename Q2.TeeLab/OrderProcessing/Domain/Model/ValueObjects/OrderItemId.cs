namespace Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

public record OrderItemId(Guid Value)
{
    public OrderItemId() : this(Guid.NewGuid()) { }
    
    public static implicit operator Guid(OrderItemId orderItemId) => orderItemId.Value;
    public static implicit operator OrderItemId(Guid value) => new(value);
    
    public override string ToString() => Value.ToString();
}
