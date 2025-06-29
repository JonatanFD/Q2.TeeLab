using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Domain.Services;

public interface IProductCatalogService
{
    Task<ProductInfo?> GetProductInfoAsync(ProductId productId);
    Task<IEnumerable<ProductInfo>> GetProductsFromProjectAsync(ProjectId projectId);
    Task<bool> IsProductAvailableAsync(ProductId productId);
    Task<bool> IsProjectInGarmentStateAsync(ProjectId projectId);
}
