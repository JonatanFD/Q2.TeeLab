using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Domain.Model.Commands;

public record ApplyDiscountToCartCommand(
    UserId UserId,
    Discount Discount);
