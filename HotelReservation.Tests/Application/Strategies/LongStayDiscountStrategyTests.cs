using FluentAssertions;
using HotelReservation.Application.Strategies;

namespace HotelReservation.Tests.Application.Strategies;

public class LongStayDiscountStrategyTests
{
    private readonly LongStayDiscountStrategy _sut = new();

    [Theory]
    [InlineData(7)]   // tam 7 gece
    [InlineData(10)]  // 10 gece
    [InlineData(14)]  // 2 hafta
    public void GetMultiplier_WhenStayIsSevenOrMoreNights_ShouldReturn0Point9(int nights)
    {
        var checkIn = new DateOnly(2026, 10, 1);
        var checkOut = checkIn.AddDays(nights - 1); // LongStayDiscount: checkOut - checkIn + 1 >= 7

        var multiplier = _sut.GetMultiplier(checkIn, checkOut);

        multiplier.Should().Be(0.9m);
    }

    [Theory]
    [InlineData(1)]  // 1 gece
    [InlineData(3)]  // 3 gece
    [InlineData(6)]  // 6 gece (sınır altı)
    public void GetMultiplier_WhenStayIsLessThanSevenNights_ShouldReturn1Point0(int nights)
    {
        var checkIn = new DateOnly(2026, 10, 1);
        var checkOut = checkIn.AddDays(nights - 1);

        var multiplier = _sut.GetMultiplier(checkIn, checkOut);

        multiplier.Should().Be(1.0m);
    }
}
