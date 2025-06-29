using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Domain.Model.Queries;

public record GetOrdersByStatusQuery(OrderStatus Status);
