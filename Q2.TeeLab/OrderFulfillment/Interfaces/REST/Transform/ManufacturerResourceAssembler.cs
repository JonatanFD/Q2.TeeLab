using Q2.TeeLab.OrderFulfillment.Domain.Model.Aggregates;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Commands;
using Q2.TeeLab.OrderFulfillment.Interfaces.REST.Resources;

namespace Q2.TeeLab.OrderFulfillment.Interfaces.REST.Transform;

public static class ManufacturerResourceAssembler
{
    public static ManufacturerResource ToResource(Manufacturer manufacturer)
    {
        return new ManufacturerResource(
            manufacturer.Id,
            manufacturer.CompanyName,
            manufacturer.ContactPersonName,
            manufacturer.Email,
            manufacturer.PhoneNumber,
            manufacturer.TaxIdentificationNumber,
            new AddressResource(
                manufacturer.Address.Street,
                manufacturer.Address.City,
                manufacturer.Address.State,
                manufacturer.Address.PostalCode,
                manufacturer.Address.Country
            ),
            manufacturer.Website,
            manufacturer.Specialization,
            manufacturer.IsActive,
            manufacturer.CreatedAt,
            manufacturer.UpdatedAt
        );
    }

    public static CreateManufacturerCommand ToCommand(CreateManufacturerResource resource)
    {
        return new CreateManufacturerCommand(
            resource.CompanyName,
            resource.ContactPersonName,
            resource.Email,
            resource.PhoneNumber,
            resource.TaxIdentificationNumber,
            resource.Street,
            resource.City,
            resource.State,
            resource.PostalCode,
            resource.Country,
            resource.Website,
            resource.Specialization
        );
    }
}
