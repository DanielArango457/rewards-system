namespace RewardsSystem.Dtos;

/// <summary>
/// Standard error payload returned for any failed operation.
/// </summary>
public class ErrorResponse
{
    public string Error { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
