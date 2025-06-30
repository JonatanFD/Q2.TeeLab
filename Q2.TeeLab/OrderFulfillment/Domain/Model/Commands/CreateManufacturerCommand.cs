using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Domain.Model.Commands;

public record CreateManufacturerCommand(
    string CompanyName,
    string ContactPersonName,
    string Email,
    string PhoneNumber,
    string TaxIdentificationNumber,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    string? Website = null,
    string? Specialization = null
);
