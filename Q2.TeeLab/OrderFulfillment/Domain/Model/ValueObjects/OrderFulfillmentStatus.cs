namespace Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

public enum OrderFulfillmentStatus
{
    Pending = 0,
    InProgress = 1,
    MaterialsOrdered = 2,
    Manufacturing = 3,
    QualityControl = 4,
    Packaging = 5,
    ReadyForShipment = 6,
    Shipped = 7,
    Delivered = 8,
    Completed = 9,
    Cancelled = 10,
    OnHold = 11
}
