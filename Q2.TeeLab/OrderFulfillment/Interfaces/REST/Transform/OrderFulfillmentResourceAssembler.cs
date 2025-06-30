using Q2.TeeLab.OrderFulfillment.Domain.Model.Commands;
using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderFulfillment.Interfaces.REST.Resources;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Interfaces.REST.Transform;

public static class OrderFulfillmentResourceAssembler
{
    public static OrderFulfillmentResource ToResource(Domain.Model.Aggregates.OrderFulfillment orderFulfillment)
    {
        return new OrderFulfillmentResource(
            orderFulfillment.Id,
            orderFulfillment.OrderId,
            orderFulfillment.CustomerId,
            orderFulfillment.ManufacturerId,
            orderFulfillment.ProjectName,
            orderFulfillment.ProjectDescription,
            orderFulfillment.Status.ToString(),
            orderFulfillment.OrderDate,
            orderFulfillment.EstimatedDeliveryDate,
            orderFulfillment.ActualDeliveryDate,
            orderFulfillment.TotalAmount,
            orderFulfillment.SpecialInstructions,
            orderFulfillment.GetCompletionPercentage(),
            orderFulfillment.Items.Select(ToItemResource),
            orderFulfillment.CreatedAt,
            orderFulfillment.UpdatedAt
        );
    }

    public static OrderFulfillmentItemResource ToItemResource(Domain.Model.Entities.OrderFulfillmentItem item)
    {
        return new OrderFulfillmentItemResource(
            item.Id,
            item.ProductId,
            item.ProductName,
            item.Quantity,
            item.Progress.ToString(),
            item.ProgressNotes,
            item.StartDate,
            item.EstimatedCompletionDate,
            item.ActualCompletionDate,
            item.CreatedAt,
            item.UpdatedAt
        );
    }

    public static CreateOrderFulfillmentCommand ToCreateCommand(CreateOrderFulfillmentResource resource)
    {
        return new CreateOrderFulfillmentCommand(
            resource.OrderId,
            new UserId(resource.CustomerId),
            new ManufacturerId(resource.ManufacturerId),
            resource.ProjectName,
            resource.ProjectDescription,
            resource.SpecialInstructions
        );
    }

    public static UpdateOrderFulfillmentStatusCommand ToUpdateStatusCommand(
        Guid orderFulfillmentId, 
        UpdateOrderFulfillmentStatusResource resource)
    {
        if (!Enum.TryParse<OrderFulfillmentStatus>(resource.Status, out var status))
            throw new ArgumentException($"Invalid status: {resource.Status}");

        return new UpdateOrderFulfillmentStatusCommand(
            new OrderFulfillmentId(orderFulfillmentId),
            status
        );
    }

    public static UpdateItemProgressCommand ToUpdateItemProgressCommand(
        Guid orderFulfillmentId,
        UpdateItemProgressResource resource)
    {
        if (!Enum.TryParse<ItemProgress>(resource.Progress, out var progress))
            throw new ArgumentException($"Invalid progress: {resource.Progress}");

        return new UpdateItemProgressCommand(
            new OrderFulfillmentId(orderFulfillmentId),
            new OrderFulfillmentItemId(resource.ItemId),
            progress,
            resource.Notes
        );
    }

    public static AddOrderFulfillmentItemCommand ToAddItemCommand(
        Guid orderFulfillmentId,
        AddOrderFulfillmentItemResource resource)
    {
        return new AddOrderFulfillmentItemCommand(
            new OrderFulfillmentId(orderFulfillmentId),
            new ProductId(resource.ProductId),
            resource.ProductName,
            resource.Quantity,
            resource.UnitPrice
        );
    }
}
