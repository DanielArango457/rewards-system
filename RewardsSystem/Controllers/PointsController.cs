using Microsoft.AspNetCore.Mvc;
using RewardsSystem.Dtos;
using RewardsSystem.Services;

namespace RewardsSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PointsController : ControllerBase
{
    private readonly IRewardsService _rewardsService;

    public PointsController(IRewardsService rewardsService)
    {
        _rewardsService = rewardsService;
    }

    /// <summary>
    /// Returns the total accumulated points balance for a customer.
    /// </summary>
    /// <remarks>
    /// GET /api/points/{customerId}
    /// </remarks>
    [HttpGet("{customerId}")]
    [ProducesResponseType(typeof(PointsBalanceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult GetBalance(string customerId)
    {
        var response = _rewardsService.GetPointsBalance(customerId);
        return Ok(response);
    }
}
