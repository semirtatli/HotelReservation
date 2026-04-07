using FluentAssertions;
using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Application.Services;
using HotelReservation.Application.Validators;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Exceptions;
using Moq;

namespace HotelReservation.Tests.Application.Services;

public class HotelServiceTests
{
    private readonly Mock<IHotelRepository> _repoMock;
    private readonly HotelService _sut;

    public HotelServiceTests()
    {
        _repoMock = new Mock<IHotelRepository>();
        _sut = new HotelService(
            _repoMock.Object,
            new CreateHotelRequestValidator(),
            new UpdateHotelRequestValidator());
    }

    // ───────────────────────────────────────────
    // AddHotelAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task AddHotelAsync_WhenRequestIsValid_ShouldCallRepoAndReturnResponse()
    {
        var request = new CreateHotelRequest { Name = "Grand Hotel" };

        var result = await _sut.AddHotelAsync(request);

        _repoMock.Verify(r => r.AddHotelAsync(It.IsAny<Hotel>()), Times.Once);
        result.Name.Should().Be("Grand Hotel");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AddHotelAsync_WhenNameIsEmptyOrWhitespace_ShouldThrowValidationException_AndNeverCallRepo(string name)
    {
        var request = new CreateHotelRequest { Name = name };

        var act = async () => await _sut.AddHotelAsync(request);

        await act.Should().ThrowAsync<ValidationException>();
        _repoMock.Verify(r => r.AddHotelAsync(It.IsAny<Hotel>()), Times.Never);
    }

    // ───────────────────────────────────────────
    // GetAllHotelsAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task GetAllHotelsAsync_WhenNoHotelsExist_ShouldReturnEmptyList()
    {
        _repoMock.Setup(r => r.GetAllHotelsAsync()).ReturnsAsync([]);

        var result = await _sut.GetAllHotelsAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllHotelsAsync_WhenHotelsExist_ShouldReturnMappedList()
    {
        var hotels = new List<Hotel> { new("Grand Hotel"), new("Hilton") };
        _repoMock.Setup(r => r.GetAllHotelsAsync()).ReturnsAsync(hotels);

        var result = await _sut.GetAllHotelsAsync();

        result.Should().HaveCount(2);
        result.Select(h => h.Name).Should().Contain(["Grand Hotel", "Hilton"]);
    }

    // ───────────────────────────────────────────
    // GetHotelByIdAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task GetHotelByIdAsync_WhenHotelNotFound_ShouldThrowNotFoundException()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetHotelByIdAsync(id)).ReturnsAsync((Hotel?)null);

        var act = async () => await _sut.GetHotelByIdAsync(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetHotelByIdAsync_WhenHotelExists_ShouldReturnMappedResponse()
    {
        var hotel = new Hotel("Grand Hotel");
        _repoMock.Setup(r => r.GetHotelByIdAsync(hotel.Id)).ReturnsAsync(hotel);

        var result = await _sut.GetHotelByIdAsync(hotel.Id);

        result.Name.Should().Be("Grand Hotel");
    }

    // ───────────────────────────────────────────
    // UpdateHotelAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task UpdateHotelAsync_WhenHotelNotFound_ShouldThrowNotFoundException()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.UpdateHotelAsync(id, It.IsAny<Hotel>())).ReturnsAsync((Hotel?)null);

        var act = async () => await _sut.UpdateHotelAsync(id, new UpdateHotelRequest { Name = "Hilton" });

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateHotelAsync_WhenRequestIsValid_ShouldReturnUpdatedResponse()
    {
        var id = Guid.NewGuid();
        var updatedHotel = new Hotel("Hilton");
        _repoMock.Setup(r => r.UpdateHotelAsync(id, It.IsAny<Hotel>())).ReturnsAsync(updatedHotel);

        var result = await _sut.UpdateHotelAsync(id, new UpdateHotelRequest { Name = "Hilton" });

        result.Name.Should().Be("Hilton");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpdateHotelAsync_WhenNameIsEmptyOrWhitespace_ShouldThrowValidationException_AndNeverCallRepo(string name)
    {
        var act = async () => await _sut.UpdateHotelAsync(Guid.NewGuid(), new UpdateHotelRequest { Name = name });

        await act.Should().ThrowAsync<ValidationException>();
        _repoMock.Verify(r => r.UpdateHotelAsync(It.IsAny<Guid>(), It.IsAny<Hotel>()), Times.Never);
    }

    // ───────────────────────────────────────────
    // DeleteHotelAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task DeleteHotelAsync_WhenHotelNotFound_ShouldThrowNotFoundException()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.DeleteHotelAsync(id)).ReturnsAsync((Hotel?)null);

        var act = async () => await _sut.DeleteHotelAsync(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteHotelAsync_WhenHotelExists_ShouldReturnDeletedHotel()
    {
        var hotel = new Hotel("Grand Hotel");
        _repoMock.Setup(r => r.DeleteHotelAsync(hotel.Id)).ReturnsAsync(hotel);

        var result = await _sut.DeleteHotelAsync(hotel.Id);

        result.Name.Should().Be("Grand Hotel");
    }
}
