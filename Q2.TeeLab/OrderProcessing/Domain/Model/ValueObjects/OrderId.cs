namespace Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

public record OrderId(Guid Value)
{
    public OrderId() : this(Guid.NewGuid()) { }
    
    public static implicit operator Guid(OrderId orderId) => orderId.Value;
    public static implicit operator OrderId(Guid value) => new(value);
    
    public override string ToString() => Value.ToString();
}
