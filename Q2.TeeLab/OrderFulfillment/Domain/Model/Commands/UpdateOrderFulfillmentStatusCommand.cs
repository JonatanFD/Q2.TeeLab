using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Domain.Model.Commands;

public record UpdateOrderFulfillmentStatusCommand(
    OrderFulfillmentId OrderFulfillmentId,
    OrderFulfillmentStatus NewStatus
);
