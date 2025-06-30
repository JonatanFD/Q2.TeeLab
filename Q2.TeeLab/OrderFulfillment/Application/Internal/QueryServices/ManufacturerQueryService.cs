using Q2.TeeLab.OrderFulfillment.Application.Internal.QueryServices;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Aggregates;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Queries;
using Q2.TeeLab.OrderFulfillment.Domain.Repositories;

namespace Q2.TeeLab.OrderFulfillment.Application.Internal.QueryServices;

public class ManufacturerQueryService : IManufacturerQueryService
{
    private readonly IManufacturerRepository manufacturerRepository;

    public ManufacturerQueryService(IManufacturerRepository manufacturerRepository)
    {
        this.manufacturerRepository = manufacturerRepository;
    }

    public async Task<Manufacturer?> Handle(GetManufacturerByIdQuery query)
    {
        return await manufacturerRepository.FindByIdAsync(query.ManufacturerId);
    }

    public async Task<IEnumerable<Manufacturer>> Handle(GetAllActiveManufacturersQuery query)
    {
        return await manufacturerRepository.FindActiveManufacturersAsync();
    }
}
