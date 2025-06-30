namespace Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

public record OrderFulfillmentItemId(Guid Value)
{
    public OrderFulfillmentItemId() : this(Guid.NewGuid()) { }
    
    public static implicit operator Guid(OrderFulfillmentItemId orderFulfillmentItemId) => orderFulfillmentItemId.Value;
    public static implicit operator OrderFulfillmentItemId(Guid value) => new(value);
}
