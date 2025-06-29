using Q2.TeeLab.OrderProcessing.Domain.Model.Commands;
using Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Transform;

public static class CreateOrderCommandAssembler
{
    public static CreateOrderFromCartCommand ToCommand(CreateOrderResource resource)
    {
        // We'll need to get the cart ID from the user's cart
        return new CreateOrderFromCartCommand(new UserId(resource.UserId), new Domain.Model.ValueObjects.CartId(), resource.Notes);
    }
}
