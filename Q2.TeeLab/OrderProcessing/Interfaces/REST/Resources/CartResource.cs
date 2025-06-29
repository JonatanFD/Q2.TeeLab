namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

public record CartResource(
    Guid Id,
    Guid UserId,
    DateTime CreatedAt,
    DateTime LastUpdated,
    decimal TotalAmount,
    string Currency,
    int TotalItemsCount,
    IEnumerable<OrderItemResource> Items);
