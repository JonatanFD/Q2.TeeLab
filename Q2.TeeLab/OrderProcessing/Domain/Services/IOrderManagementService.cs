using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Domain.Model.Entities;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Domain.Services;

public interface IOrderManagementService
{
    Task<bool> ValidateOrderItemsAsync(IEnumerable<OrderItem> items);
    Task<Money> CalculateShippingCostAsync(Order order, string shippingAddress);
    Task<IEnumerable<Discount>> GetAvailableDiscountsAsync(UserId userId);
    Task<bool> CanApplyDiscountAsync(Discount discount, UserId userId);
}
