using Q2.TeeLab.OrderFulfillment.Application.Internal.CommandServices;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Aggregates;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Commands;
using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderFulfillment.Domain.Repositories;

namespace Q2.TeeLab.OrderFulfillment.Application.Internal.CommandServices;

public class ManufacturerCommandService : IManufacturerCommandService
{
    private readonly IManufacturerRepository manufacturerRepository;

    public ManufacturerCommandService(IManufacturerRepository manufacturerRepository)
    {
        this.manufacturerRepository = manufacturerRepository;
    }

    public async Task<Guid> Handle(CreateManufacturerCommand command)
    {
        // Check if manufacturer with same tax identification number already exists
        var existingManufacturer = await manufacturerRepository.FindByTaxIdentificationNumberAsync(command.TaxIdentificationNumber);
        if (existingManufacturer != null)
            throw new InvalidOperationException("Manufacturer with this tax identification number already exists");

        // Check if manufacturer with same email already exists
        var existingEmail = await manufacturerRepository.FindByEmailAsync(command.Email);
        if (existingEmail != null)
            throw new InvalidOperationException("Manufacturer with this email already exists");

        var address = new Address(
            command.Street,
            command.City,
            command.State,
            command.PostalCode,
            command.Country);

        var manufacturer = new Manufacturer(
            command.CompanyName,
            command.ContactPersonName,
            command.Email,
            command.PhoneNumber,
            command.TaxIdentificationNumber,
            address,
            command.Website,
            command.Specialization);

        await manufacturerRepository.SaveAsync(manufacturer);
        return manufacturer.Id;
    }
}
