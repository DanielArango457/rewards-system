using Microsoft.AspNetCore.Mvc;
using RewardsSystem.Dtos;
using RewardsSystem.Services;

namespace RewardsSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RedemptionsController : ControllerBase
{
    private readonly IRewardsService _rewardsService;

    public RedemptionsController(IRewardsService rewardsService)
    {
        _rewardsService = rewardsService;
    }

    /// <summary>
    /// Redeems points on behalf of a customer, converting them into a pesos value.
    /// Fails with a clear error if the customer does not have enough points.
    /// </summary>
    /// <remarks>
    /// POST /api/redemptions
    /// Body: { "customerId": "C001", "points": 10 }
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(RedeemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult RedeemPoints([FromBody] RedeemPointsRequest request)
    {
        var response = _rewardsService.RedeemPoints(request);
        return Ok(response);
    }
}
