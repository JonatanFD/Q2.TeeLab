using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Domain.Model.Queries;

public record GetActiveOrdersByUserIdQuery(UserId UserId);
