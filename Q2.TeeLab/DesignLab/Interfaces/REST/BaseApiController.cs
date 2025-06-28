using Microsoft.AspNetCore.Mvc;
using Q2.TeeLab.DesignLab.Interfaces.REST.Resources;

namespace Q2.TeeLab.DesignLab.Interfaces.REST;

public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<T> HandleResult<T>(T result, string? notFoundMessage = null)
    {
        if (result == null)
        {
            return NotFound(ApiResponse<T>.ErrorResponse(notFoundMessage ?? "Resource not found"));
        }

        return Ok(ApiResponse<T>.SuccessResponse(result));
    }

    protected ActionResult HandleResult(bool result, string successMessage, string errorMessage)
    {
        if (result)
        {
            return Ok(ApiResponse<bool>.SuccessResponse(true, successMessage));
        }

        return BadRequest(ApiResponse<bool>.ErrorResponse(errorMessage));
    }

    protected ActionResult<T> HandleError<T>(Exception ex, string message)
    {
        // Log the exception here if needed
        return StatusCode(500, ApiResponse<T>.ErrorResponse(message, new[] { ex.Message }));
    }
}
