namespace RewardsSystem.Dtos;

/// <summary>
/// Confirmation returned after points are redeemed.
/// </summary>
public class RedeemResponse
{
    public Guid RedemptionId { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public int PointsRedeemed { get; set; }
    public decimal PesosValue { get; set; }
    public int RemainingPointsBalance { get; set; }
    public string Message { get; set; } = string.Empty;
}
