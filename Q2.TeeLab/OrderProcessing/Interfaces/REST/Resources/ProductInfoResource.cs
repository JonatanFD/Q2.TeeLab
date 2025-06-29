namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

public record ProductInfoResource(
    Guid Id,
    Guid ProjectId,
    string Description,
    decimal Price,
    string Currency,
    IEnumerable<DiscountResource> Discounts);
