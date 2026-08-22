namespace RewardsSystem.Models;

/// <summary>
/// Represents the accumulated points balance for a customer.
/// </summary>
public class PointsAccount
{
    public string CustomerId { get; set; } = string.Empty;
    public int PointsBalance { get; set; }
}
