namespace Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

public record Address(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country)
{
    public string GetFullAddress() => $"{Street}, {City}, {State} {PostalCode}, {Country}";
}
