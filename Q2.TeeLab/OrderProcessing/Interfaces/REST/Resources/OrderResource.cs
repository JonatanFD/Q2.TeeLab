namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

public record OrderResource(
    Guid Id,
    Guid UserId,
    DateTime OrderDate,
    string Status,
    decimal TotalAmount,
    decimal DiscountAmount,
    decimal FinalAmount,
    string Currency,
    string? Notes,
    DateTime? DeliveryDate,
    string? TrackingNumber,
    IEnumerable<OrderItemResource> Items);
