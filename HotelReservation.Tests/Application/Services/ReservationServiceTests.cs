using FluentAssertions;
using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Notifications.Observers;
using HotelReservation.Application.Pipeline;
using HotelReservation.Domain.RepositoryInterfaces;
using HotelReservation.Application.Services;
using HotelReservation.Application.Strategies;
using HotelReservation.Application.Validators;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Enums;
using HotelReservation.Domain.Events;
using HotelReservation.Domain.Exceptions;
using Moq;

namespace HotelReservation.Tests.Application.Services;

public class ReservationServiceTests
{
    private readonly Mock<IReservationRepository> _reservationRepoMock;
    private readonly Mock<IRoomRepository> _roomRepoMock;
    private readonly Mock<ICustomerRepository> _customerRepoMock;
    private readonly Mock<IPricingStrategy> _pricingStrategyMock;
    private readonly Mock<IReservationObserver> _observerMock;
    private readonly ReservationService _sut;

    private static readonly DateOnly FutureCheckIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
    private static readonly DateOnly FutureCheckOut = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));

    public ReservationServiceTests()
    {
        _reservationRepoMock = new Mock<IReservationRepository>();
        _roomRepoMock = new Mock<IRoomRepository>();
        _customerRepoMock = new Mock<ICustomerRepository>();
        _pricingStrategyMock = new Mock<IPricingStrategy>();
        _observerMock = new Mock<IReservationObserver>();

        _observerMock
            .Setup(o => o.OnReservationCreatedAsync(It.IsAny<ReservationCreatedEvent>()))
            .Returns(Task.CompletedTask);

        var availabilityHandler = new RoomAvailabilityHandler(_reservationRepoMock.Object);
        var capacityHandler = new CapacityCheckHandler();
        var pricingHandler = new PricingCalculationHandler(_pricingStrategyMock.Object);
        availabilityHandler.SetNext(capacityHandler).SetNext(pricingHandler);

        _sut = new ReservationService(
            _reservationRepoMock.Object,
            _roomRepoMock.Object,
            _customerRepoMock.Object,
            new CreateReservationRequestValidator(),
            new UpdateReservationRequestValidator(),
            [_observerMock.Object],
            availabilityHandler);
    }

    // ───────────────────────────────────────────
    // AddReservationAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task AddReservationAsync_WhenValidationFails_ShouldThrowValidationException()
    {
        var request = new CreateReservationRequest
        {
            RoomId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            NumberOfGuests = 0, // geçersiz
            CheckInDate = FutureCheckIn,
            CheckOutDate = FutureCheckOut
        };

        var act = async () => await _sut.AddReservationAsync(request);

        await act.Should().ThrowAsync<ValidationException>();
        _roomRepoMock.Verify(r => r.GetRoomByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task AddReservationAsync_WhenRoomNotFound_ShouldThrowNotFoundException()
    {
        _roomRepoMock.Setup(r => r.GetRoomByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Room?)null);

        var act = async () => await _sut.AddReservationAsync(CreateValidCreateRequest());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddReservationAsync_WhenRoomNotAvailable_ShouldThrowRoomNotAvailableException()
    {
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Standard);
        _roomRepoMock.Setup(r => r.GetRoomByIdAsync(It.IsAny<Guid>())).ReturnsAsync(room);
        _reservationRepoMock
            .Setup(r => r.IsRoomAvailableAsync(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), null))
            .ReturnsAsync(false);

        var act = async () => await _sut.AddReservationAsync(CreateValidCreateRequest());

        await act.Should().ThrowAsync<RoomNotAvailableException>();
    }

    [Fact]
    public async Task AddReservationAsync_WhenGuestsExceedCapacity_ShouldThrowArgumentException()
    {
        var room = new Room(1, 100m, Guid.NewGuid(), RoomType.Standard); // kapasite 1
        _roomRepoMock.Setup(r => r.GetRoomByIdAsync(It.IsAny<Guid>())).ReturnsAsync(room);
        _reservationRepoMock
            .Setup(r => r.IsRoomAvailableAsync(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), null))
            .ReturnsAsync(true);

        var request = CreateValidCreateRequest();
        request.NumberOfGuests = 5; // kapasiteden fazla

        var act = async () => await _sut.AddReservationAsync(request);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddReservationAsync_WhenCustomerNotFound_ShouldThrowNotFoundException()
    {
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Standard);
        _roomRepoMock.Setup(r => r.GetRoomByIdAsync(It.IsAny<Guid>())).ReturnsAsync(room);
        _reservationRepoMock
            .Setup(r => r.IsRoomAvailableAsync(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), null))
            .ReturnsAsync(true);
        _pricingStrategyMock
            .Setup(p => p.Calculate(It.IsAny<Room>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .Returns(500m);
        _customerRepoMock.Setup(r => r.GetCustomerByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Customer?)null);

        var act = async () => await _sut.AddReservationAsync(CreateValidCreateRequest());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddReservationAsync_WhenValid_ShouldSaveReservationAndNotifyObservers()
    {
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Standard);
        var customer = new Customer("Ali", new DateOnly(1990, 1, 1), "ali@test.com", "555");

        _roomRepoMock.Setup(r => r.GetRoomByIdAsync(It.IsAny<Guid>())).ReturnsAsync(room);
        _reservationRepoMock
            .Setup(r => r.IsRoomAvailableAsync(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), null))
            .ReturnsAsync(true);
        _pricingStrategyMock
            .Setup(p => p.Calculate(It.IsAny<Room>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .Returns(500m);
        _customerRepoMock.Setup(r => r.GetCustomerByIdAsync(It.IsAny<Guid>())).ReturnsAsync(customer);

        var result = await _sut.AddReservationAsync(CreateValidCreateRequest());

        _reservationRepoMock.Verify(r => r.AddReservationAsync(It.IsAny<Reservation>()), Times.Once);
        _observerMock.Verify(o => o.OnReservationCreatedAsync(It.IsAny<ReservationCreatedEvent>()), Times.Once);
        result.Should().NotBeNull();
    }

    // ───────────────────────────────────────────
    // GetAllReservationsAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task GetAllReservationsAsync_WhenNoReservationsExist_ShouldReturnEmptyList()
    {
        _reservationRepoMock.Setup(r => r.GetAllReservationsAsync()).ReturnsAsync([]);

        var result = await _sut.GetAllReservationsAsync();

        result.Should().BeEmpty();
    }

    // ───────────────────────────────────────────
    // GetReservationByIdAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task GetReservationByIdAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        var id = Guid.NewGuid();
        _reservationRepoMock.Setup(r => r.GetReservationByIdAsync(id)).ReturnsAsync((Reservation?)null);

        var act = async () => await _sut.GetReservationByIdAsync(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetReservationByIdAsync_WhenFound_ShouldReturnMappedResponse()
    {
        var reservation = CreateValidReservation();
        _reservationRepoMock.Setup(r => r.GetReservationByIdAsync(reservation.Id)).ReturnsAsync(reservation);

        var result = await _sut.GetReservationByIdAsync(reservation.Id);

        result.Should().NotBeNull();
        result.NumberOfGuests.Should().Be(reservation.NumberOfGuests);
    }

    // ───────────────────────────────────────────
    // UpdateReservationAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task UpdateReservationAsync_WhenReservationNotFound_ShouldThrowNotFoundException()
    {
        var id = Guid.NewGuid();
        _reservationRepoMock.Setup(r => r.GetReservationByIdAsync(id)).ReturnsAsync((Reservation?)null);

        var act = async () => await _sut.UpdateReservationAsync(id, CreateValidUpdateRequest());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateReservationAsync_WhenRoomNotAvailable_ShouldThrowRoomNotAvailableException()
    {
        var id = Guid.NewGuid();
        var existing = CreateValidReservation();
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Standard);
        _reservationRepoMock.Setup(r => r.GetReservationByIdAsync(id)).ReturnsAsync(existing);
        _roomRepoMock.Setup(r => r.GetRoomByIdAsync(existing.RoomId)).ReturnsAsync(room);
        _reservationRepoMock
            .Setup(r => r.IsRoomAvailableAsync(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), id))
            .ReturnsAsync(false);

        var act = async () => await _sut.UpdateReservationAsync(id, CreateValidUpdateRequest());

        await act.Should().ThrowAsync<RoomNotAvailableException>();
    }

    [Fact]
    public async Task UpdateReservationAsync_WhenValid_ShouldReturnUpdatedResponse()
    {
        var id = Guid.NewGuid();
        var existing = CreateValidReservation();
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Standard);

        _reservationRepoMock.Setup(r => r.GetReservationByIdAsync(id)).ReturnsAsync(existing);
        _reservationRepoMock
            .Setup(r => r.IsRoomAvailableAsync(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), id))
            .ReturnsAsync(true);
        _roomRepoMock.Setup(r => r.GetRoomByIdAsync(existing.RoomId)).ReturnsAsync(room);
        _pricingStrategyMock
            .Setup(p => p.Calculate(It.IsAny<Room>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .Returns(500m);
        _reservationRepoMock.Setup(r => r.UpdateReservationAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);

        var result = await _sut.UpdateReservationAsync(id, CreateValidUpdateRequest());

        result.Should().NotBeNull();
    }

    // ───────────────────────────────────────────
    // DeleteReservationAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task DeleteReservationAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        var id = Guid.NewGuid();
        _reservationRepoMock.Setup(r => r.DeleteReservationAsync(id)).ReturnsAsync((Reservation?)null);

        var act = async () => await _sut.DeleteReservationAsync(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteReservationAsync_WhenFound_ShouldReturnDeletedReservation()
    {
        var reservation = CreateValidReservation();
        _reservationRepoMock.Setup(r => r.DeleteReservationAsync(reservation.Id)).ReturnsAsync(reservation);

        var result = await _sut.DeleteReservationAsync(reservation.Id);

        result.Should().NotBeNull();
    }

    // ───────────────────────────────────────────
    // HELPERS
    // ───────────────────────────────────────────

    private static CreateReservationRequest CreateValidCreateRequest() => new()
    {
        RoomId = Guid.NewGuid(),
        CustomerId = Guid.NewGuid(),
        NumberOfGuests = 1,
        CheckInDate = FutureCheckIn,
        CheckOutDate = FutureCheckOut
    };

    private static UpdateReservationRequest CreateValidUpdateRequest() => new()
    {
        NumberOfGuests = 1,
        CheckInDate = FutureCheckIn,
        CheckOutDate = FutureCheckOut
    };

    private static Reservation CreateValidReservation() =>
        new(FutureCheckIn, FutureCheckOut, Guid.NewGuid(), Guid.NewGuid(), 1, 100m);
}
