using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Q2.TeeLab.OrderProcessing.Application.Internal.CommandServices;
using Q2.TeeLab.OrderProcessing.Application.Internal.QueryServices;
using Q2.TeeLab.OrderProcessing.Domain.Model.Commands;
using Q2.TeeLab.OrderProcessing.Domain.Model.Queries;
using Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;
using Q2.TeeLab.OrderProcessing.Interfaces.REST.Transform;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace Q2.TeeLab.OrderProcessing.Interfaces.REST;

[ApiController]
[Route("api/v1/order-processing/carts")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Order Processing - Shopping Carts")]
public class CartController : BaseApiController
{
    private readonly ICartCommandService _cartCommandService;
    private readonly ICartQueryService _cartQueryService;

    public CartController(
        ICartCommandService cartCommandService,
        ICartQueryService cartQueryService)
    {
        _cartCommandService = cartCommandService;
        _cartQueryService = cartQueryService;
    }

    [HttpGet("users/{userId:guid}")]
    [SwaggerOperation(
        Summary = "Get user's cart",
        Description = "Get the shopping cart for a specific user",
        Tags = new[] { "Order Processing - Shopping Carts" })]
    [SwaggerResponse(200, "Cart found", typeof(OrderApiResponse<CartResource>))]
    [SwaggerResponse(404, "Cart not found", typeof(OrderApiResponse<CartResource>))]
    public async Task<ActionResult<OrderApiResponse<CartResource>>> GetCartByUserId(Guid userId)
    {
        try
        {
            var query = new GetCartByUserIdQuery(new UserId(userId));
            var cart = await _cartQueryService.Handle(query);

            if (cart == null)
            {
                return NotFound(OrderApiResponse<CartResource>.ErrorResponse($"Cart for user {userId} not found"));
            }

            var resource = CartResourceAssembler.ToResource(cart);
            return Ok(OrderApiResponse<CartResource>.SuccessResponse(resource));
        }
        catch (Exception ex)
        {
            return HandleError<CartResource>(ex, "An error occurred while retrieving the cart");
        }
    }

    [HttpPost("users/{userId:guid}/items")]
    [SwaggerOperation(
        Summary = "Add item to cart",
        Description = "Add a product to the user's cart",
        Tags = new[] { "Order Processing - Shopping Carts" })]
    [SwaggerResponse(200, "Item added successfully", typeof(OrderApiResponse<bool>))]
    [SwaggerResponse(400, "Bad request", typeof(OrderApiResponse<bool>))]
    public async Task<ActionResult<OrderApiResponse<bool>>> AddItemToCart(
        Guid userId,
        [FromBody] AddItemToCartResource resource)
    {
        try
        {
            var command = AddItemToCartCommandAssembler.ToCommand(new UserId(userId), resource);
            await _cartCommandService.Handle(command);

            return HandleResult(true, "Item added to cart successfully", "Failed to add item to cart");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(OrderApiResponse<bool>.ErrorResponse("Invalid input data", new[] { ex.Message }));
        }
        catch (Exception ex)
        {
            return HandleError<bool>(ex, "An error occurred while adding the item to cart");
        }
    }

    [HttpPut("users/{userId:guid}/items")]
    [SwaggerOperation(
        Summary = "Update cart item quantity",
        Description = "Update the quantity of an item in the user's cart",
        Tags = new[] { "Order Processing - Shopping Carts" })]
    [SwaggerResponse(200, "Item quantity updated successfully", typeof(OrderApiResponse<bool>))]
    [SwaggerResponse(400, "Bad request", typeof(OrderApiResponse<bool>))]
    public async Task<ActionResult<OrderApiResponse<bool>>> UpdateCartItemQuantity(
        Guid userId,
        [FromBody] UpdateCartItemResource resource)
    {
        try
        {
            var command = UpdateCartItemCommandAssembler.ToCommand(new UserId(userId), resource);
            await _cartCommandService.Handle(command);

            return HandleResult(true, "Item quantity updated successfully", "Failed to update item quantity");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(OrderApiResponse<bool>.ErrorResponse("Invalid input data", new[] { ex.Message }));
        }
        catch (Exception ex)
        {
            return HandleError<bool>(ex, "An error occurred while updating the item quantity");
        }
    }

    [HttpDelete("users/{userId:guid}/items/{productId:guid}")]
    [SwaggerOperation(
        Summary = "Remove item from cart",
        Description = "Remove a product from the user's cart",
        Tags = new[] { "Order Processing - Shopping Carts" })]
    [SwaggerResponse(200, "Item removed successfully", typeof(OrderApiResponse<bool>))]
    [SwaggerResponse(400, "Bad request", typeof(OrderApiResponse<bool>))]
    public async Task<ActionResult<OrderApiResponse<bool>>> RemoveItemFromCart(Guid userId, Guid productId)
    {
        try
        {
            var command = new RemoveItemFromCartCommand(new UserId(userId), new Domain.Model.ValueObjects.ProductId(productId));
            await _cartCommandService.Handle(command);

            return HandleResult(true, "Item removed from cart successfully", "Failed to remove item from cart");
        }
        catch (Exception ex)
        {
            return HandleError<bool>(ex, "An error occurred while removing the item from cart");
        }
    }

    [HttpDelete("users/{userId:guid}")]
    [SwaggerOperation(
        Summary = "Clear cart",
        Description = "Remove all items from the user's cart",
        Tags = new[] { "Order Processing - Shopping Carts" })]
    [SwaggerResponse(200, "Cart cleared successfully", typeof(OrderApiResponse<bool>))]
    [SwaggerResponse(400, "Bad request", typeof(OrderApiResponse<bool>))]
    public async Task<ActionResult<OrderApiResponse<bool>>> ClearCart(Guid userId)
    {
        try
        {
            var command = new ClearCartCommand(new UserId(userId));
            await _cartCommandService.Handle(command);

            return HandleResult(true, "Cart cleared successfully", "Failed to clear cart");
        }
        catch (Exception ex)
        {
            return HandleError<bool>(ex, "An error occurred while clearing the cart");
        }
    }

    [HttpPost("users/{userId:guid}/discounts")]
    [SwaggerOperation(
        Summary = "Apply discount to cart",
        Description = "Apply a discount to the user's cart",
        Tags = new[] { "Order Processing - Shopping Carts" })]
    [SwaggerResponse(200, "Discount applied successfully", typeof(OrderApiResponse<bool>))]
    [SwaggerResponse(400, "Bad request", typeof(OrderApiResponse<bool>))]
    public async Task<ActionResult<OrderApiResponse<bool>>> ApplyDiscountToCart(
        Guid userId,
        [FromBody] ApplyDiscountResource resource)
    {
        try
        {
            var command = ApplyDiscountCommandAssembler.ToCommand(new UserId(userId), resource);
            await _cartCommandService.Handle(command);

            return HandleResult(true, "Discount applied to cart successfully", "Failed to apply discount to cart");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(OrderApiResponse<bool>.ErrorResponse("Invalid discount", new[] { ex.Message }));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(OrderApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return HandleError<bool>(ex, "An error occurred while applying the discount");
        }
    }
}
