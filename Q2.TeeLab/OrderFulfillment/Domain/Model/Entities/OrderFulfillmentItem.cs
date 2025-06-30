using Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

namespace Q2.TeeLab.OrderFulfillment.Domain.Model.Entities;

public class OrderFulfillmentItem
{
    public OrderFulfillmentItemId Id { get; private set; }
    public ProductId ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public ItemProgress Progress { get; private set; }
    public string? ProgressNotes { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EstimatedCompletionDate { get; private set; }
    public DateTime? ActualCompletionDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected OrderFulfillmentItem() 
    {
        Id = null!;
        ProductId = null!;
        ProductName = null!;
    }

    public OrderFulfillmentItem(
        ProductId productId, 
        string productName, 
        int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required", nameof(productName));

        Id = new OrderFulfillmentItemId();
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        Progress = ItemProgress.NotStarted;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProgress(ItemProgress progress, string? notes = null)
    {
        Progress = progress;
        ProgressNotes = notes;
        UpdatedAt = DateTime.UtcNow;

        if (progress == ItemProgress.InProduction && StartDate == null)
        {
            StartDate = DateTime.UtcNow;
        }

        if (progress == ItemProgress.Finished && ActualCompletionDate == null)
        {
            ActualCompletionDate = DateTime.UtcNow;
        }
    }

    public void SetEstimatedCompletionDate(DateTime estimatedDate)
    {
        EstimatedCompletionDate = estimatedDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(newQuantity));

        Quantity = newQuantity;
        UpdatedAt = DateTime.UtcNow;
    }
}
