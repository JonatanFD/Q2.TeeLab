using Q2.TeeLab.OrderFulfillment.Application.Internal.CommandServices;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Commands;
using Q2.TeeLab.OrderFulfillment.Domain.Repositories;

namespace Q2.TeeLab.OrderFulfillment.Application.Internal.CommandServices;

public class OrderFulfillmentCommandService : IOrderFulfillmentCommandService
{
    private readonly IOrderFulfillmentRepository orderFulfillmentRepository;
    private readonly IManufacturerRepository manufacturerRepository;

    public OrderFulfillmentCommandService(
        IOrderFulfillmentRepository orderFulfillmentRepository,
        IManufacturerRepository manufacturerRepository)
    {
        this.orderFulfillmentRepository = orderFulfillmentRepository;
        this.manufacturerRepository = manufacturerRepository;
    }

    public async Task<Guid> Handle(CreateOrderFulfillmentCommand command)
    {
        // Verify manufacturer exists
        var manufacturer = await manufacturerRepository.FindByIdAsync(command.ManufacturerId);
        if (manufacturer == null)
            throw new ArgumentException("Manufacturer not found", nameof(command.ManufacturerId));

        // Check if order fulfillment already exists for this order
        var existingOrderFulfillment = await orderFulfillmentRepository.FindByOrderIdAsync(command.OrderId);
        if (existingOrderFulfillment != null)
            throw new InvalidOperationException("Order fulfillment already exists for this order");

        var orderFulfillment = new Domain.Model.Aggregates.OrderFulfillment(
            command.OrderId,
            command.CustomerId,
            command.ManufacturerId,
            command.ProjectName,
            command.ProjectDescription,
            command.SpecialInstructions);

        await orderFulfillmentRepository.SaveAsync(orderFulfillment);
        return orderFulfillment.Id;
    }

    public async Task Handle(UpdateOrderFulfillmentStatusCommand command)
    {
        var orderFulfillment = await orderFulfillmentRepository.FindByIdAsync(command.OrderFulfillmentId);
        if (orderFulfillment == null)
            throw new ArgumentException("Order fulfillment not found", nameof(command.OrderFulfillmentId));

        orderFulfillment.UpdateStatus(command.NewStatus);
        await orderFulfillmentRepository.SaveAsync(orderFulfillment);
    }

    public async Task Handle(UpdateItemProgressCommand command)
    {
        var orderFulfillment = await orderFulfillmentRepository.FindByIdAsync(command.OrderFulfillmentId);
        if (orderFulfillment == null)
            throw new ArgumentException("Order fulfillment not found", nameof(command.OrderFulfillmentId));

        orderFulfillment.UpdateItemProgress(command.ItemId, command.Progress, command.Notes);
        await orderFulfillmentRepository.SaveAsync(orderFulfillment);
    }

    public async Task Handle(AddOrderFulfillmentItemCommand command)
    {
        var orderFulfillment = await orderFulfillmentRepository.FindByIdAsync(command.OrderFulfillmentId);
        if (orderFulfillment == null)
            throw new ArgumentException("Order fulfillment not found", nameof(command.OrderFulfillmentId));

        orderFulfillment.AddItem(command.ProductId, command.ProductName, command.Quantity, command.UnitPrice);
        await orderFulfillmentRepository.SaveAsync(orderFulfillment);
    }
}
