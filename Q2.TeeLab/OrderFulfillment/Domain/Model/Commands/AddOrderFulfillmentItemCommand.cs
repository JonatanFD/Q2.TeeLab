using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Domain.Model.Commands;

public record AddOrderFulfillmentItemCommand(
    OrderFulfillmentId OrderFulfillmentId,
    ProductId ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);
