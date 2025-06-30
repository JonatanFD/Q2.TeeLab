using Q2.TeeLab.OrderFulfillment.Application.Internal.QueryServices;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Queries;
using Q2.TeeLab.OrderFulfillment.Domain.Repositories;

namespace Q2.TeeLab.OrderFulfillment.Application.Internal.QueryServices;

public class OrderFulfillmentQueryService : IOrderFulfillmentQueryService
{
    private readonly IOrderFulfillmentRepository orderFulfillmentRepository;

    public OrderFulfillmentQueryService(IOrderFulfillmentRepository orderFulfillmentRepository)
    {
        this.orderFulfillmentRepository = orderFulfillmentRepository;
    }

    public async Task<Domain.Model.Aggregates.OrderFulfillment?> Handle(GetOrderFulfillmentByIdQuery query)
    {
        return await orderFulfillmentRepository.FindByIdAsync(query.OrderFulfillmentId);
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.OrderFulfillment>> Handle(GetOrderFulfillmentsByManufacturerQuery query)
    {
        return await orderFulfillmentRepository.FindByManufacturerIdAsync(query.ManufacturerId);
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.OrderFulfillment>> Handle(GetOrderFulfillmentsByStatusQuery query)
    {
        return await orderFulfillmentRepository.FindByStatusAsync(query.Status);
    }
}
