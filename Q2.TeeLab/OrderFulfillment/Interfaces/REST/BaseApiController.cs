using Microsoft.AspNetCore.Mvc;
using Q2.TeeLab.OrderFulfillment.Interfaces.REST.Resources;

namespace Q2.TeeLab.OrderFulfillment.Interfaces.REST;

public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<OrderFulfillmentApiResponse<T>> HandleResult<T>(T result, string? notFoundMessage = null)
    {
        if (result == null)
        {
            return NotFound(OrderFulfillmentApiResponse<T>.ErrorResponse(notFoundMessage ?? "Resource not found"));
        }

        return Ok(OrderFulfillmentApiResponse<T>.SuccessResponse(result));
    }

    protected ActionResult<OrderFulfillmentApiResponse<bool>> HandleResult(bool result, string successMessage, string errorMessage)
    {
        if (result)
        {
            return Ok(OrderFulfillmentApiResponse<bool>.SuccessResponse(true, successMessage));
        }

        return BadRequest(OrderFulfillmentApiResponse<bool>.ErrorResponse(errorMessage));
    }

    protected ActionResult<OrderFulfillmentApiResponse<T>> HandleError<T>(Exception ex, string message)
    {
        // Log the exception here if needed
        return StatusCode(500, OrderFulfillmentApiResponse<T>.ErrorResponse(message, new[] { ex.Message }));
    }
}
