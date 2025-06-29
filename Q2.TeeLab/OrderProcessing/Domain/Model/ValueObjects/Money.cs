namespace Q2.TeeLab.OrderProcessing.Domain.Model.ValueObjects;

public record Money(decimal Amount, string Currency = "USD")
{
    public static Money Zero => new(0);
    
    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException($"Cannot add different currencies: {left.Currency} and {right.Currency}");
        
        return new Money(left.Amount + right.Amount, left.Currency);
    }
    
    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException($"Cannot subtract different currencies: {left.Currency} and {right.Currency}");
        
        return new Money(left.Amount - right.Amount, left.Currency);
    }
    
    public static Money operator *(Money money, decimal multiplier) => 
        new(money.Amount * multiplier, money.Currency);
    
    public static Money operator *(Money money, int multiplier) => 
        new(money.Amount * multiplier, money.Currency);
    
    public bool IsPositive => Amount > 0;
    public bool IsZero => Amount == 0;
    public bool IsNegative => Amount < 0;
}
