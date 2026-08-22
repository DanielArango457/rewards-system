using System.ComponentModel.DataAnnotations;

namespace RewardsSystem.Dtos;

/// <summary>
/// Payload required to redeem points for a customer.
/// </summary>
public class RedeemPointsRequest
{
    [Required(ErrorMessage = "customerId is required.")]
    public string CustomerId { get; set; } = string.Empty;

    [Required(ErrorMessage = "points is required.")]
    public int? Points { get; set; }
}
