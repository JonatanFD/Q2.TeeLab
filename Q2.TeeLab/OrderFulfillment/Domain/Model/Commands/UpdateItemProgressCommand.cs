using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Domain.Model.Commands;

public record UpdateItemProgressCommand(
    OrderFulfillmentId OrderFulfillmentId,
    OrderFulfillmentItemId ItemId,
    ItemProgress Progress,
    string? Notes = null
);
