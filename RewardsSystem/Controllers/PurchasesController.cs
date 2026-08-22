using Microsoft.AspNetCore.Mvc;
using RewardsSystem.Dtos;
using RewardsSystem.Services;

namespace RewardsSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchasesController : ControllerBase
{
    private readonly IRewardsService _rewardsService;

    public PurchasesController(IRewardsService rewardsService)
    {
        _rewardsService = rewardsService;
    }

    /// <summary>
    /// Registers a purchase and calculates the reward points earned.
    /// </summary>
    /// <remarks>
    /// POST /api/purchases
    /// Body: { "customerId": "C001", "amount": 25000 }
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(PurchaseResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult RegisterPurchase([FromBody] RegisterPurchaseRequest request)
    {
        var response = _rewardsService.RegisterPurchase(request);
        return CreatedAtAction(nameof(RegisterPurchase), new { id = response.PurchaseId }, response);
    }
}
