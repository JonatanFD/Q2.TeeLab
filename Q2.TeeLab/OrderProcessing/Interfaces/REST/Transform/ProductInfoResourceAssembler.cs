using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Transform;

public static class ProductInfoResourceAssembler
{
    public static ProductInfoResource ToResource(ProductInfo productInfo)
    {
        return new ProductInfoResource(
            productInfo.Id.Value,
            productInfo.ProjectId.Value,
            productInfo.Description,
            productInfo.Price.Amount,
            productInfo.Price.Currency,
            productInfo.Discounts.Select(DiscountResourceAssembler.ToResource));
    }
}
