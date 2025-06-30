namespace Q2.TeeLab.OrderFulfillment.Interfaces.REST.Resources;

public record CreateManufacturerResource(
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
