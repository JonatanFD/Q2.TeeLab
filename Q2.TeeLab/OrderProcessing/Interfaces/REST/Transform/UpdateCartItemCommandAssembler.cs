using Q2.TeeLab.OrderProcessing.Domain.Model.Commands;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Transform;

public static class UpdateCartItemCommandAssembler
{
    public static UpdateCartItemQuantityCommand ToCommand(UserId userId, UpdateCartItemResource resource)
    {
        return new UpdateCartItemQuantityCommand(userId, new ProductId(resource.ProductId), resource.Quantity);
    }
}
