using Microsoft.EntityFrameworkCore;
using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderProcessing.Domain.Repositories;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Q2.TeeLab.OrderProcessing.Infrastructure.Persistence.EFC.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Order>> FindByUserIdAsync(UserId userId)
    {
        return await Context.Set<Order>()
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> FindActiveOrdersByUserIdAsync(UserId userId)
    {
        return await Context.Set<Order>()
            .Include(o => o.Items)
            .Where(o => o.UserId == userId && o.Status.IsActive())
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> FindByStatusAsync(OrderStatus status)
    {
        return await Context.Set<Order>()
            .Include(o => o.Items)
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> FindByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await Context.Set<Order>()
            .Include(o => o.Items)
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> FindOrderHistoryByUserIdAsync(UserId userId, int page, int pageSize)
    {
        return await Context.Set<Order>()
            .Include(o => o.Items)
            .Where(o => o.UserId == userId && o.Status.IsCompleted())
            .OrderByDescending(o => o.OrderDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> SearchOrdersAsync(
        string? searchTerm = null,
        UserId? userId = null,
        OrderStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int page = 1,
        int pageSize = 10)
    {
        var query = Context.Set<Order>()
            .Include(o => o.Items)
            .AsQueryable();

        if (userId != null)
            query = query.Where(o => o.UserId == userId);

        if (status != null)
            query = query.Where(o => o.Status == status);

        if (fromDate != null)
            query = query.Where(o => o.OrderDate >= fromDate);

        if (toDate != null)
            query = query.Where(o => o.OrderDate <= toDate);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(o => 
                o.TrackingNumber!.Contains(searchTerm) ||
                o.Notes!.Contains(searchTerm));
        }

        return await query
            .OrderByDescending(o => o.OrderDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountOrdersByUserIdAsync(UserId userId)
    {
        return await Context.Set<Order>()
            .CountAsync(o => o.UserId == userId);
    }

    public async Task<int> CountOrdersByStatusAsync(OrderStatus status)
    {
        return await Context.Set<Order>()
            .CountAsync(o => o.Status == status);
    }

    public async Task<Order?> FindByIdAsync(OrderId id)
    {
        return await Context.Set<Order>()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}
