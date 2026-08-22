namespace RewardsSystem.Models;

/// <summary>
/// Represents a single purchase made by a customer and the points it generated.
/// </summary>
public class Purchase
{
    public Guid PurchaseId { get; set; } = Guid.NewGuid();
    public string CustomerId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int PointsEarned { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
