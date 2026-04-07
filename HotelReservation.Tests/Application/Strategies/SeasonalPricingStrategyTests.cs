using FluentAssertions;
using HotelReservation.Application.Strategies;

namespace HotelReservation.Tests.Application.Strategies;

public class SeasonalPricingStrategyTests
{
    private readonly SeasonalPricingStrategy _sut = new();

    [Theory]
    [InlineData(6)]  // Haziran
    [InlineData(7)]  // Temmuz
    [InlineData(8)]  // Ağustos
    public void GetMultiplier_WhenSummerMonth_ShouldReturn1Point3(int month)
    {
        var checkIn = new DateOnly(2026, month, 1);
        var checkOut = new DateOnly(2026, month, 10);

        var multiplier = _sut.GetMultiplier(checkIn, checkOut);

        multiplier.Should().Be(1.3m);
    }

    [Theory]
    [InlineData(1)]   // Ocak
    [InlineData(3)]   // Mart
    [InlineData(5)]   // Mayıs
    [InlineData(9)]   // Eylül
    [InlineData(12)]  // Aralık
    public void GetMultiplier_WhenNonSummerMonth_ShouldReturn1Point0(int month)
    {
        var checkIn = new DateOnly(2026, month, 1);
        var checkOut = new DateOnly(2026, month, 10);

        var multiplier = _sut.GetMultiplier(checkIn, checkOut);

        multiplier.Should().Be(1.0m);
    }
}
