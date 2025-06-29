using Q2.TeeLab.OrderProcessing.Domain.Model.Entities;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Model;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;

public class Cart : AggregateRoot<CartId>
{
    private readonly List<OrderItem> _items = new();
    
    public UserId UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime LastUpdated { get; private set; }
    public Money TotalAmount { get; private set; }
    
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    protected Cart() { } // EF Constructor

    public Cart(UserId userId)
    {
        Id = new CartId();
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        CreatedAt = DateTime.UtcNow;
        LastUpdated = DateTime.UtcNow;
        TotalAmount = Money.Zero;
    }

    public void AddItem(ProductInfo product, int quantity = 1)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        var existingItem = _items.FirstOrDefault(i => i.Product.Id == product.Id);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var newItem = new OrderItem(product, quantity);
            _items.Add(newItem);
        }

        UpdateTimestamp();
        CalculateTotal();
    }

    public void RemoveItem(ProductId productId)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null)
        {
            _items.Remove(item);
            UpdateTimestamp();
            CalculateTotal();
        }
    }

    public void UpdateItemQuantity(ProductId productId, int newQuantity)
    {
        if (newQuantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(newQuantity));

        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item == null)
            throw new ArgumentException($"Item with product ID {productId} not found in cart");

        if (newQuantity == 0)
        {
            RemoveItem(productId);
        }
        else
        {
            item.UpdateQuantity(newQuantity);
            UpdateTimestamp();
            CalculateTotal();
        }
    }

    public void Clear()
    {
        _items.Clear();
        UpdateTimestamp();
        CalculateTotal();
    }

    public Order ConvertToOrder(string? notes = null)
    {
        if (IsEmpty)
            throw new InvalidOperationException("Cannot create order from empty cart");

        // Create copies of items for the order
        var orderItems = _items.Select(item => new OrderItem(item.Product, item.Quantity)).ToList();
        return new Order(UserId, orderItems, notes);
    }

    public void ApplyDiscount(Discount discount)
    {
        foreach (var item in _items)
        {
            if (discount.IsValid(DateTime.UtcNow))
            {
                item.ApplyAdditionalDiscount(discount);
            }
        }
        
        UpdateTimestamp();
        CalculateTotal();
    }

    private void UpdateTimestamp()
    {
        LastUpdated = DateTime.UtcNow;
    }

    private void CalculateTotal()
    {
        if (!_items.Any())
        {
            TotalAmount = Money.Zero;
            return;
        }

        var currency = _items.First().UnitPrice.Currency;
        TotalAmount = _items.Aggregate(new Money(0, currency), (sum, item) => sum + item.TotalPrice);
    }

    public bool IsEmpty => !_items.Any();
    public int TotalItemsCount => _items.Sum(i => i.Quantity);
    public Money TotalDiscountAmount => _items.Aggregate(Money.Zero, (sum, item) => sum + item.CalculateDiscountAmount());
}
