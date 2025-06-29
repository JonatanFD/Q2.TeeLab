using Microsoft.EntityFrameworkCore;
using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderProcessing.Domain.Repositories;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Configuration;
using Q2.TeeLab.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Q2.TeeLab.OrderProcessing.Infrastructure.Persistence.EFC.Repositories;

public class CartRepository : BaseRepository<Cart>, ICartRepository
{
    public CartRepository(AppDbContext context) : base(context) { }

    public async Task<Cart?> FindByUserIdAsync(UserId userId)
    {
        return await Context.Set<Cart>()
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Cart> GetOrCreateByUserIdAsync(UserId userId)
    {
        var cart = await FindByUserIdAsync(userId);
        if (cart == null)
        {
            cart = new Cart(userId);
            await AddAsync(cart);
        }
        return cart;
    }

    public async Task<Cart?> FindByIdAsync(CartId id)
    {
        return await Context.Set<Cart>()
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
