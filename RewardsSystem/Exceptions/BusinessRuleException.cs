namespace RewardsSystem.Exceptions;

/// <summary>
/// Base type for any exception that represents a violated business rule
/// (as opposed to an unexpected/technical failure).
/// </summary>
public abstract class BusinessRuleException : Exception
{
    protected BusinessRuleException(string message) : base(message)
    {
    }
}
