using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Domain.Services;

public interface IPaymentServicePort
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    Task<PaymentResult> RefundPaymentAsync(string paymentId, decimal amount);
    Task<PaymentStatus> GetPaymentStatusAsync(string paymentId);
}

public record PaymentRequest(
    string OrderId,
    decimal Amount,
    string Currency,
    string PaymentMethodId,
    string CustomerId);

public record PaymentResult(
    bool IsSuccess,
    string? PaymentId,
    string? ErrorMessage,
    PaymentStatus Status);

public enum PaymentStatus
{
    Pending,
    Processing,
    Succeeded,
    Failed,
    Cancelled,
    Refunded
}
