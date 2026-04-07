using FluentAssertions;
using HotelReservation.Application.Strategies;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Enums;
using Moq;

namespace HotelReservation.Tests.Application.Strategies;

public class CompositePricingStrategyTests
{
    private static readonly DateOnly CheckIn = new(2026, 10, 1);
    private static readonly DateOnly CheckOut = new(2026, 10, 4); // 4 gece

    [Fact]
    public void Calculate_WhenNoModifiers_ShouldReturnBasePrice()
    {
        // Standard room: basePrice * 1.0 = 100, 4 gece = 400
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Standard);
        var sut = new CompositePricingStrategy([]);

        var price = sut.Calculate(room, CheckIn, CheckOut);

        price.Should().Be(400m); // 100 * 1.0 * 4 gece
    }

    [Fact]
    public void Calculate_WhenOneModifierApplied_ShouldApplyMultiplier()
    {
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Standard);
        var modifierMock = new Mock<IPricingModifier>();
        modifierMock.Setup(m => m.GetMultiplier(CheckIn, CheckOut)).Returns(1.3m);

        var sut = new CompositePricingStrategy([modifierMock.Object]);

        var price = sut.Calculate(room, CheckIn, CheckOut);

        price.Should().Be(520m); // 100 * 4 * 1.3
    }

    [Fact]
    public void Calculate_WhenMultipleModifiersApplied_ShouldApplyAllMultipliers()
    {
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Standard);
        var modifier1 = new Mock<IPricingModifier>();
        var modifier2 = new Mock<IPricingModifier>();
        modifier1.Setup(m => m.GetMultiplier(CheckIn, CheckOut)).Returns(1.3m);
        modifier2.Setup(m => m.GetMultiplier(CheckIn, CheckOut)).Returns(0.9m);

        var sut = new CompositePricingStrategy([modifier1.Object, modifier2.Object]);

        var price = sut.Calculate(room, CheckIn, CheckOut);

        price.Should().Be(468m); // 100 * 4 * 1.3 * 0.9
    }

    [Fact]
    public void Calculate_WhenDeluxeRoom_ShouldApplyRoomTypeMultiplier()
    {
        // Deluxe: basePrice * 1.5 = 150 per night, 4 gece = 600
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Deluxe);
        var sut = new CompositePricingStrategy([]);

        var price = sut.Calculate(room, CheckIn, CheckOut);

        price.Should().Be(600m); // 100 * 1.5 * 4
    }
}
