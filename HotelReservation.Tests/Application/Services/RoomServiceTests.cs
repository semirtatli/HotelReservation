using FluentAssertions;
using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Application.Services;
using HotelReservation.Application.Validators;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Enums;
using HotelReservation.Domain.Exceptions;
using Moq;

namespace HotelReservation.Tests.Application.Services;

public class RoomServiceTests
{
    private readonly Mock<IRoomRepository> _repoMock;
    private readonly RoomService _sut;

    public RoomServiceTests()
    {
        _repoMock = new Mock<IRoomRepository>();
        _sut = new RoomService(
            _repoMock.Object,
            new CreateRoomRequestValidator(),
            new UpdateRoomRequestValidator());
    }

    // ───────────────────────────────────────────
    // AddRoomAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task AddRoomAsync_WhenRequestIsValid_ShouldCallRepoAndReturnResponse()
    {
        var request = CreateValidCreateRoomRequest();

        var result = await _sut.AddRoomAsync(request);

        _repoMock.Verify(r => r.AddRoomAsync(It.IsAny<Room>()), Times.Once);
        result.Capacity.Should().Be(request.Capacity);
    }

    [Fact]
    public async Task AddRoomAsync_WhenCapacityIsZero_ShouldThrowValidationException_AndNeverCallRepo()
    {
        var request = CreateValidCreateRoomRequest();
        request.Capacity = 0;

        var act = async () => await _sut.AddRoomAsync(request);

        await act.Should().ThrowAsync<ValidationException>();
        _repoMock.Verify(r => r.AddRoomAsync(It.IsAny<Room>()), Times.Never);
    }

    [Fact]
    public async Task AddRoomAsync_WhenPriceIsNegative_ShouldThrowValidationException_AndNeverCallRepo()
    {
        var request = CreateValidCreateRoomRequest();
        request.Price = -1m;

        var act = async () => await _sut.AddRoomAsync(request);

        await act.Should().ThrowAsync<ValidationException>();
        _repoMock.Verify(r => r.AddRoomAsync(It.IsAny<Room>()), Times.Never);
    }

    // ───────────────────────────────────────────
    // GetAllRoomsAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task GetAllRoomsAsync_WhenNoRoomsExist_ShouldReturnEmptyList()
    {
        _repoMock.Setup(r => r.GetAllRoomsAsync()).ReturnsAsync([]);

        var result = await _sut.GetAllRoomsAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllRoomsAsync_WhenRoomsExist_ShouldReturnMappedList()
    {
        var rooms = new List<Room>
        {
            new(2, 100m, Guid.NewGuid(), RoomType.Standard),
            new(4, 200m, Guid.NewGuid(), RoomType.Suite)
        };
        _repoMock.Setup(r => r.GetAllRoomsAsync()).ReturnsAsync(rooms);

        var result = await _sut.GetAllRoomsAsync();

        result.Should().HaveCount(2);
    }

    // ───────────────────────────────────────────
    // GetRoomByIdAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task GetRoomByIdAsync_WhenRoomNotFound_ShouldThrowNotFoundException()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetRoomByIdAsync(id)).ReturnsAsync((Room?)null);

        var act = async () => await _sut.GetRoomByIdAsync(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetRoomByIdAsync_WhenRoomExists_ShouldReturnMappedResponse()
    {
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Standard);
        _repoMock.Setup(r => r.GetRoomByIdAsync(room.Id)).ReturnsAsync(room);

        var result = await _sut.GetRoomByIdAsync(room.Id);

        result.Capacity.Should().Be(2);
    }

    // ───────────────────────────────────────────
    // UpdateRoomAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task UpdateRoomAsync_WhenRoomNotFound_ShouldThrowNotFoundException()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.UpdateRoomAsync(id, It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<RoomType>()))
                 .ReturnsAsync((Room?)null);

        var act = async () => await _sut.UpdateRoomAsync(id, CreateValidUpdateRoomRequest());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateRoomAsync_WhenRequestIsValid_ShouldReturnUpdatedResponse()
    {
        var id = Guid.NewGuid();
        var updatedRoom = new Room(3, 150m, Guid.NewGuid(), RoomType.Deluxe);
        _repoMock.Setup(r => r.UpdateRoomAsync(id, It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<RoomType>()))
                 .ReturnsAsync(updatedRoom);

        var result = await _sut.UpdateRoomAsync(id, CreateValidUpdateRoomRequest());

        result.Capacity.Should().Be(3);
    }

    [Fact]
    public async Task UpdateRoomAsync_WhenCapacityIsZero_ShouldThrowValidationException_AndNeverCallRepo()
    {
        var request = CreateValidUpdateRoomRequest();
        request.Capacity = 0;

        var act = async () => await _sut.UpdateRoomAsync(Guid.NewGuid(), request);

        await act.Should().ThrowAsync<ValidationException>();
        _repoMock.Verify(r => r.UpdateRoomAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<RoomType>()), Times.Never);
    }

    // ───────────────────────────────────────────
    // DeleteRoomAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task DeleteRoomAsync_WhenRoomNotFound_ShouldThrowNotFoundException()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.DeleteRoomAsync(id)).ReturnsAsync((Room?)null);

        var act = async () => await _sut.DeleteRoomAsync(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteRoomAsync_WhenRoomExists_ShouldReturnDeletedRoom()
    {
        var room = new Room(2, 100m, Guid.NewGuid(), RoomType.Standard);
        _repoMock.Setup(r => r.DeleteRoomAsync(room.Id)).ReturnsAsync(room);

        var result = await _sut.DeleteRoomAsync(room.Id);

        result.Capacity.Should().Be(2);
    }

    // ───────────────────────────────────────────
    // HELPERS
    // ───────────────────────────────────────────

    private static CreateRoomRequest CreateValidCreateRoomRequest() => new()
    {
        Capacity = 2,
        Price = 100m,
        HotelId = Guid.NewGuid(),
        RoomType = RoomType.Standard
    };

    private static UpdateRoomRequest CreateValidUpdateRoomRequest() => new()
    {
        Capacity = 3,
        Price = 150m,
        RoomType = RoomType.Deluxe
    };
}
