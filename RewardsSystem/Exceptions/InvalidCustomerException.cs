namespace RewardsSystem.Exceptions;

/// <summary>
/// Thrown when the customer identifier supplied in a request is missing or invalid.
/// </summary>
public class InvalidCustomerException : BusinessRuleException
{
    public InvalidCustomerException(string message) : base(message)
    {
    }
}
