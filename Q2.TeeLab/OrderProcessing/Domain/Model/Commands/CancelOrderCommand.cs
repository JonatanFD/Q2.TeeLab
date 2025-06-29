using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Domain.Model.Commands;

public record CancelOrderCommand(
    OrderId OrderId,
    string? Reason = null);
