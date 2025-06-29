namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

public record OrderItemResource(
    Guid Id,
    ProductInfoResource Product,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice,
    string Currency,
    IEnumerable<DiscountResource> AppliedDiscounts);
