using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Domain.Model.Aggregates;

public class Manufacturer
{
    public ManufacturerId Id { get; private set; }
    public string CompanyName { get; private set; }
    public string ContactPersonName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string TaxIdentificationNumber { get; private set; } // RUC
    public Address Address { get; private set; }
    public string? Website { get; private set; }
    public string? Specialization { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected Manufacturer() 
    {
        Id = null!;
        CompanyName = null!;
        ContactPersonName = null!;
        Email = null!;
        PhoneNumber = null!;
        TaxIdentificationNumber = null!;
        Address = null!;
    }

    public Manufacturer(
        string companyName,
        string contactPersonName,
        string email,
        string phoneNumber,
        string taxIdentificationNumber,
        Address address,
        string? website = null,
        string? specialization = null)
    {
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name is required", nameof(companyName));
        if (string.IsNullOrWhiteSpace(contactPersonName))
            throw new ArgumentException("Contact person name is required", nameof(contactPersonName));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required", nameof(phoneNumber));
        if (string.IsNullOrWhiteSpace(taxIdentificationNumber))
            throw new ArgumentException("Tax identification number is required", nameof(taxIdentificationNumber));

        Id = new ManufacturerId();
        CompanyName = companyName;
        ContactPersonName = contactPersonName;
        Email = email;
        PhoneNumber = phoneNumber;
        TaxIdentificationNumber = taxIdentificationNumber;
        Address = address;
        Website = website;
        Specialization = specialization;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateContactInformation(
        string contactPersonName,
        string email,
        string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(contactPersonName))
            throw new ArgumentException("Contact person name is required", nameof(contactPersonName));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required", nameof(phoneNumber));

        ContactPersonName = contactPersonName;
        Email = email;
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress(Address newAddress)
    {
        Address = newAddress ?? throw new ArgumentNullException(nameof(newAddress));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCompanyDetails(string companyName, string? website = null, string? specialization = null)
    {
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name is required", nameof(companyName));

        CompanyName = companyName;
        Website = website;
        Specialization = specialization;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
