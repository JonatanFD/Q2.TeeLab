using Microsoft.EntityFrameworkCore;
using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderFulfillment.Domain.Repositories;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace Q2.TeeLab.OrderFulfillment.Infrastructure.Persistence.EFC.Repositories;

public class OrderFulfillmentRepository : IOrderFulfillmentRepository
{
    private readonly AppDbContext context;

    public OrderFulfillmentRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Domain.Model.Aggregates.OrderFulfillment?> FindByIdAsync(OrderFulfillmentId id)
    {
        return await context.Set<Domain.Model.Aggregates.OrderFulfillment>()
            .Include(of => of.Items)
            .FirstOrDefaultAsync(of => of.Id == id);
    }

    public async Task<Domain.Model.Aggregates.OrderFulfillment?> FindByOrderIdAsync(Guid orderId)
    {
        return await context.Set<Domain.Model.Aggregates.OrderFulfillment>()
            .Include(of => of.Items)
            .FirstOrDefaultAsync(of => of.OrderId == orderId);
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.OrderFulfillment>> FindAllAsync()
    {
        return await context.Set<Domain.Model.Aggregates.OrderFulfillment>()
            .Include(of => of.Items)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.OrderFulfillment>> FindByCustomerIdAsync(UserId customerId)
    {
        return await context.Set<Domain.Model.Aggregates.OrderFulfillment>()
            .Include(of => of.Items)
            .Where(of => of.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.OrderFulfillment>> FindByManufacturerIdAsync(ManufacturerId manufacturerId)
    {
        return await context.Set<Domain.Model.Aggregates.OrderFulfillment>()
            .Include(of => of.Items)
            .Where(of => of.ManufacturerId == manufacturerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.OrderFulfillment>> FindByStatusAsync(OrderFulfillmentStatus status)
    {
        return await context.Set<Domain.Model.Aggregates.OrderFulfillment>()
            .Include(of => of.Items)
            .Where(of => of.Status == status)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.OrderFulfillment>> FindOverdueOrdersAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await context.Set<Domain.Model.Aggregates.OrderFulfillment>()
            .Include(of => of.Items)
            .Where(of => of.EstimatedDeliveryDate.HasValue && 
                        of.EstimatedDeliveryDate.Value.Date < today &&
                        of.Status != OrderFulfillmentStatus.Completed &&
                        of.Status != OrderFulfillmentStatus.Delivered &&
                        of.Status != OrderFulfillmentStatus.Cancelled)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.OrderFulfillment>> FindOrdersWithEstimatedDeliveryBetweenAsync(DateTime startDate, DateTime endDate)
    {
        return await context.Set<Domain.Model.Aggregates.OrderFulfillment>()
            .Include(of => of.Items)
            .Where(of => of.EstimatedDeliveryDate.HasValue &&
                        of.EstimatedDeliveryDate.Value.Date >= startDate.Date &&
                        of.EstimatedDeliveryDate.Value.Date <= endDate.Date)
            .ToListAsync();
    }

    public async Task SaveAsync(Domain.Model.Aggregates.OrderFulfillment orderFulfillment)
    {
        var existingOrderFulfillment = await FindByIdAsync(orderFulfillment.Id);
        if (existingOrderFulfillment == null)
        {
            await context.Set<Domain.Model.Aggregates.OrderFulfillment>().AddAsync(orderFulfillment);
        }
        else
        {
            context.Set<Domain.Model.Aggregates.OrderFulfillment>().Update(orderFulfillment);
        }
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(OrderFulfillmentId id)
    {
        var orderFulfillment = await FindByIdAsync(id);
        if (orderFulfillment != null)
        {
            context.Set<Domain.Model.Aggregates.OrderFulfillment>().Remove(orderFulfillment);
            await context.SaveChangesAsync();
        }
    }
}
