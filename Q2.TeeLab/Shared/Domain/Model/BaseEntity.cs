namespace Q2.TeeLab.Shared.Domain.Model;

public abstract class BaseEntity<TId>
{
    public TId Id { get; protected set; } = default!;
    
    protected BaseEntity() { }
    
    protected BaseEntity(TId id)
    {
        Id = id;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity<TId> other)
            return false;
            
        if (ReferenceEquals(this, other))
            return true;
            
        if (GetType() != other.GetType())
            return false;
            
        return Id?.Equals(other.Id) == true;
    }
    
    public override int GetHashCode()
    {
        return Id?.GetHashCode() ?? 0;
    }
    
    public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right)
    {
        return Equals(left, right);
    }
    
    public static bool operator !=(BaseEntity<TId>? left, BaseEntity<TId>? right)
    {
        return !Equals(left, right);
    }
}
