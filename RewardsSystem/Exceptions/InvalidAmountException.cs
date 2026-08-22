namespace RewardsSystem.Exceptions;

/// <summary>
/// Thrown when a purchase amount is missing, non-numeric, zero, negative,
/// or outside the allowed range.
/// </summary>
public class InvalidAmountException : BusinessRuleException
{
    public InvalidAmountException(string message) : base(message)
    {
    }
}
