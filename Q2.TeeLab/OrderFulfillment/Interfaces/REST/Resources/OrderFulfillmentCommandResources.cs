namespace Q2.TeeLab.OrderFulfillment.Interfaces.REST.Resources;

public record CreateOrderFulfillmentResource(
    Guid OrderId,
    Guid CustomerId,
    Guid ManufacturerId,
    string ProjectName,
    string? ProjectDescription = null,
    string? SpecialInstructions = null
);

public record UpdateOrderFulfillmentStatusResource(
    string Status
);

public record UpdateItemProgressResource(
    Guid ItemId,
    string Progress,
    string? Notes = null
);

public record AddOrderFulfillmentItemResource(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);
