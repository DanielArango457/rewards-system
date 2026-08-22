namespace RewardsSystem.Models;

/// <summary>
/// Represents a points redemption made by a customer.
/// </summary>
public class Redemption
{
    public Guid RedemptionId { get; set; } = Guid.NewGuid();
    public string CustomerId { get; set; } = string.Empty;
    public int PointsRedeemed { get; set; }
    public decimal PesosValue { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
