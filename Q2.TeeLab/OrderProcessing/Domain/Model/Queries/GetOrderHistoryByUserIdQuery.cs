using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Domain.Model.Queries;

public record GetOrderHistoryByUserIdQuery(UserId UserId, int Page = 1, int PageSize = 10);
