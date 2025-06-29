using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Repositories;

namespace Q2.TeeLab.OrderProcessing.Domain.Repositories;

public interface ICartRepository : IBaseRepository<Cart, CartId>
{
    Task<Cart?> FindByUserIdAsync(UserId userId);
    Task<Cart> GetOrCreateByUserIdAsync(UserId userId);
}
