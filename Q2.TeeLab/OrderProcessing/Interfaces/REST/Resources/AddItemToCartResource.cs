namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

public record AddItemToCartResource(
    Guid ProductId,
    int Quantity = 1);
