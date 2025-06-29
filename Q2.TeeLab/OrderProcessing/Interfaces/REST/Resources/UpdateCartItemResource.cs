namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

public record UpdateCartItemResource(
    Guid ProductId,
    int Quantity);
