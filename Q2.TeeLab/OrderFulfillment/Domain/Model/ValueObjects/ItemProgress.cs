namespace Q2.TeeLab.OrderFulfillment.Domain.Model.ValueObjects;

public enum ItemProgress
{
    NotStarted = 0,
    InProgress = 1,
    MaterialsGathered = 2,
    InProduction = 3,
    QualityCheck = 4,
    Finished = 5,
    Defective = 6,
    OnHold = 7
}
