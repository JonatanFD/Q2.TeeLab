using Q2.TeeLab.OrderProcessing.Domain.Model.Entities;
using Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Transform;

public static class OrderItemResourceAssembler
{
    public static OrderItemResource ToResource(OrderItem orderItem)
    {
        return new OrderItemResource(
            orderItem.Id.Value,
            ProductInfoResourceAssembler.ToResource(orderItem.Product),
            orderItem.Quantity,
            orderItem.UnitPrice.Amount,
            orderItem.TotalPrice.Amount,
            orderItem.UnitPrice.Currency,
            orderItem.AppliedDiscounts.Select(DiscountResourceAssembler.ToResource));
    }
}
