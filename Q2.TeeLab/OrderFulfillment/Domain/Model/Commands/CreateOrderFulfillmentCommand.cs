using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Domain.Model.Commands;

public record CreateOrderFulfillmentCommand(
    Guid OrderId,
    UserId CustomerId,
    ManufacturerId ManufacturerId,
    string ProjectName,
    string? ProjectDescription = null,
    string? SpecialInstructions = null
);
