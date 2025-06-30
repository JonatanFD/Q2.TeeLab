using Q2.TeeLab.OrderFulfillment.Domain.Model.Commands;

namespace Q2.TeeLab.OrderFulfillment.Application.Internal.CommandServices;

public interface IOrderFulfillmentCommandService
{
    Task<Guid> Handle(CreateOrderFulfillmentCommand command);
    Task Handle(UpdateOrderFulfillmentStatusCommand command);
    Task Handle(UpdateItemProgressCommand command);
    Task Handle(AddOrderFulfillmentItemCommand command);
}
