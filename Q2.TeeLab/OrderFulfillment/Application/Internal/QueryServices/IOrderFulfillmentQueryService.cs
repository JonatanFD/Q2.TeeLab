using Q2.TeeLab.OrderFulfillment.Domain.Model.Queries;

namespace Q2.TeeLab.OrderFulfillment.Application.Internal.QueryServices;

public interface IOrderFulfillmentQueryService
{
    Task<Domain.Model.Aggregates.OrderFulfillment?> Handle(GetOrderFulfillmentByIdQuery query);
    Task<IEnumerable<Domain.Model.Aggregates.OrderFulfillment>> Handle(GetOrderFulfillmentsByManufacturerQuery query);
    Task<IEnumerable<Domain.Model.Aggregates.OrderFulfillment>> Handle(GetOrderFulfillmentsByStatusQuery query);
}