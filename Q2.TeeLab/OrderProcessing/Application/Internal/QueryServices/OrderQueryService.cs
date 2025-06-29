using Q2.TeeLab.OrderProcessing.Application.Internal.QueryServices;
using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Domain.Model.Queries;
using Q2.TeeLab.OrderProcessing.Domain.Repositories;

namespace Q2.TeeLab.OrderProcessing.Application.Internal.QueryServices;

public class OrderQueryService : IOrderQueryService
{
    private readonly IOrderRepository _orderRepository;

    public OrderQueryService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order?> Handle(GetOrderByIdQuery query)
    {
        return await _orderRepository.FindByIdAsync(query.OrderId);
    }

    public async Task<IEnumerable<Order>> Handle(GetOrdersByUserIdQuery query)
    {
        return await _orderRepository.FindByUserIdAsync(query.UserId);
    }

    public async Task<IEnumerable<Order>> Handle(GetActiveOrdersByUserIdQuery query)
    {
        return await _orderRepository.FindActiveOrdersByUserIdAsync(query.UserId);
    }

    public async Task<IEnumerable<Order>> Handle(GetOrdersByStatusQuery query)
    {
        return await _orderRepository.FindByStatusAsync(query.Status);
    }

    public async Task<IEnumerable<Order>> Handle(GetOrderHistoryByUserIdQuery query)
    {
        return await _orderRepository.FindOrderHistoryByUserIdAsync(query.UserId, query.Page, query.PageSize);
    }

    public async Task<IEnumerable<Order>> Handle(GetOrdersByDateRangeQuery query)
    {
        return await _orderRepository.FindByDateRangeAsync(query.StartDate, query.EndDate);
    }

    public async Task<IEnumerable<Order>> Handle(SearchOrdersQuery query)
    {
        return await _orderRepository.SearchOrdersAsync(
            query.SearchTerm,
            query.UserId,
            query.Status,
            query.FromDate,
            query.ToDate,
            query.Page,
            query.PageSize);
    }
}
