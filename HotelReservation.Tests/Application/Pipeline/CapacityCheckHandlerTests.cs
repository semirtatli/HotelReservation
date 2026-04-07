using FluentAssertions;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Pipeline;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Enums;
using Moq;

namespace HotelReservation.Tests.Application.Pipeline;

public class CapacityCheckHandlerTests
{
    private readonly CapacityCheckHandler _sut = new();

    private static readonly DateOnly CheckIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
    private static readonly DateOnly CheckOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));

    [Fact]
    public async Task HandleAsync_WhenGuestsExceedCapacity_ShouldThrowArgumentException()
    {
        var context = CreateContext(numberOfGuests: 5, roomCapacity: 2);

        var act = async () => await _sut.HandleAsync(context);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task HandleAsync_WhenGuestsEqualCapacity_ShouldCallNextHandler()
    {
        var nextMock = new Mock<IReservationHandler>();
        nextMock.Setup(n => n.HandleAsync(It.IsAny<ReservationContext>())).Returns(Task.CompletedTask);
        _sut.SetNext(nextMock.Object);

        var context = CreateContext(numberOfGuests: 2, roomCapacity: 2);
        await _sut.HandleAsync(context);

        nextMock.Verify(n => n.HandleAsync(context), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenGuestsBelowCapacity_ShouldCallNextHandler()
    {
        var nextMock = new Mock<IReservationHandler>();
        nextMock.Setup(n => n.HandleAsync(It.IsAny<ReservationContext>())).Returns(Task.CompletedTask);
        _sut.SetNext(nextMock.Object);

        var context = CreateContext(numberOfGuests: 1, roomCapacity: 2);
        await _sut.HandleAsync(context);

        nextMock.Verify(n => n.HandleAsync(context), Times.Once);
    }

    private static ReservationContext CreateContext(int numberOfGuests, int roomCapacity) => new()
    {
        Request = new CreateReservationRequest
        {
            RoomId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            NumberOfGuests = numberOfGuests,
            CheckInDate = CheckIn,
            CheckOutDate = CheckOut
        },
        Room = new Room(roomCapacity, 100m, Guid.NewGuid(), RoomType.Standard)
    };
}
