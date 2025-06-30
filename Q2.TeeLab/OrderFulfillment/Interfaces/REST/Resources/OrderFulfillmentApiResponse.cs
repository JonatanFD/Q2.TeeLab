namespace Q2.TeeLab.OrderFulfillment.Interfaces.REST.Resources;

public record OrderFulfillmentApiResponse<T>(
    bool Success,
    T? Data,
    string? Message,
    IEnumerable<string>? Errors)
{
    public static OrderFulfillmentApiResponse<T> SuccessResponse(T data, string? message = null) =>
        new(true, data, message, null);

    public static OrderFulfillmentApiResponse<T> ErrorResponse(string message, IEnumerable<string>? errors = null) =>
        new(false, default, message, errors);
}
