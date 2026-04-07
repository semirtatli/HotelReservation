using FluentAssertions;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Enums;

namespace HotelReservation.Tests.Domain;

public class RoomTests
{
    // ───────────────────────────────────────────
    // CONSTRUCTOR
    // ───────────────────────────────────────────

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WhenCapacityIsZeroOrNegative_ShouldThrowArgumentOutOfRangeException(int capacity)
    {
        var act = () => new Room(capacity, 100m, Guid.NewGuid(), RoomType.Standard);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WhenBasePriceIsNegative_ShouldThrowArgumentOutOfRangeException()
    {
        var act = () => new Room(2, -1m, Guid.NewGuid(), RoomType.Standard);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WhenValidData_ShouldCreateRoom()
    {
        var hotelId = Guid.NewGuid();

        var room = new Room(3, 200m, hotelId, RoomType.Deluxe);

        room.Id.Should().NotBeEmpty();
        room.Capacity.Should().Be(3);
        room.BasePrice.Should().Be(200m);
        room.HotelId.Should().Be(hotelId);
        room.RoomType.Should().Be(RoomType.Deluxe);
    }

    // ───────────────────────────────────────────
    // GetPricePerNight
    // ───────────────────────────────────────────

    [Fact]
    public void GetPricePerNight_WhenStandard_ShouldReturnBasePrice()
    {
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Standard);

        var price = room.GetPricePerNight();

        price.Should().Be(100m); // 100 * 1.0
    }

    [Fact]
    public void GetPricePerNight_WhenDeluxe_ShouldReturnBasePriceMultipliedBy1Point5()
    {
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Deluxe);

        var price = room.GetPricePerNight();

        price.Should().Be(150m); // 100 * 1.5
    }

    [Fact]
    public void GetPricePerNight_WhenSuite_ShouldReturnBasePriceMultipliedBy2Point5()
    {
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Suite);

        var price = room.GetPricePerNight();

        price.Should().Be(250m); // 100 * 2.5
    }

    // ───────────────────────────────────────────
    // UpdateCapacity
    // ───────────────────────────────────────────

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void UpdateCapacity_WhenCapacityIsZeroOrNegative_ShouldThrowArgumentOutOfRangeException(int capacity)
    {
        var room = CreateValidRoom();

        var act = () => room.UpdateCapacity(capacity);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void UpdateCapacity_WhenValidCapacity_ShouldUpdateCapacity()
    {
        var room = CreateValidRoom();

        room.UpdateCapacity(5);

        room.Capacity.Should().Be(5);
    }

    // ───────────────────────────────────────────
    // UpdatePrice
    // ───────────────────────────────────────────

    [Fact]
    public void UpdatePrice_WhenPriceIsNegative_ShouldThrowArgumentOutOfRangeException()
    {
        var room = CreateValidRoom();

        var act = () => room.UpdatePrice(-1m);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void UpdatePrice_WhenValidPrice_ShouldUpdatePrice()
    {
        var room = CreateValidRoom();

        room.UpdatePrice(300m);

        room.BasePrice.Should().Be(300m);
    }

    // ───────────────────────────────────────────
    // HELPER
    // ───────────────────────────────────────────

    private static Room CreateValidRoom() =>
        new(2, 100m, Guid.NewGuid(), RoomType.Standard);
}
