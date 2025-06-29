namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

public record OrderSummaryResource(
    Guid Id,
    DateTime OrderDate,
    string Status,
    decimal FinalAmount,
    string Currency,
    int TotalItems);
