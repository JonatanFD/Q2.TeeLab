using Q2.TeeLab.OrderFulfillment.Domain.Model.Entities;
using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;
using Q2.TeeLab.Shared.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Domain.Model.Aggregates;

public class OrderFulfillment
{
    public OrderFulfillmentId Id { get; private set; }
    public Guid OrderId { get; private set; }
    public UserId CustomerId { get; private set; }
    public ManufacturerId ManufacturerId { get; private set; }
    public string ProjectName { get; private set; }
    public string? ProjectDescription { get; private set; }
    public OrderFulfillmentStatus Status { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? EstimatedDeliveryDate { get; private set; }
    public DateTime? ActualDeliveryDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string? SpecialInstructions { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<OrderFulfillmentItem> _items = new();
    public IReadOnlyCollection<OrderFulfillmentItem> Items => _items.AsReadOnly();

    protected OrderFulfillment() 
    {
        Id = null!;
        CustomerId = null!;
        ManufacturerId = null!;
        ProjectName = null!;
    }

    public OrderFulfillment(
        Guid orderId,
        UserId customerId,
        ManufacturerId manufacturerId,
        string projectName,
        string? projectDescription = null,
        string? specialInstructions = null)
    {
        if (string.IsNullOrWhiteSpace(projectName))
            throw new ArgumentException("Project name is required", nameof(projectName));

        Id = new OrderFulfillmentId();
        OrderId = orderId;
        CustomerId = customerId;
        ManufacturerId = manufacturerId;
        ProjectName = projectName;
        ProjectDescription = projectDescription;
        SpecialInstructions = specialInstructions;
        Status = OrderFulfillmentStatus.Pending;
        OrderDate = DateTime.UtcNow;
        TotalAmount = 0;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(OrderFulfillmentStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        if (newStatus == OrderFulfillmentStatus.Delivered && ActualDeliveryDate == null)
        {
            ActualDeliveryDate = DateTime.UtcNow;
        }
    }

    public void SetEstimatedDeliveryDate(DateTime estimatedDate)
    {
        EstimatedDeliveryDate = estimatedDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddItem(ProductId productId, string productName, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));

        var item = new OrderFulfillmentItem(productId, productName, quantity);
        _items.Add(item);
        
        RecalculateTotalAmount();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateItemProgress(OrderFulfillmentItemId itemId, ItemProgress progress, string? notes = null)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new ArgumentException("Item not found", nameof(itemId));

        item.UpdateProgress(progress, notes);
        UpdatedAt = DateTime.UtcNow;

        // Auto-update order status based on item progress
        UpdateOrderStatusBasedOnItemProgress();
    }

    public void RemoveItem(OrderFulfillmentItemId itemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            _items.Remove(item);
            RecalculateTotalAmount();
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public double GetCompletionPercentage()
    {
        if (!_items.Any())
            return 0;

        var completedItems = _items.Count(i => i.Progress == ItemProgress.Finished);
        return (double)completedItems / _items.Count * 100;
    }

    public void UpdateProjectInformation(string projectName, string? projectDescription = null, string? specialInstructions = null)
    {
        if (string.IsNullOrWhiteSpace(projectName))
            throw new ArgumentException("Project name is required", nameof(projectName));

        ProjectName = projectName;
        ProjectDescription = projectDescription;
        SpecialInstructions = specialInstructions;
        UpdatedAt = DateTime.UtcNow;
    }

    private void RecalculateTotalAmount()
    {
        // Note: This would need unit prices stored per item to calculate properly
        // For now, we'll just track the total amount
        UpdatedAt = DateTime.UtcNow;
    }

    private void UpdateOrderStatusBasedOnItemProgress()
    {
        if (!_items.Any())
            return;

        var allItemsFinished = _items.All(i => i.Progress == ItemProgress.Finished);
        var anyItemInProgress = _items.Any(i => i.Progress == ItemProgress.InProgress || i.Progress == ItemProgress.InProduction);
        var allItemsNotStarted = _items.All(i => i.Progress == ItemProgress.NotStarted);

        if (allItemsFinished && Status != OrderFulfillmentStatus.Completed)
        {
            UpdateStatus(OrderFulfillmentStatus.ReadyForShipment);
        }
        else if (anyItemInProgress && Status == OrderFulfillmentStatus.Pending)
        {
            UpdateStatus(OrderFulfillmentStatus.Manufacturing);
        }
    }
}
