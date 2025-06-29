using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Transform;

public static class CartResourceAssembler
{
    public static CartResource ToResource(Cart cart)
    {
        return new CartResource(
            cart.Id.Value,
            cart.UserId.Value,
            cart.CreatedAt,
            cart.LastUpdated,
            cart.TotalAmount.Amount,
            cart.TotalAmount.Currency,
            cart.Items.Count(),
            cart.Items.Select(OrderItemResourceAssembler.ToResource));
    }
}
