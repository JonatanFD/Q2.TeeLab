using Q2.TeeLab.OrderProcessing.Domain.Model.Commands;
using Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;

namespace Q2.TeeLab.OrderProcessing.Application.Internal.CommandServices;

public interface IOrderCommandService
{
    Task<Order> Handle(CreateOrderFromCartCommand command);
    Task<bool> Handle(ConfirmOrderCommand command);
    Task<bool> Handle(CancelOrderCommand command);
    Task<bool> Handle(UpdateOrderStatusCommand command);
    Task<bool> Handle(ApplyDiscountToOrderCommand command);
}
