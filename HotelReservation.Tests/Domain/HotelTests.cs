using FluentAssertions;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Tests.Domain;

public class HotelTests
{
    // ───────────────────────────────────────────
    // CONSTRUCTOR
    // ───────────────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WhenNameIsEmptyOrWhitespace_ShouldThrowArgumentException(string name)
    {
        var act = () => new Hotel(name);

        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Fact]
    public void Constructor_WhenValidName_ShouldCreateHotel()
    {
        var hotel = new Hotel("Grand Hotel");

        hotel.Id.Should().NotBeEmpty();
        hotel.Name.Should().Be("Grand Hotel");
    }

    // ───────────────────────────────────────────
    // UpdateName
    // ───────────────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateName_WhenNameIsEmptyOrWhitespace_ShouldThrowArgumentException(string name)
    {
        var hotel = new Hotel("Grand Hotel");

        var act = () => hotel.UpdateName(name);

        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Fact]
    public void UpdateName_WhenValidName_ShouldUpdateName()
    {
        var hotel = new Hotel("Grand Hotel");

        hotel.UpdateName("Hilton");

        hotel.Name.Should().Be("Hilton");
    }
}
