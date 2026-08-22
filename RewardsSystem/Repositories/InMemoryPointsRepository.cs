using System.Collections.Concurrent;
using RewardsSystem.Models;

namespace RewardsSystem.Repositories;

/// <summary>
/// Simple thread-safe in-memory storage.
/// This can be swapped for a real database implementation later
/// (e.g. Entity Framework Core) without touching the Services or Controllers layers,
/// since both depend on the IPointsRepository abstraction only.
/// </summary>
public class InMemoryPointsRepository : IPointsRepository
{
    private readonly ConcurrentDictionary<string, int> _balances = new();
    private readonly ConcurrentBag<Purchase> _purchases = new();
    private readonly ConcurrentBag<Redemption> _redemptions = new();
    private readonly object _lock = new();

    public int GetPointsBalance(string customerId)
    {
        return _balances.GetValueOrDefault(customerId, 0);
    }

    public void AddPoints(string customerId, int points)
    {
        lock (_lock)
        {
            _balances[customerId] = GetPointsBalance(customerId) + points;
        }
    }

    public void SubtractPoints(string customerId, int points)
    {
        lock (_lock)
        {
            _balances[customerId] = GetPointsBalance(customerId) - points;
        }
    }

    public Purchase SavePurchase(Purchase purchase)
    {
        _purchases.Add(purchase);
        return purchase;
    }

    public Redemption SaveRedemption(Redemption redemption)
    {
        _redemptions.Add(redemption);
        return redemption;
    }

    public IReadOnlyList<Purchase> GetPurchaseHistory(string customerId)
    {
        return _purchases
            .Where(p => p.CustomerId == customerId)
            .OrderByDescending(p => p.CreatedAt)
            .ToList();
    }

    public IReadOnlyList<Redemption> GetRedemptionHistory(string customerId)
    {
        return _redemptions
            .Where(r => r.CustomerId == customerId)
            .OrderByDescending(r => r.CreatedAt)
            .ToList();
    }
}
