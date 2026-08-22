namespace RewardsSystem.Dtos;

/// <summary>
/// Response returned when consulting a customer's points balance.
/// </summary>
public class PointsBalanceResponse
{
    public string CustomerId { get; set; } = string.Empty;
    public int PointsBalance { get; set; }
    public decimal EquivalentValueInPesos { get; set; }
}
