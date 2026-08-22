using RewardsSystem.Dtos;

namespace RewardsSystem.Services;

/// <summary>
/// Business logic contract for the rewards/points program.
/// </summary>
public interface IRewardsService
{
    PurchaseResponse RegisterPurchase(RegisterPurchaseRequest request);

    PointsBalanceResponse GetPointsBalance(string customerId);

    RedeemResponse RedeemPoints(RedeemPointsRequest request);

    // Note: RegisterPurchaseRequest.Amount and RedeemPointsRequest.Points are nullable
    // (decimal?/int?) so that a missing field can be told apart from a value of zero.
}
