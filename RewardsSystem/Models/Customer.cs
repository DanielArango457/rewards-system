namespace RewardsSystem.Models;

/// <summary>
/// Represents a customer that can earn and redeem reward points.
/// </summary>
public class Customer
{
    public string CustomerId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
