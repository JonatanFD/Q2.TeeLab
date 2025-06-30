namespace Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

public record ManufacturerId(Guid Value)
{
    public ManufacturerId() : this(Guid.NewGuid()) { }
    
    public static implicit operator Guid(ManufacturerId manufacturerId) => manufacturerId.Value;
    public static implicit operator ManufacturerId(Guid value) => new(value);
}
