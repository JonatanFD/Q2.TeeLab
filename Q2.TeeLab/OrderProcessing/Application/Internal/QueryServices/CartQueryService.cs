using Q2.TeeLab.OrderProcessing.Application.Internal.QueryServices;
using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Domain.Model.Queries;
using Q2.TeeLab.OrderProcessing.Domain.Repositories;

namespace Q2.TeeLab.OrderProcessing.Application.Internal.QueryServices;

public class CartQueryService : ICartQueryService
{
    private readonly ICartRepository _cartRepository;

    public CartQueryService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Cart?> Handle(GetCartByUserIdQuery query)
    {
        return await _cartRepository.FindByUserIdAsync(query.UserId);
    }
}
