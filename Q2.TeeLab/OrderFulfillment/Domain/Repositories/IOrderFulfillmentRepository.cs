using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Domain.Repositories;

public interface IOrderFulfillmentRepository
{
    Task<Model.Aggregates.OrderFulfillment?> FindByIdAsync(OrderFulfillmentId id);
    Task<Model.Aggregates.OrderFulfillment?> FindByOrderIdAsync(Guid orderId);
    Task<IEnumerable<Model.Aggregates.OrderFulfillment>> FindAllAsync();
    Task<IEnumerable<Model.Aggregates.OrderFulfillment>> FindByCustomerIdAsync(UserId customerId);
    Task<IEnumerable<Model.Aggregates.OrderFulfillment>> FindByManufacturerIdAsync(ManufacturerId manufacturerId);
    Task<IEnumerable<Model.Aggregates.OrderFulfillment>> FindByStatusAsync(OrderFulfillmentStatus status);
    Task<IEnumerable<Model.Aggregates.OrderFulfillment>> FindOverdueOrdersAsync();
    Task<IEnumerable<Model.Aggregates.OrderFulfillment>> FindOrdersWithEstimatedDeliveryBetweenAsync(DateTime startDate, DateTime endDate);
    Task SaveAsync(Model.Aggregates.OrderFulfillment orderFulfillment);
    Task DeleteAsync(OrderFulfillmentId id);
}
