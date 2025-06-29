using Q2.TeeLab.OrderProcessing.Domain.Model.Commands;

namespace Q2.TeeLab.OrderProcessing.Application.Internal.CommandServices;

public interface ICartCommandService
{
    Task Handle(AddItemToCartCommand command);
    Task Handle(UpdateCartItemQuantityCommand command);
    Task Handle(RemoveItemFromCartCommand command);
    Task Handle(ClearCartCommand command);
    Task Handle(ApplyDiscountToCartCommand command);
}
