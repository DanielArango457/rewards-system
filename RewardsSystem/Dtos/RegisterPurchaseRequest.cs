using System.ComponentModel.DataAnnotations;

namespace RewardsSystem.Dtos;

/// <summary>
/// Payload required to register a new purchase.
/// </summary>
public class RegisterPurchaseRequest
{
    [Required(ErrorMessage = "customerId is required.")]
    public string CustomerId { get; set; } = string.Empty;

    [Required(ErrorMessage = "amount is required.")]
    public decimal? Amount { get; set; }
}
