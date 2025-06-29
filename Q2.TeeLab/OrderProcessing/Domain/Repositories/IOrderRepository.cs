using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Repositories;

namespace Q2.TeeLab.OrderProcessing.Domain.Repositories;

public interface IOrderRepository : IBaseRepository<Order, OrderId>
{
    Task<IEnumerable<Order>> FindByUserIdAsync(UserId userId);
    Task<IEnumerable<Order>> FindActiveOrdersByUserIdAsync(UserId userId);
    Task<IEnumerable<Order>> FindByStatusAsync(OrderStatus status);
    Task<IEnumerable<Order>> FindByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Order>> FindOrderHistoryByUserIdAsync(UserId userId, int page, int pageSize);
    Task<IEnumerable<Order>> SearchOrdersAsync(
        string? searchTerm = null,
        UserId? userId = null,
        OrderStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int page = 1,
        int pageSize = 10);
    Task<int> CountOrdersByUserIdAsync(UserId userId);
    Task<int> CountOrdersByStatusAsync(OrderStatus status);
}
