using Q2.TeeLab.OrderFulfillment.Domain.Model.Aggregates;
using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Domain.Repositories;

public interface IManufacturerRepository
{
    Task<Manufacturer?> FindByIdAsync(ManufacturerId id);
    Task<IEnumerable<Manufacturer>> FindAllAsync();
    Task<IEnumerable<Manufacturer>> FindActiveManufacturersAsync();
    Task<Manufacturer?> FindByTaxIdentificationNumberAsync(string taxIdentificationNumber);
    Task<Manufacturer?> FindByEmailAsync(string email);
    Task<IEnumerable<Manufacturer>> FindBySpecializationAsync(string specialization);
    Task SaveAsync(Manufacturer manufacturer);
    Task DeleteAsync(ManufacturerId id);
}
