using FluentAssertions;
using HotelReservation.Application.Pipeline;
using HotelReservation.Application.Strategies;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Enums;
using Moq;

namespace HotelReservation.Tests.Application.Pipeline;

public class PricingCalculationHandlerTests
{
    private readonly Mock<IPricingStrategy> _pricingStrategyMock;
    private readonly PricingCalculationHandler _sut;

    private static readonly DateOnly CheckIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
    private static readonly DateOnly CheckOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));

    public PricingCalculationHandlerTests()
    {
        _pricingStrategyMock = new Mock<IPricingStrategy>();
        _sut = new PricingCalculationHandler(_pricingStrategyMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldSetTotalPriceInContext()
    {
        _pricingStrategyMock
            .Setup(p => p.Calculate(It.IsAny<Room>(), CheckIn, CheckOut))
            .Returns(750m);

        var context = CreateContext();
        await _sut.HandleAsync(context);

        context.TotalPrice.Should().Be(750m);
    }

    [Fact]
    public async Task HandleAsync_ShouldCallNextHandler()
    {
        _pricingStrategyMock
            .Setup(p => p.Calculate(It.IsAny<Room>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .Returns(500m);

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
