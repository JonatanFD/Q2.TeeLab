namespace Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

public record ProductId(Guid Value)
{
    public ProductId() : this(Guid.NewGuid()) { }
    
    public static implicit operator Guid(ProductId productId) => productId.Value;
    public static implicit operator ProductId(Guid value) => new(value);
}
