namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

public record DiscountResource(
    string Name,
    string Type,
    decimal Value,
    DateTime? ValidFrom,
    DateTime? ValidTo);
