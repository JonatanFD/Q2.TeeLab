using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Q2.TeeLab.OrderProcessing.Application.Internal.CommandServices;
using Q2.TeeLab.OrderProcessing.Application.Internal.QueryServices;
using Q2.TeeLab.OrderProcessing.Domain.Model.Commands;
using Q2.TeeLab.OrderProcessing.Domain.Model.Queries;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;
using Q2.TeeLab.OrderProcessing.Interfaces.REST.Transform;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace Q2.TeeLab.OrderProcessing.Interfaces.REST;

[ApiController]
[Route("api/v1/order-processing/orders")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Order Processing - Orders")]
public class OrderController(
    IOrderCommandService orderCommandService,
    IOrderQueryService orderQueryService
    ) : BaseApiController
{


    [HttpPost]
    [SwaggerOperation(
        Summary = "Create order from cart",
        Description = "Create a new order from the user's cart",
        Tags = new[] { "Order Processing - Orders" })]
    [SwaggerResponse(201, "Order created successfully", typeof(OrderApiResponse<OrderResource>))]
    [SwaggerResponse(400, "Bad request", typeof(OrderApiResponse<OrderResource>))]
    public async Task<ActionResult<OrderApiResponse<OrderResource>>> CreateOrder(
        [FromBody] CreateOrderResource resource)
    {
        try
        {
            var command = CreateOrderCommandAssembler.ToCommand(resource);
            var order = await orderCommandService.Handle(command);
            var orderResource = OrderResourceAssembler.ToResource(order);

            var response = OrderApiResponse<OrderResource>.SuccessResponse(orderResource, "Order created successfully");
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id.Value }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(OrderApiResponse<OrderResource>.ErrorResponse("Invalid input data", new[] { ex.Message }));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(OrderApiResponse<OrderResource>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, OrderApiResponse<OrderResource>.ErrorResponse("An error occurred while creating the order", new[] { ex.Message }));
        }
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get order by ID",
        Description = "Get an order by its unique identifier",
        Tags = new[] { "Order Processing - Orders" })]
    [SwaggerResponse(200, "Order found", typeof(OrderApiResponse<OrderResource>))]
    [SwaggerResponse(404, "Order not found", typeof(OrderApiResponse<OrderResource>))]
    public async Task<ActionResult<OrderApiResponse<OrderResource>>> GetOrderById(Guid id)
    {
        try
        {
            var query = new GetOrderByIdQuery(new Domain.Model.ValueObjects.OrderId(id));
            var order = await orderQueryService.Handle(query);

            if (order == null)
            {
                return NotFound(OrderApiResponse<OrderResource>.ErrorResponse($"Order with id {id} not found"));
            }

            var resource = OrderResourceAssembler.ToResource(order);
            return Ok(OrderApiResponse<OrderResource>.SuccessResponse(resource));
        }
        catch (Exception ex)
        {
            return StatusCode(500, OrderApiResponse<OrderResource>.ErrorResponse("An error occurred while retrieving the order", new[] { ex.Message }));
        }
    }

    [HttpGet("users/{userId:guid}")]
    [SwaggerOperation(
        Summary = "Get orders by user ID",
        Description = "Get all orders for a specific user",
        Tags = new[] { "Order Processing - Orders" })]
    [SwaggerResponse(200, "Orders found", typeof(OrderApiResponse<IEnumerable<OrderSummaryResource>>))]
    public async Task<ActionResult<OrderApiResponse<IEnumerable<OrderSummaryResource>>>> GetOrdersByUserId(
        Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = new GetOrdersByUserIdQuery(new UserId(userId), page, pageSize);
            var orders = await orderQueryService.Handle(query);
            var resources = orders.Select(OrderResourceAssembler.ToSummaryResource);

            return Ok(OrderApiResponse<IEnumerable<OrderSummaryResource>>.SuccessResponse(resources));
        }
        catch (Exception ex)
        {
            return StatusCode(500, OrderApiResponse<IEnumerable<OrderSummaryResource>>.ErrorResponse("An error occurred while retrieving the orders", new[] { ex.Message }));
        }
    }

    [HttpGet("users/{userId:guid}/active")]
    [SwaggerOperation(
        Summary = "Get active orders by user ID",
        Description = "Get all active orders for a specific user",
        Tags = new[] { "Order Processing - Orders" })]
    [SwaggerResponse(200, "Active orders found", typeof(OrderApiResponse<IEnumerable<OrderSummaryResource>>))]
    public async Task<ActionResult<OrderApiResponse<IEnumerable<OrderSummaryResource>>>> GetActiveOrdersByUserId(Guid userId)
    {
        try
        {
            var query = new GetActiveOrdersByUserIdQuery(new UserId(userId));
            var orders = await orderQueryService.Handle(query);
            var resources = orders.Select(OrderResourceAssembler.ToSummaryResource);

            return Ok(OrderApiResponse<IEnumerable<OrderSummaryResource>>.SuccessResponse(resources));
        }
        catch (Exception ex)
        {
            return StatusCode(500, OrderApiResponse<IEnumerable<OrderSummaryResource>>.ErrorResponse("An error occurred while retrieving the active orders", new[] { ex.Message }));
        }
    }

    [HttpPut("{id:guid}/confirm")]
    [SwaggerOperation(
        Summary = "Confirm order",
        Description = "Confirm an order",
        Tags = new[] { "Order Processing - Orders" })]
    [SwaggerResponse(200, "Order confirmed successfully", typeof(OrderApiResponse<bool>))]
    [SwaggerResponse(404, "Order not found", typeof(OrderApiResponse<bool>))]
    public async Task<ActionResult<OrderApiResponse<bool>>> ConfirmOrder(Guid id)
    {
        try
        {
            var command = new ConfirmOrderCommand(new OrderId(id));
            var result = await orderCommandService.Handle(command);

            if (!result)
            {
                return NotFound(OrderApiResponse<bool>.ErrorResponse($"Order with id {id} not found"));
            }

            return Ok(OrderApiResponse<bool>.SuccessResponse(true, "Order confirmed successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(OrderApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, OrderApiResponse<bool>.ErrorResponse("An error occurred while confirming the order", new[] { ex.Message }));
        }
    }

    [HttpPut("{id:guid}/cancel")]
    [SwaggerOperation(
        Summary = "Cancel order",
        Description = "Cancel an order",
        Tags = new[] { "Order Processing - Orders" })]
    [SwaggerResponse(200, "Order cancelled successfully", typeof(OrderApiResponse<bool>))]
    [SwaggerResponse(404, "Order not found", typeof(OrderApiResponse<bool>))]
    public async Task<ActionResult<OrderApiResponse<bool>>> CancelOrder(Guid id, [FromBody] string? reason = null)
    {
        try
        {
            var command = new CancelOrderCommand(new OrderId(id), reason);
            var result = await orderCommandService.Handle(command);

            if (!result)
            {
                return NotFound(OrderApiResponse<bool>.ErrorResponse($"Order with id {id} not found"));
            }

            return Ok(OrderApiResponse<bool>.SuccessResponse(true, "Order cancelled successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(OrderApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, OrderApiResponse<bool>.ErrorResponse("An error occurred while cancelling the order", new[] { ex.Message }));
        }
    }

    [HttpPut("{id:guid}/status")]
    [SwaggerOperation(
        Summary = "Update order status",
        Description = "Update the status of an order",
        Tags = new[] { "Order Processing - Orders" })]
    [SwaggerResponse(200, "Order status updated successfully", typeof(OrderApiResponse<bool>))]
    [SwaggerResponse(404, "Order not found", typeof(OrderApiResponse<bool>))]
    public async Task<ActionResult<OrderApiResponse<bool>>> UpdateOrderStatus(
        Guid id,
        [FromBody] UpdateOrderStatusResource resource)
    {
        try
        {
            var command = new UpdateOrderStatusCommand(new OrderId(id), Enum.Parse<OrderStatus>(resource.Status, true), resource.TrackingNumber);
            var result = await orderCommandService.Handle(command);

            if (!result)
            {
                return NotFound(OrderApiResponse<bool>.ErrorResponse($"Order with id {id} not found"));
            }

            return Ok(OrderApiResponse<bool>.SuccessResponse(true, "Order status updated successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(OrderApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, OrderApiResponse<bool>.ErrorResponse("An error occurred while updating the order status", new[] { ex.Message }));
        }
    }
}
