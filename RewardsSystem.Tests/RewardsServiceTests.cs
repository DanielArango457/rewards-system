using RewardsSystem.Dtos;
using RewardsSystem.Exceptions;
using RewardsSystem.Repositories;
using RewardsSystem.Services;
using Xunit;

namespace RewardsSystem.Tests;

public class RewardsServiceTests
{
    private static RewardsService CreateService()
    {
        return new RewardsService(new InMemoryPointsRepository());
    }

    [Fact]
    public void RegisterPurchase_WithValidAmount_EarnsExpectedPoints()
    {
        var service = CreateService();

        var response = service.RegisterPurchase(new RegisterPurchaseRequest
        {
            CustomerId = "C001",
            Amount = 25000
        });

        Assert.Equal(25, response.PointsEarned);
        Assert.Equal(25, response.NewPointsBalance);
    }

    [Fact]
    public void RegisterPurchase_AccumulatesPointsAcrossMultiplePurchases()
    {
        var service = CreateService();

        service.RegisterPurchase(new RegisterPurchaseRequest { CustomerId = "C001", Amount = 10000 });
        service.RegisterPurchase(new RegisterPurchaseRequest { CustomerId = "C001", Amount = 5000 });

        var balance = service.GetPointsBalance("C001");

        Assert.Equal(15, balance.PointsBalance);
    }

    [Fact]
    public void RegisterPurchase_WithNegativeAmount_ThrowsInvalidAmountException()
    {
        var service = CreateService();

        Assert.Throws<InvalidAmountException>(() =>
            service.RegisterPurchase(new RegisterPurchaseRequest { CustomerId = "C001", Amount = -100 }));
    }

    [Fact]
    public void RegisterPurchase_WithMissingCustomerId_ThrowsInvalidCustomerException()
    {
        var service = CreateService();

        Assert.Throws<InvalidCustomerException>(() =>
            service.RegisterPurchase(new RegisterPurchaseRequest { CustomerId = "", Amount = 1000 }));
    }

    [Fact]
    public void RedeemPoints_WithSufficientBalance_SucceedsAndUpdatesBalance()
    {
        var service = CreateService();
        service.RegisterPurchase(new RegisterPurchaseRequest { CustomerId = "C001", Amount = 20000 });

        var response = service.RedeemPoints(new RedeemPointsRequest { CustomerId = "C001", Points = 5 });

        Assert.Equal(500, response.PesosValue);
        Assert.Equal(15, response.RemainingPointsBalance);
    }

    [Fact]
    public void RedeemPoints_WithInsufficientBalance_ThrowsInsufficientPointsException()
    {
        var service = CreateService();
        service.RegisterPurchase(new RegisterPurchaseRequest { CustomerId = "C001", Amount = 1000 });

        Assert.Throws<InsufficientPointsException>(() =>
            service.RedeemPoints(new RedeemPointsRequest { CustomerId = "C001", Points = 100 }));
    }

    [Fact]
    public void RedeemPoints_WithZeroOrNegativePoints_ThrowsInvalidPointsException()
    {
        var service = CreateService();

        Assert.Throws<InvalidPointsException>(() =>
            service.RedeemPoints(new RedeemPointsRequest { CustomerId = "C001", Points = 0 }));
    }
  
    [Fact]
    public void GetPointsBalance_ForNewCustomerWithNoPurchases_ReturnsZero()
    {
        var service = CreateService();

        var balance = service.GetPointsBalance("NEW_CUSTOMER");

        Assert.Equal(0, balance.PointsBalance);
    }  
}
