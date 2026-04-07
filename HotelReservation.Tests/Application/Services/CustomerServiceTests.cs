using FluentAssertions;
using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Domain.RepositoryInterfaces;
using HotelReservation.Application.Services;
using HotelReservation.Application.Validators;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Exceptions;
using Moq;

namespace HotelReservation.Tests.Application.Services;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _repoMock;
    private readonly CustomerService _sut;

    public CustomerServiceTests()
    {
        _repoMock = new Mock<ICustomerRepository>();
        _sut = new CustomerService(
            _repoMock.Object,
            new CustomerRequestValidator(),
            new UpdateCustomerRequestValidator());
    }

    // ───────────────────────────────────────────
    // AddCustomerAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task AddCustomerAsync_WhenRequestIsValid_ShouldCallRepoOnce()
    {
        var request = CreateValidCustomerRequest();

        await _sut.AddCustomerAsync(request);

        _repoMock.Verify(r => r.AddCustomerAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task AddCustomerAsync_WhenNameIsEmpty_ShouldThrowValidationException_AndNeverCallRepo()
    {
        var request = CreateValidCustomerRequest();
        request.Name = "";

        var act = async () => await _sut.AddCustomerAsync(request);

        await act.Should().ThrowAsync<ValidationException>();
        _repoMock.Verify(r => r.AddCustomerAsync(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task AddCustomerAsync_WhenEmailIsInvalid_ShouldThrowValidationException_AndNeverCallRepo()
    {
        var request = CreateValidCustomerRequest();
        request.Email = "gecersiz-email";

        var act = async () => await _sut.AddCustomerAsync(request);

        await act.Should().ThrowAsync<ValidationException>();
        _repoMock.Verify(r => r.AddCustomerAsync(It.IsAny<Customer>()), Times.Never);
    }

    // ───────────────────────────────────────────
    // GetAllCustomersAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task GetAllCustomersAsync_WhenNoCustomersExist_ShouldReturnEmptyList()
    {
        _repoMock.Setup(r => r.GetAllCustomersAsync()).ReturnsAsync([]);

        var result = await _sut.GetAllCustomersAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllCustomersAsync_WhenCustomersExist_ShouldReturnMappedList()
    {
        var customers = new List<Customer>
        {
            new("Ali", new DateOnly(1990, 1, 1), "ali@test.com", "555"),
            new("Veli", new DateOnly(1985, 5, 10), "veli@test.com", "444")
        };
        _repoMock.Setup(r => r.GetAllCustomersAsync()).ReturnsAsync(customers);

        var result = await _sut.GetAllCustomersAsync();

        result.Should().HaveCount(2);
        result.Select(r => r.Name).Should().Contain(["Ali", "Veli"]);
    }

    // ───────────────────────────────────────────
    // GetCustomerByIdAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task GetCustomerByIdAsync_WhenCustomerNotFound_ShouldThrowNotFoundException()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetCustomerByIdAsync(id)).ReturnsAsync((Customer?)null);

        var act = async () => await _sut.GetCustomerByIdAsync(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WhenCustomerExists_ShouldReturnMappedResponse()
    {
        var customer = new Customer("Ali", new DateOnly(1990, 1, 1), "ali@test.com", "555");
        _repoMock.Setup(r => r.GetCustomerByIdAsync(customer.Id)).ReturnsAsync(customer);

        var result = await _sut.GetCustomerByIdAsync(customer.Id);

        result.Name.Should().Be("Ali");
        result.Email.Should().Be("ali@test.com");
    }

    // ───────────────────────────────────────────
    // UpdateCustomerAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task UpdateCustomerAsync_WhenCustomerNotFound_ShouldThrowNotFoundException()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetCustomerByIdAsync(id)).ReturnsAsync((Customer?)null);

        var act = async () => await _sut.UpdateCustomerAsync(id, CreateValidUpdateRequest());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateCustomerAsync_WhenRequestIsValid_ShouldReturnUpdatedResponse()
    {
        var id = Guid.NewGuid();
        var existingCustomer = new Customer("Ali", new DateOnly(1990, 1, 1), "ali@test.com", "555");
        var updateRequest = new UpdateCustomerRequest
        {
            Name = "Yeni Ali",
            DateOfBirth = new DateOnly(1990, 1, 1),
            Email = "yeni@test.com",
            PhoneNumber = "999"
        };
        _repoMock.Setup(r => r.GetCustomerByIdAsync(id)).ReturnsAsync(existingCustomer);
        _repoMock.Setup(r => r.UpdateCustomerAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

        var result = await _sut.UpdateCustomerAsync(id, updateRequest);

        result.Name.Should().Be("Yeni Ali");
        result.Email.Should().Be("yeni@test.com");
    }

    [Fact]
    public async Task UpdateCustomerAsync_WhenEmailIsInvalid_ShouldThrowValidationException_AndNeverCallRepo()
    {
        var request = CreateValidUpdateRequest();
        request.Email = "gecersiz";

        var act = async () => await _sut.UpdateCustomerAsync(Guid.NewGuid(), request);

        await act.Should().ThrowAsync<ValidationException>();
        _repoMock.Verify(r => r.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Never);
    }

    // ───────────────────────────────────────────
    // DeleteCustomerAsync
    // ───────────────────────────────────────────

    [Fact]
    public async Task DeleteCustomerAsync_WhenCustomerNotFound_ShouldThrowNotFoundException()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.DeleteCustomerAsync(id)).ReturnsAsync((Customer?)null);

        var act = async () => await _sut.DeleteCustomerAsync(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteCustomerAsync_WhenCustomerExists_ShouldReturnDeletedCustomer()
    {
        var customer = new Customer("Ali", new DateOnly(1990, 1, 1), "ali@test.com", "555");
        _repoMock.Setup(r => r.DeleteCustomerAsync(customer.Id)).ReturnsAsync(customer);

        var result = await _sut.DeleteCustomerAsync(customer.Id);

        result.Name.Should().Be("Ali");
    }

    // ───────────────────────────────────────────
    // HELPERS
    // ───────────────────────────────────────────

    private static CustomerRequest CreateValidCustomerRequest() => new()
    {
        Name = "Ali",
        DateOfBirth = new DateOnly(1990, 1, 1),
        Email = "ali@test.com",
        PhoneNumber = "555"
    };

    private static UpdateCustomerRequest CreateValidUpdateRequest() => new()
    {
        Name = "Ali",
        DateOfBirth = new DateOnly(1990, 1, 1),
        Email = "ali@test.com",
        PhoneNumber = "555"
    };
}
