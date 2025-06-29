using Microsoft.AspNetCore.Mvc;
using Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

namespace Q2.TeeLab.OrderProcessing.Interfaces.REST;

public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<OrderApiResponse<T>> HandleResult<T>(T result, string? notFoundMessage = null)
    {
        if (result == null)
        {
            return NotFound(OrderApiResponse<T>.ErrorResponse(notFoundMessage ?? "Resource not found"));
        }

        return Ok(OrderApiResponse<T>.SuccessResponse(result));
    }

    protected ActionResult<OrderApiResponse<bool>> HandleResult(bool result, string successMessage, string errorMessage)
    {
        if (result)
        {
            return Ok(OrderApiResponse<bool>.SuccessResponse(true, successMessage));
        }

        return BadRequest(OrderApiResponse<bool>.ErrorResponse(errorMessage));
    }

    protected ActionResult<OrderApiResponse<T>> HandleError<T>(Exception ex, string message)
    {
        // Log the exception here if needed
        return StatusCode(500, OrderApiResponse<T>.ErrorResponse(message, new[] { ex.Message }));
    }
}
