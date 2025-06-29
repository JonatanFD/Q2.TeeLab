namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

public record UpdateOrderStatusResource(
    string Status,
    string? TrackingNumber = null);
