namespace RewardsSystem.Exceptions;

/// <summary>
/// Thrown when a customer tries to redeem more points than they currently have.
/// </summary>
public class InsufficientPointsException : BusinessRuleException
{
    public InsufficientPointsException(string customerId, int requested, int available)
        : base($"Customer '{customerId}' does not have enough points. " +
               $"Requested: {requested}, available: {available}.")
    {
    }
}
