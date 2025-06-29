using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderProcessing.Domain.Services;

namespace Q2.TeeLab.OrderProcessing.Infrastructure.Services;

public class ProductCatalogService : IProductCatalogService
{
    public async Task<ProductInfo?> GetProductInfoAsync(ProductId productId)
    {
        // Mock implementation - in real scenario this would query the DesignLab context
        // or an external product catalog service
        
        var mockProduct = new ProductInfo(
            productId,
            new ProjectId(Guid.NewGuid()),
            $"Product {productId}",
            new Money(29.99m),
            new List<Discount>
            {
                new("Early Bird", DiscountType.Percentage, 15, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(10))
            });

        return await Task.FromResult(mockProduct);
    }

    public async Task<IEnumerable<ProductInfo>> GetProductsFromProjectAsync(ProjectId projectId)
    {
        // Mock implementation - in real scenario this would query the DesignLab context
        var mockProducts = new List<ProductInfo>
        {
            new(new ProductId(), projectId, $"T-Shirt from Project {projectId}", new Money(24.99m), new List<Discount>()),
            new(new ProductId(), projectId, $"Hoodie from Project {projectId}", new Money(39.99m), new List<Discount>())
        };

        return await Task.FromResult(mockProducts);
    }

    public async Task<bool> IsProductAvailableAsync(ProductId productId)
    {
        // Mock implementation - in real scenario this would check inventory
        return await Task.FromResult(true);
    }

    public async Task<bool> IsProjectInGarmentStateAsync(ProjectId projectId)
    {
        // Mock implementation - in real scenario this would query the DesignLab context
        // to check if the project is in "Garment" state
        return await Task.FromResult(true);
    }
}
