using Q2.TeeLab.OrderFulfillment.Domain.Model.Commands;

namespace Q2.TeeLab.OrderFulfillment.Application.Internal.CommandServices;

public interface IManufacturerCommandService
{
    Task<Guid> Handle(CreateManufacturerCommand command);
}
