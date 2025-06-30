using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Domain.Model.Queries;

public record GetOrderFulfillmentsByStatusQuery(OrderFulfillmentStatus Status);
