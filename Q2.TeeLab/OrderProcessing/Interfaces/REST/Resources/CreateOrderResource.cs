namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

public record CreateOrderResource(
    Guid UserId,
    string? Notes = null);
