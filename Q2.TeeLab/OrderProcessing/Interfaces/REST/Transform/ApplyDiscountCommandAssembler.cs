using Q2.TeeLab.OrderProcessing.Domain.Model.Commands;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Transform;

public static class ApplyDiscountCommandAssembler
{
    public static ApplyDiscountToCartCommand ToCommand(UserId userId, ApplyDiscountResource resource)
    {
        var discount = new Discount(
            resource.Name,
            resource.Type == "Percentage" ? DiscountType.Percentage : DiscountType.FixedAmount,
            resource.Value,
            resource.ValidFrom,
            resource.ValidTo);

        return new ApplyDiscountToCartCommand(userId, discount);
    }

    public static ApplyDiscountToOrderCommand ToCommand(Domain.Model.ValueObjects.OrderId orderId, ApplyDiscountResource resource)
    {
        var discount = new Discount(
            resource.Name,
            resource.Type == "Percentage" ? DiscountType.Percentage : DiscountType.FixedAmount,
            resource.Value,
            resource.ValidFrom,
            resource.ValidTo);

        return new ApplyDiscountToOrderCommand(orderId, discount);
    }
}
