namespace RewardsSystem.Exceptions;

/// <summary>
/// Thrown when the number of points requested for redemption is missing,
/// non-numeric, zero, negative, or otherwise out of the allowed range.
/// </summary>
public class InvalidPointsException : BusinessRuleException
{
    public InvalidPointsException(string message) : base(message)
    {
    }
}
