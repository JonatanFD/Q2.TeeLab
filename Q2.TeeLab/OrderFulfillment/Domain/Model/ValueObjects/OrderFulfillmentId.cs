namespace Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

public record OrderFulfillmentId(Guid Value)
{
    public OrderFulfillmentId() : this(Guid.NewGuid()) { }
    
    public static implicit operator Guid(OrderFulfillmentId orderFulfillmentId) => orderFulfillmentId.Value;
    public static implicit operator OrderFulfillmentId(Guid value) => new(value);
}
