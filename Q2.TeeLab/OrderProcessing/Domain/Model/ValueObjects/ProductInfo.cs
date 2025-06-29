namespace Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

public record ProductId(Guid Value)
{
    public ProductId() : this(Guid.NewGuid()) { }
    
    public static implicit operator Guid(ProductId productId) => productId.Value;
    public static implicit operator ProductId(Guid value) => new(value);
    
    public override string ToString() => Value.ToString();
}

public record ProductInfo(
    ProductId Id,
    ProjectId ProjectId,
    string Description,
    Money Price,
    IReadOnlyList<Discount> Discounts)
{
    public Money CalculateFinalPrice()
    {
        var totalDiscount = Discounts.Aggregate(decimal.Zero, (acc, discount) =>
            discount.Type switch
            {
                DiscountType.Percentage => acc + (Price.Amount * discount.Value / 100),
                DiscountType.FixedAmount => acc + discount.Value,
                _ => acc
            });

        return new Money(Math.Max(0, Price.Amount - totalDiscount), Price.Currency);
    }
}
