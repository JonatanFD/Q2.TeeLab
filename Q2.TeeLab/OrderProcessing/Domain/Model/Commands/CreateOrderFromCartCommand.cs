using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Domain.Model.Commands;

public record CreateOrderFromCartCommand(
    UserId UserId,
    CartId CartId,
    string? Notes = null);
