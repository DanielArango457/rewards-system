using RewardsSystem.Models;

namespace RewardsSystem.Repositories;

/// <summary>
/// Data access contract for customer points, purchases and redemptions.
/// Kept separate from the business logic (Services) and from the HTTP layer (Controllers).
/// </summary>
public interface IPointsRepository
{
    int GetPointsBalance(string customerId);

    void AddPoints(string customerId, int points);

    void SubtractPoints(string customerId, int points);

    Purchase SavePurchase(Purchase purchase);

    Redemption SaveRedemption(Redemption redemption);

    IReadOnlyList<Purchase> GetPurchaseHistory(string customerId);

    IReadOnlyList<Redemption> GetRedemptionHistory(string customerId);
}
