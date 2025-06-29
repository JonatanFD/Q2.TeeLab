using Q2.TeeLab.OrderProcessing.Domain.Model.Queries;
using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;

namespace Q2.TeeLab.OrderProcessing.Application.Internal.QueryServices;

public interface IOrderQueryService
{
    Task<Order?> Handle(GetOrderByIdQuery query);
    Task<IEnumerable<Order>> Handle(GetOrdersByUserIdQuery query);
    Task<IEnumerable<Order>> Handle(GetActiveOrdersByUserIdQuery query);
    Task<IEnumerable<Order>> Handle(GetOrdersByStatusQuery query);
    Task<IEnumerable<Order>> Handle(GetOrderHistoryByUserIdQuery query);
    Task<IEnumerable<Order>> Handle(GetOrdersByDateRangeQuery query);
    Task<IEnumerable<Order>> Handle(SearchOrdersQuery query);
}
