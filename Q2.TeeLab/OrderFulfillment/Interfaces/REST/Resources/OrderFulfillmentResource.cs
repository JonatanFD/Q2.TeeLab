namespace Q2.TeeLab.OrderFulfillment.Interfaces.REST.Resources;

public record OrderFulfillmentResource(
    Guid Id,
    Guid OrderId,
    Guid CustomerId,
    Guid ManufacturerId,
    string ProjectName,
    string? ProjectDescription,
    string Status,
    DateTime OrderDate,
    DateTime? EstimatedDeliveryDate,
    DateTime? ActualDeliveryDate,
    decimal TotalAmount,
    string? SpecialInstructions,
    double CompletionPercentage,
    IEnumerable<OrderFulfillmentItemResource> Items,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record OrderFulfillmentItemResource(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    string Progress,
    string? ProgressNotes,
    DateTime? StartDate,
    DateTime? EstimatedCompletionDate,
    DateTime? ActualCompletionDate,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
