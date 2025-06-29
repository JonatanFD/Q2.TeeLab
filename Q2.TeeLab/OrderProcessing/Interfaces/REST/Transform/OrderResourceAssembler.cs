using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Transform;

public static class OrderResourceAssembler
{
    public static OrderResource ToResource(Order order)
    {
        return new OrderResource(
            order.Id.Value,
            order.UserId.Value,
            order.OrderDate,
            order.Status.ToString(),
            order.TotalAmount.Amount,
            order.DiscountAmount.Amount,
            order.FinalAmount.Amount,
            order.TotalAmount.Currency,
            order.Notes,
            order.DeliveryDate,
            order.TrackingNumber,
            order.Items.Select(OrderItemResourceAssembler.ToResource));
    }

    public static OrderSummaryResource ToSummaryResource(Order order)
    {
        return new OrderSummaryResource(
            order.Id.Value,
            order.OrderDate,
            order.Status.ToString(),
            order.FinalAmount.Amount,
            order.FinalAmount.Currency,
            order.Items.Count());
    }
}
