using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;
using Q2.TeeLab.OrderProcessing.Domain.Model.Queries;

namespace Q2.TeeLab.OrderProcessing.Application.Internal.QueryServices;

public interface ICartQueryService
{
    Task<Cart?> Handle(GetCartByUserIdQuery query);
}
