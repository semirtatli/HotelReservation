using FluentAssertions;
using HotelReservation.Application.Pipeline;
using HotelReservation.Domain.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Enums;
using HotelReservation.Domain.Exceptions;
using Moq;

namespace HotelReservation.Tests.Application.Pipeline;

public class RoomAvailabilityHandlerTests
{
    private readonly Mock<IReservationRepository> _repoMock;
    private readonly RoomAvailabilityHandler _sut;

    private static readonly DateOnly CheckIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
    private static readonly DateOnly CheckOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));

    public RoomAvailabilityHandlerTests()
    {
        _repoMock = new Mock<IReservationRepository>();
        _sut = new RoomAvailabilityHandler(_repoMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenRoomNotAvailable_ShouldThrowRoomNotAvailableException()
    {
        _repoMock
            .Setup(r => r.IsRoomAvailableAsync(It.IsAny<Guid>(), CheckIn, CheckOut, null))
            .ReturnsAsync(false);

        var context = CreateContext();
        var act = async () => await _sut.HandleAsync(context);

        await act.Should().ThrowAsync<RoomNotAvailableException>();
    }

    [Fact]
    public async Task HandleAsync_WhenRoomIsAvailable_ShouldCallNextHandler()
    {
        _repoMock
            .Setup(r => r.IsRoomAvailableAsync(It.IsAny<Guid>(), CheckIn, CheckOut, null))
            .ReturnsAsync(true);

        var nextMock = new Mock<IReservationHandler>();
        nextMock.Setup(n => n.HandleAsync(It.IsAny<ReservationContext>())).Returns(Task.CompletedTask);
        _sut.SetNext(nextMock.Object);

        var context = CreateContext();
        await _sut.HandleAsync(context);

        nextMock.Verify(n => n.HandleAsync(context), Times.Once);
    }

    private static ReservationContext CreateContext() => new()
    {
        RoomId = Guid.NewGuid(),
        CheckInDate = CheckIn,
        CheckOutDate = CheckOut,
        NumberOfGuests = 1,
        Room = new Room(2, 100m, Guid.NewGuid(), RoomType.Standard)
    };
}
