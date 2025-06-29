namespace Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

public enum DiscountType
{
    Percentage,
    FixedAmount
}

public record Discount(
    string Name,
    DiscountType Type,
    decimal Value,
    DateTime? ValidFrom = null,
    DateTime? ValidTo = null)
{
    public bool IsValid(DateTime date) => 
        (ValidFrom == null || date >= ValidFrom) && 
        (ValidTo == null || date <= ValidTo);

    public decimal CalculateDiscountAmount(decimal baseAmount) => Type switch
    {
        DiscountType.Percentage => baseAmount * Value / 100,
        DiscountType.FixedAmount => Value,
        _ => 0
    };
}
