using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Model;

namespace Q2.TeeLab.OrderProcessing.Domain.Model.Entities;

public class OrderItem : BaseEntity<OrderItemId>
{
    public ProductInfo Product { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money TotalPrice { get; private set; }
    public IReadOnlyList<Discount> AppliedDiscounts { get; private set; }

    protected OrderItem() { } // EF Constructor

    public OrderItem(ProductInfo product, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        Id = new OrderItemId();
        Product = product ?? throw new ArgumentNullException(nameof(product));
        Quantity = quantity;
        UnitPrice = product.Price;
        AppliedDiscounts = product.Discounts.Where(d => d.IsValid(DateTime.UtcNow)).ToList();
        
        CalculateTotalPrice();
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(newQuantity));
            
        Quantity = newQuantity;
        CalculateTotalPrice();
    }

    public void ApplyAdditionalDiscount(Discount discount)
    {
        if (!discount.IsValid(DateTime.UtcNow))
            throw new InvalidOperationException("Discount is not valid at this time");
            
        var discounts = AppliedDiscounts.ToList();
        discounts.Add(discount);
        AppliedDiscounts = discounts.AsReadOnly();
        
        CalculateTotalPrice();
    }

    private void CalculateTotalPrice()
    {
        var baseTotal = UnitPrice * Quantity;
        var totalDiscountAmount = AppliedDiscounts.Sum(d => d.CalculateDiscountAmount(baseTotal.Amount));
        TotalPrice = new Money(Math.Max(0, baseTotal.Amount - totalDiscountAmount), baseTotal.Currency);
    }

    public Money CalculateDiscountAmount()
    {
        var baseTotal = UnitPrice * Quantity;
        var totalDiscountAmount = AppliedDiscounts.Sum(d => d.CalculateDiscountAmount(baseTotal.Amount));
        return new Money(totalDiscountAmount, baseTotal.Currency);
    }
}
