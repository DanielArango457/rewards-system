namespace RewardsSystem.Dtos;

/// <summary>
/// Confirmation returned after a purchase is registered.
/// </summary>
public class PurchaseResponse
{
    public Guid PurchaseId { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int PointsEarned { get; set; }
    public int NewPointsBalance { get; set; }
    public string Message { get; set; } = string.Empty;
}
