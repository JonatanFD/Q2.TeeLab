using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Transform;

public static class DiscountResourceAssembler
{
    public static DiscountResource ToResource(Discount discount)
    {
        return new DiscountResource(
            discount.Name,
            discount.Type.ToString(),
            discount.Value,
            discount.ValidFrom,
            discount.ValidTo);
    }
}
