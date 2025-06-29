namespace Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

public enum OrderStatus
{
    Draft,
    Pending,
    Confirmed,
    Processing,
    Shipped,
    Delivered,
    Cancelled,
    Refunded
}

public static class OrderStatusExtensions
{
    public static bool CanTransitionTo(this OrderStatus current, OrderStatus target) => current switch
    {
        OrderStatus.Draft => target is OrderStatus.Pending or OrderStatus.Cancelled,
        OrderStatus.Pending => target is OrderStatus.Confirmed or OrderStatus.Cancelled,
        OrderStatus.Confirmed => target is OrderStatus.Processing or OrderStatus.Cancelled,
        OrderStatus.Processing => target is OrderStatus.Shipped or OrderStatus.Cancelled,
        OrderStatus.Shipped => target is OrderStatus.Delivered,
        OrderStatus.Delivered => target is OrderStatus.Refunded,
        OrderStatus.Cancelled => false,
        OrderStatus.Refunded => false,
        _ => false
    };
    
    public static bool IsActive(this OrderStatus status) => status switch
    {
        OrderStatus.Draft or OrderStatus.Pending or OrderStatus.Confirmed 
        or OrderStatus.Processing or OrderStatus.Shipped => true,
        _ => false
    };
    
    public static bool IsCompleted(this OrderStatus status) => status switch
    {
        OrderStatus.Delivered or OrderStatus.Cancelled or OrderStatus.Refunded => true,
        _ => false
    };
}
