using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Q2.TeeLab.OrderFulfillment.Application.Internal.CommandServices;
using Q2.TeeLab.OrderFulfillment.Application.Internal.QueryServices;
using Q2.TeeLab.OrderFulfillment.Domain.Model.Queries;
using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderFulfillment.Interfaces.REST.Resources;
using Q2.TeeLab.OrderFulfillment.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace Q2.TeeLab.OrderFulfillment.Interfaces.REST;

[ApiController]
[Route("api/v1/order-fulfillment")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Order Processing")]
public class OrderFulfillmentController(
    IOrderFulfillmentCommandService orderFulfillmentCommandService,
    IOrderFulfillmentQueryService orderFulfillmentQueryService
) : BaseApiController
{
    [HttpGet("{orderFulfillmentId:guid}")]
    [SwaggerOperation(
        Summary = "Get order fulfillment by ID",
        Description = "Retrieve a specific order fulfillment by its ID",
        Tags = new[] { "Order Processing" })]
    [SwaggerResponse(200, "Order fulfillment found", typeof(OrderFulfillmentApiResponse<OrderFulfillmentResource>))]
    [SwaggerResponse(404, "Order fulfillment not found", typeof(OrderFulfillmentApiResponse<OrderFulfillmentResource>))]
    public async Task<ActionResult<OrderFulfillmentApiResponse<OrderFulfillmentResource>>> GetOrderFulfillmentById(Guid orderFulfillmentId)
    {
        try
        {
            var query = new GetOrderFulfillmentByIdQuery(new OrderFulfillmentId(orderFulfillmentId));
            var orderFulfillment = await orderFulfillmentQueryService.Handle(query);

            if (orderFulfillment == null)
            {
                return NotFound(OrderFulfillmentApiResponse<OrderFulfillmentResource>.ErrorResponse($"Order fulfillment with ID {orderFulfillmentId} not found"));
            }

            var resource = OrderFulfillmentResourceAssembler.ToResource(orderFulfillment);
            return Ok(OrderFulfillmentApiResponse<OrderFulfillmentResource>.SuccessResponse(resource));
        }
        catch (Exception ex)
        {
            return HandleError<OrderFulfillmentResource>(ex, "An error occurred while retrieving the order fulfillment");
        }
    }

    [HttpGet("manufacturers/{manufacturerId:guid}")]
    [SwaggerOperation(
        Summary = "Get order fulfillments by manufacturer",
        Description = "Retrieve all order fulfillments assigned to a specific manufacturer",
        Tags = new[] { "Order Processing" })]
    [SwaggerResponse(200, "Order fulfillments found", typeof(OrderFulfillmentApiResponse<IEnumerable<OrderFulfillmentResource>>))]
    public async Task<ActionResult<OrderFulfillmentApiResponse<IEnumerable<OrderFulfillmentResource>>>> GetOrderFulfillmentsByManufacturer(Guid manufacturerId)
    {
        try
        {
            var query = new GetOrderFulfillmentsByManufacturerQuery(new ManufacturerId(manufacturerId));
            var orderFulfillments = await orderFulfillmentQueryService.Handle(query);
            var resources = orderFulfillments.Select(OrderFulfillmentResourceAssembler.ToResource);

            return Ok(OrderFulfillmentApiResponse<IEnumerable<OrderFulfillmentResource>>.SuccessResponse(resources));
        }
        catch (Exception ex)
        {
            return HandleError<IEnumerable<OrderFulfillmentResource>>(ex, "An error occurred while retrieving order fulfillments");
        }
    }

    [HttpGet("status/{status}")]
    [SwaggerOperation(
        Summary = "Get order fulfillments by status",
        Description = "Retrieve all order fulfillments with a specific status",
        Tags = new[] { "Order Processing" })]
    [SwaggerResponse(200, "Order fulfillments found", typeof(OrderFulfillmentApiResponse<IEnumerable<OrderFulfillmentResource>>))]
    [SwaggerResponse(400, "Invalid status", typeof(OrderFulfillmentApiResponse<IEnumerable<OrderFulfillmentResource>>))]
    public async Task<ActionResult<OrderFulfillmentApiResponse<IEnumerable<OrderFulfillmentResource>>>> GetOrderFulfillmentsByStatus(string status)
    {
        try
        {
            if (!Enum.TryParse<OrderFulfillmentStatus>(status, true, out var orderStatus))
            {
                return BadRequest(OrderFulfillmentApiResponse<IEnumerable<OrderFulfillmentResource>>.ErrorResponse($"Invalid status: {status}"));
            }

            var query = new GetOrderFulfillmentsByStatusQuery(orderStatus);
            var orderFulfillments = await orderFulfillmentQueryService.Handle(query);
            var resources = orderFulfillments.Select(OrderFulfillmentResourceAssembler.ToResource);

            return Ok(OrderFulfillmentApiResponse<IEnumerable<OrderFulfillmentResource>>.SuccessResponse(resources));
        }
        catch (Exception ex)
        {
            return HandleError<IEnumerable<OrderFulfillmentResource>>(ex, "An error occurred while retrieving order fulfillments");
        }
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create order fulfillment",
        Description = "Create a new order fulfillment for a manufacturer",
        Tags = new[] { "Order Processing" })]
    [SwaggerResponse(201, "Order fulfillment created successfully", typeof(OrderFulfillmentApiResponse<Guid>))]
    [SwaggerResponse(400, "Bad request", typeof(OrderFulfillmentApiResponse<Guid>))]
    public async Task<ActionResult<OrderFulfillmentApiResponse<Guid>>> CreateOrderFulfillment(
        [FromBody] CreateOrderFulfillmentResource resource)
    {
        try
        {
            var command = OrderFulfillmentResourceAssembler.ToCreateCommand(resource);
            var orderFulfillmentId = await orderFulfillmentCommandService.Handle(command);

            return Created($"/api/v1/order-fulfillment/{orderFulfillmentId}",
                OrderFulfillmentApiResponse<Guid>.SuccessResponse(orderFulfillmentId, "Order fulfillment created successfully"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(OrderFulfillmentApiResponse<Guid>.ErrorResponse("Invalid input data", new[] { ex.Message }));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(OrderFulfillmentApiResponse<Guid>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return HandleError<Guid>(ex, "An error occurred while creating the order fulfillment");
        }
    }

    [HttpPut("{orderFulfillmentId:guid}/status")]
    [SwaggerOperation(
        Summary = "Update order fulfillment status",
        Description = "Update the status of an order fulfillment",
        Tags = new[] { "Order Processing" })]
    [SwaggerResponse(200, "Status updated successfully", typeof(OrderFulfillmentApiResponse<bool>))]
    [SwaggerResponse(400, "Bad request", typeof(OrderFulfillmentApiResponse<bool>))]
    public async Task<ActionResult<OrderFulfillmentApiResponse<bool>>> UpdateOrderFulfillmentStatus(
        Guid orderFulfillmentId,
        [FromBody] UpdateOrderFulfillmentStatusResource resource)
    {
        try
        {
            var command = OrderFulfillmentResourceAssembler.ToUpdateStatusCommand(orderFulfillmentId, resource);
            await orderFulfillmentCommandService.Handle(command);

            return HandleResult(true, "Order fulfillment status updated successfully", "Failed to update order fulfillment status");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(OrderFulfillmentApiResponse<bool>.ErrorResponse("Invalid input data", new[] { ex.Message }));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(OrderFulfillmentApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return HandleError<bool>(ex, "An error occurred while updating the order fulfillment status");
        }
    }

    [HttpPut("{orderFulfillmentId:guid}/items/progress")]
    [SwaggerOperation(
        Summary = "Update item progress",
        Description = "Update the progress of a specific item in an order fulfillment",
        Tags = new[] { "Order Processing" })]
    [SwaggerResponse(200, "Item progress updated successfully", typeof(OrderFulfillmentApiResponse<bool>))]
    [SwaggerResponse(400, "Bad request", typeof(OrderFulfillmentApiResponse<bool>))]
    public async Task<ActionResult<OrderFulfillmentApiResponse<bool>>> UpdateItemProgress(
        Guid orderFulfillmentId,
        [FromBody] UpdateItemProgressResource resource)
    {
        try
        {
            var command = OrderFulfillmentResourceAssembler.ToUpdateItemProgressCommand(orderFulfillmentId, resource);
            await orderFulfillmentCommandService.Handle(command);

            return HandleResult(true, "Item progress updated successfully", "Failed to update item progress");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(OrderFulfillmentApiResponse<bool>.ErrorResponse("Invalid input data", new[] { ex.Message }));
        }
        catch (Exception ex)
        {
            return HandleError<bool>(ex, "An error occurred while updating the item progress");
        }
    }

    [HttpPost("{orderFulfillmentId:guid}/items")]
    [SwaggerOperation(
        Summary = "Add item to order fulfillment",
        Description = "Add a new item to an existing order fulfillment",
        Tags = new[] { "Order Processing" })]
    [SwaggerResponse(200, "Item added successfully", typeof(OrderFulfillmentApiResponse<bool>))]
    [SwaggerResponse(400, "Bad request", typeof(OrderFulfillmentApiResponse<bool>))]
    public async Task<ActionResult<OrderFulfillmentApiResponse<bool>>> AddOrderFulfillmentItem(
        Guid orderFulfillmentId,
        [FromBody] AddOrderFulfillmentItemResource resource)
    {
        try
        {
            var command = OrderFulfillmentResourceAssembler.ToAddItemCommand(orderFulfillmentId, resource);
            await orderFulfillmentCommandService.Handle(command);

            return HandleResult(true, "Item added to order fulfillment successfully", "Failed to add item to order fulfillment");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(OrderFulfillmentApiResponse<bool>.ErrorResponse("Invalid input data", new[] { ex.Message }));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(OrderFulfillmentApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return HandleError<bool>(ex, "An error occurred while adding the item to order fulfillment");
        }
    }
}
