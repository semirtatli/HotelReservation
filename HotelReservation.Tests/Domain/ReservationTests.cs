using FluentAssertions;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Tests.Domain;

public class ReservationTests
{
    // ───────────────────────────────────────────
    // CONSTRUCTOR
    // ───────────────────────────────────────────

    [Fact]
    public void Constructor_WhenCheckInDateIsInPast_ShouldThrowArgumentException()
    {
        var past = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        var checkOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));

        var act = () => new Reservation(past, checkOut, Guid.NewGuid(), Guid.NewGuid(), 1, 100m);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WhenCheckInDateIsToday_ShouldThrowArgumentException()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var checkOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));

        var act = () => new Reservation(today, checkOut, Guid.NewGuid(), Guid.NewGuid(), 1, 100m);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WhenCheckOutIsBeforeCheckIn_ShouldThrowArgumentException()
    {
        var checkIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
        var checkOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3));

        var act = () => new Reservation(checkIn, checkOut, Guid.NewGuid(), Guid.NewGuid(), 1, 100m);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WhenNumberOfGuestsIsZero_ShouldThrowArgumentException()
    {
        var checkIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
        var checkOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));

        var act = () => new Reservation(checkIn, checkOut, Guid.NewGuid(), Guid.NewGuid(), 0, 100m);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WhenTotalPriceIsZero_ShouldThrowArgumentException()
    {
        var checkIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
        var checkOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));

        var act = () => new Reservation(checkIn, checkOut, Guid.NewGuid(), Guid.NewGuid(), 1, 0m);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WhenTotalPriceIsNegative_ShouldThrowArgumentException()
    {
        var checkIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
        var checkOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));

        var act = () => new Reservation(checkIn, checkOut, Guid.NewGuid(), Guid.NewGuid(), 1, -50m);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WhenValidData_ShouldCreateReservation()
    {
        var checkIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
        var checkOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));
        var customerId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        var reservation = new Reservation(checkIn, checkOut, customerId, roomId, 2, 500m);

        reservation.Id.Should().NotBeEmpty();
        reservation.CheckInDate.Should().Be(checkIn);
        reservation.CheckOutDate.Should().Be(checkOut);
        reservation.CustomerId.Should().Be(customerId);
        reservation.RoomId.Should().Be(roomId);
        reservation.NumberOfGuests.Should().Be(2);
        reservation.TotalPrice.Should().Be(500m);
    }

    // ───────────────────────────────────────────
    // Update
    // ───────────────────────────────────────────

    [Fact]
    public void Update_WhenCheckInIsInPast_ShouldThrowArgumentException()
    {
        var reservation = CreateValidReservation();
        var past = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        var checkOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));

        var act = () => reservation.Update(past, checkOut, 1, 100m);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WhenCheckOutIsBeforeCheckIn_ShouldThrowArgumentException()
    {
        var reservation = CreateValidReservation();
        var checkIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
        var checkOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3));

        var act = () => reservation.Update(checkIn, checkOut, 1, 100m);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WhenNumberOfGuestsIsZero_ShouldThrowArgumentException()
    {
        var reservation = CreateValidReservation();
        var checkIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
        var checkOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));

        var act = () => reservation.Update(checkIn, checkOut, 0, 100m);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WhenValidData_ShouldUpdateReservation()
    {
        var reservation = CreateValidReservation();
        var newCheckIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7));
        var newCheckOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(14));

        reservation.Update(newCheckIn, newCheckOut, 3, 700m);

        reservation.CheckInDate.Should().Be(newCheckIn);
        reservation.CheckOutDate.Should().Be(newCheckOut);
        reservation.NumberOfGuests.Should().Be(3);
        reservation.TotalPrice.Should().Be(700m);
    }

    // ───────────────────────────────────────────
    // HELPER
    // ───────────────────────────────────────────

    private static Reservation CreateValidReservation() =>
        new(
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)),
            Guid.NewGuid(), Guid.NewGuid(), 1, 100m);
}
