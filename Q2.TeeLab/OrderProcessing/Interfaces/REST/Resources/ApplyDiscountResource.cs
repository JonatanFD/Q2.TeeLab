namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

public record ApplyDiscountResource(
    string Name,
    string Type, // "Percentage" or "FixedAmount"
    decimal Value,
    DateTime? ValidFrom = null,
    DateTime? ValidTo = null);
