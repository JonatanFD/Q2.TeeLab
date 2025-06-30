namespace Q2.TeeLab.OrderFulfillment.Interfaces.REST.Resources;

public record ManufacturerResource(
    Guid Id,
    string CompanyName,
    string ContactPersonName,
    string Email,
    string PhoneNumber,
    string TaxIdentificationNumber,
    AddressResource Address,
    string? Website,
    string? Specialization,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record AddressResource(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country
);
