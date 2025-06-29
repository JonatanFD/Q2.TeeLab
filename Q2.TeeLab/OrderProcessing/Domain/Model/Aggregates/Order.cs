using Q2.TeeLab.OrderProcessing.Domain.Model.Entities;
using Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Model;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderProcessing.Domain.Model.Aggregates;

public class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderItem> _items = new();
    
    public UserId UserId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public Money TotalAmount { get; private set; }
    public Money DiscountAmount { get; private set; }
    public Money FinalAmount { get; private set; }
    public string? Notes { get; private set; }
    public DateTime? DeliveryDate { get; private set; }
    public string? TrackingNumber { get; private set; }
    
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    protected Order() { } // EF Constructor

    public Order(UserId userId, IEnumerable<OrderItem> items, string? notes = null)
    {
        Id = new OrderId();
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        OrderDate = DateTime.UtcNow;
        Status = OrderStatus.Draft;
        Notes = notes;
        
        if (items?.Any() != true)
            throw new ArgumentException("Order must contain at least one item", nameof(items));
            
        foreach (var item in items)
        {
            _items.Add(item);
        }
        
        CalculateTotals();
    }

    public static Order CreateFromCart(UserId userId, IEnumerable<OrderItem> cartItems, string? notes = null)
    {
        return new Order(userId, cartItems, notes);
    }

    public void AddItem(OrderItem item)
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException($"Cannot add items to order in {Status} status");
            
        var existingItem = _items.FirstOrDefault(i => i.Product.Id == item.Product.Id);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + item.Quantity);
        }
        else
        {
            _items.Add(item);
        }
        
        CalculateTotals();
    }

    public void RemoveItem(ProductId productId)
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException($"Cannot remove items from order in {Status} status");
            
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null)
        {
            _items.Remove(item);
            CalculateTotals();
        }
    }

    public void UpdateItemQuantity(ProductId productId, int newQuantity)
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException($"Cannot update items in order in {Status} status");
            
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item == null)
            throw new ArgumentException($"Item with product ID {productId} not found in order");
            
        if (newQuantity <= 0)
        {
            RemoveItem(productId);
        }
        else
        {
            item.UpdateQuantity(newQuantity);
            CalculateTotals();
        }
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        if (!Status.CanTransitionTo(newStatus))
            throw new InvalidOperationException($"Cannot transition from {Status} to {newStatus}");
            
        Status = newStatus;
        
        if (newStatus == OrderStatus.Delivered && DeliveryDate == null)
        {
            DeliveryDate = DateTime.UtcNow;
        }
    }

    public void Confirm()
    {
        ChangeStatus(OrderStatus.Confirmed);
    }

    public void Cancel()
    {
        if (!Status.CanTransitionTo(OrderStatus.Cancelled))
            throw new InvalidOperationException($"Cannot cancel order in {Status} status");
            
        ChangeStatus(OrderStatus.Cancelled);
    }

    public void SetTrackingNumber(string trackingNumber)
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException("Tracking number can only be set for shipped orders");
            
        TrackingNumber = trackingNumber ?? throw new ArgumentNullException(nameof(trackingNumber));
    }

    public void ApplyGlobalDiscount(Discount discount)
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException($"Cannot apply discounts to order in {Status} status");
            
        foreach (var item in _items)
        {
            item.ApplyAdditionalDiscount(discount);
        }
        
        CalculateTotals();
    }

    private void CalculateTotals()
    {
        if (!_items.Any())
        {
            TotalAmount = Money.Zero;
            DiscountAmount = Money.Zero;
            FinalAmount = Money.Zero;
            return;
        }

        var currency = _items.First().UnitPrice.Currency;
        TotalAmount = _items.Aggregate(new Money(0, currency), (sum, item) => sum + (item.UnitPrice * item.Quantity));
        DiscountAmount = _items.Aggregate(new Money(0, currency), (sum, item) => sum + item.CalculateDiscountAmount());
        FinalAmount = _items.Aggregate(new Money(0, currency), (sum, item) => sum + item.TotalPrice);
    }

    public bool IsEmpty => !_items.Any();
    public int TotalItemsCount => _items.Sum(i => i.Quantity);
    public bool CanBeModified => Status == OrderStatus.Draft;
    public bool IsActive => Status.IsActive();
    public bool IsCompleted => Status.IsCompleted();
}
