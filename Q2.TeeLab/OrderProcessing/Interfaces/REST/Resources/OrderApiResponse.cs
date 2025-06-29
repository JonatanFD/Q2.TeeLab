namespace Q2.TeeLab.OrderProcessing.Interfaces.REST.Resources;

public record OrderApiResponse<T>(
    bool Success,
    T? Data,
    string? Message,
    IEnumerable<string>? Errors)
{
    public static OrderApiResponse<T> SuccessResponse(T data, string? message = null) =>
        new(true, data, message, null);

    public static OrderApiResponse<T> ErrorResponse(string message, IEnumerable<string>? errors = null) =>
        new(false, default, message, errors);
}
