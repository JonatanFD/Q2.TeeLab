using Q2.TeeLab.OrderFulfillment.Domain.Model.Aggregates;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Queries;

namespace Q2.TeeLab.OrderFulfillment.Application.Internal.QueryServices;

public interface IManufacturerQueryService
{
    Task<Manufacturer?> Handle(GetManufacturerByIdQuery query);
    Task<IEnumerable<Manufacturer>> Handle(GetAllActiveManufacturersQuery query);
}
