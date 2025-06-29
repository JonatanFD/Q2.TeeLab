using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Domain.Model.Entities;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderProcessing.Domain.Services;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Infrastructure.Services;

public class OrderManagementService : IOrderManagementService
{
    private readonly IProductCatalogService _productCatalogService;

    public OrderManagementService(IProductCatalogService productCatalogService)
    {
        _productCatalogService = productCatalogService;
    }

    public async Task<bool> ValidateOrderItemsAsync(IEnumerable<OrderItem> items)
    {
        foreach (var item in items)
        {
            var isAvailable = await _productCatalogService.IsProductAvailableAsync(item.Product.Id);
            if (!isAvailable)
                return false;

            var isProjectInGarmentState = await _productCatalogService.IsProjectInGarmentStateAsync(item.Product.ProjectId);
            if (!isProjectInGarmentState)
                return false;
        }

        return true;
    }

    public async Task<Money> CalculateShippingCostAsync(Order order, string shippingAddress)
    {
        // Simple shipping cost calculation - can be enhanced based on business rules
        var baseCost = new Money(10.00m); // Base shipping cost
        var itemCount = order.TotalItemsCount;
        
        // Add $2 per additional item after the first 3
        if (itemCount > 3)
        {
            var additionalCost = new Money((itemCount - 3) * 2.00m);
            baseCost = baseCost + additionalCost;
        }

        return await Task.FromResult(baseCost);
    }

    public async Task<IEnumerable<Discount>> GetAvailableDiscountsAsync(UserId userId)
    {
        // Mock implementation - in real scenario this would query a discount service
        var discounts = new List<Discount>
        {
            new("First Time Customer", DiscountType.Percentage, 10, DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(30)),
            new("Bulk Purchase", DiscountType.FixedAmount, 15, DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(30))
        };

        return await Task.FromResult(discounts);
    }

    public async Task<bool> CanApplyDiscountAsync(Discount discount, UserId userId)
    {
        // Simple validation - can be enhanced with business rules
        if (!discount.IsValid(DateTime.UtcNow))
            return false;

        // Mock additional validation logic
        return await Task.FromResult(true);
    }
}
