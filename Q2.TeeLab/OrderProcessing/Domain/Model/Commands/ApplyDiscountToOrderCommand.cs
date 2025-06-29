using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Domain.Model.Commands;

public record ApplyDiscountToOrderCommand(
    OrderId OrderId,
    Discount Discount);
