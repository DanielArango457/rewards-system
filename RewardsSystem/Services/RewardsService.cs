using RewardsSystem.Dtos;
using RewardsSystem.Exceptions;
using RewardsSystem.Models;
using RewardsSystem.Repositories;

namespace RewardsSystem.Services;

/// <summary>
/// Implements the reward system business rules:
/// - Every $1,000 (COP) spent earns 1 point.
/// - Every point redeemed is worth $100 (COP).
/// - Points accumulate across multiple purchases.
/// - Redeeming more points than the available balance is not allowed.
/// </summary>
public class RewardsService : IRewardsService
{
    // Business rule constants.
    private const decimal PesosPerPointEarned = 1000m;
    private const decimal PesosPerPointRedeemed = 100m;

    // Reasonable input boundaries to satisfy the "allowed ranges" acceptance criterion.
    private const decimal MinPurchaseAmount = 1m;
    private const decimal MaxPurchaseAmount = 1_000_000_000m;
    private const int MinRedeemablePoints = 1;
    private const int MaxRedeemablePoints = 1_000_000_000;

    private readonly IPointsRepository _repository;

    public RewardsService(IPointsRepository repository)
    {
        _repository = repository;
    }

    public PurchaseResponse RegisterPurchase(RegisterPurchaseRequest request)
    {
        var customerId = ValidateCustomerId(request.CustomerId);
        var amount = ValidatePurchaseAmount(request.Amount);

        var pointsEarned = (int)Math.Floor(amount / PesosPerPointEarned);

        var purchase = new Purchase
        {
            CustomerId = customerId,
            Amount = amount,
            PointsEarned = pointsEarned
        };

        _repository.SavePurchase(purchase);
        _repository.AddPoints(customerId, pointsEarned);

        var newBalance = _repository.GetPointsBalance(customerId);

        return new PurchaseResponse
        {
            PurchaseId = purchase.PurchaseId,
            CustomerId = customerId,
            Amount = amount,
            PointsEarned = pointsEarned,
            NewPointsBalance = newBalance,
            Message = pointsEarned > 0
                ? $"Purchase registered successfully. {pointsEarned} point(s) earned."
                : "Purchase registered successfully. Amount was below $1,000, so no points were earned."
        };
    }

    public PointsBalanceResponse GetPointsBalance(string customerId)
    {
        var validCustomerId = ValidateCustomerId(customerId);
        var balance = _repository.GetPointsBalance(validCustomerId);

        return new PointsBalanceResponse
        {
            CustomerId = validCustomerId,
            PointsBalance = balance,
            EquivalentValueInPesos = balance * PesosPerPointRedeemed
        };
    }

    public RedeemResponse RedeemPoints(RedeemPointsRequest request)
    {
        var customerId = ValidateCustomerId(request.CustomerId);
        var points = ValidateRedeemPoints(request.Points);

        var currentBalance = _repository.GetPointsBalance(customerId);

        if (points > currentBalance)
        {
            throw new InsufficientPointsException(customerId, points, currentBalance);
        }

        _repository.SubtractPoints(customerId, points);

        var pesosValue = points * PesosPerPointRedeemed;

        var redemption = new Redemption
        {
            CustomerId = customerId,
            PointsRedeemed = points,
            PesosValue = pesosValue
        };

        _repository.SaveRedemption(redemption);

        var remainingBalance = _repository.GetPointsBalance(customerId);

        return new RedeemResponse
        {
            RedemptionId = redemption.RedemptionId,
            CustomerId = customerId,
            PointsRedeemed = points,
            PesosValue = pesosValue,
            RemainingPointsBalance = remainingBalance,
            Message = $"Redemption successful. {points} point(s) redeemed for ${pesosValue:N0}."
        };
    }

    private static string ValidateCustomerId(string? customerId)
    {
        if (string.IsNullOrWhiteSpace(customerId))
        {
            throw new InvalidCustomerException("customerId is required and cannot be empty.");
        }

        return customerId.Trim();
    }

    private static decimal ValidatePurchaseAmount(decimal? amount)
    {
        if (amount is null)
        {
            throw new InvalidAmountException("amount is required and must be a valid number.");
        }

        if (amount.Value < MinPurchaseAmount || amount.Value > MaxPurchaseAmount)
        {
            throw new InvalidAmountException(
                $"amount must be a number between {MinPurchaseAmount} and {MaxPurchaseAmount}.");
        }

        return amount.Value;
    }

    private static int ValidateRedeemPoints(int? points)
    {
        if (points is null)
        {
            throw new InvalidPointsException("points is required and must be a valid whole number.");
        }

        if (points.Value < MinRedeemablePoints || points.Value > MaxRedeemablePoints)
        {
            throw new InvalidPointsException(
                $"points must be a whole number between {MinRedeemablePoints} and {MaxRedeemablePoints}.");
        }

        return points.Value;
    }
}
