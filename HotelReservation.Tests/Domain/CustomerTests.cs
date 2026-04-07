using FluentAssertions;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Tests.Domain;

public class CustomerTests
{
    // ───────────────────────────────────────────
    // CONSTRUCTOR
    // ───────────────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WhenNameIsEmptyOrWhitespace_ShouldThrowArgumentException(string name)
    {
        var act = () => new Customer(name, new DateOnly(1990, 1, 1), "test@test.com", "555");

        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Fact]
    public void Constructor_WhenDateOfBirthIsToday_ShouldThrowArgumentException()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        var act = () => new Customer("Ali", today, "test@test.com", "555");

        act.Should().Throw<ArgumentException>().WithParameterName("dateOfBirth");
    }

    [Fact]
    public void Constructor_WhenDateOfBirthIsInFuture_ShouldThrowArgumentException()
    {
        var future = DateOnly.FromDateTime(DateTime.Now.AddYears(1));

        var act = () => new Customer("Ali", future, "test@test.com", "555");

        act.Should().Throw<ArgumentException>().WithParameterName("dateOfBirth");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WhenEmailIsEmptyOrWhitespace_ShouldThrowArgumentException(string email)
    {
        var act = () => new Customer("Ali", new DateOnly(1990, 1, 1), email, "555");

        act.Should().Throw<ArgumentException>().WithParameterName("email");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WhenPhoneNumberIsEmptyOrWhitespace_ShouldThrowArgumentException(string phone)
    {
        var act = () => new Customer("Ali", new DateOnly(1990, 1, 1), "test@test.com", phone);

        act.Should().Throw<ArgumentException>().WithParameterName("phoneNumber");
    }

    [Fact]
    public void Constructor_WhenValidData_ShouldCreateCustomer()
    {
        var dob = new DateOnly(1990, 1, 1);

        var customer = new Customer("  Ali  ", dob, "  ali@test.com  ", "  555  ");

        customer.Id.Should().NotBeEmpty();
        customer.Name.Should().Be("Ali");
        customer.DateOfBirth.Should().Be(dob);
        customer.Email.Should().Be("ali@test.com");
        customer.PhoneNumber.Should().Be("555");
    }

    // ───────────────────────────────────────────
    // UpdateName
    // ───────────────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateName_WhenNameIsEmptyOrWhitespace_ShouldThrowArgumentException(string name)
    {
        var customer = CreateValidCustomer();

        var act = () => customer.UpdateName(name);

        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Fact]
    public void UpdateName_WhenValidName_ShouldUpdateAndTrim()
    {
        var customer = CreateValidCustomer();

        customer.UpdateName("  Veli  ");

        customer.Name.Should().Be("Veli");
    }

    // ───────────────────────────────────────────
    // UpdateDateOfBirth
    // ───────────────────────────────────────────

    [Fact]
    public void UpdateDateOfBirth_WhenDateIsToday_ShouldThrowArgumentException()
    {
        var customer = CreateValidCustomer();
        var today = DateOnly.FromDateTime(DateTime.Now);

        var act = () => customer.UpdateDateOfBirth(today);

        act.Should().Throw<ArgumentException>().WithParameterName("dateOfBirth");
    }

    [Fact]
    public void UpdateDateOfBirth_WhenDateIsInFuture_ShouldThrowArgumentException()
    {
        var customer = CreateValidCustomer();
        var future = DateOnly.FromDateTime(DateTime.Now.AddYears(1));

        var act = () => customer.UpdateDateOfBirth(future);

        act.Should().Throw<ArgumentException>().WithParameterName("dateOfBirth");
    }

    [Fact]
    public void UpdateDateOfBirth_WhenValidDate_ShouldUpdateDateOfBirth()
    {
        var customer = CreateValidCustomer();
        var newDob = new DateOnly(1985, 6, 15);

        customer.UpdateDateOfBirth(newDob);

        customer.DateOfBirth.Should().Be(newDob);
    }

    // ───────────────────────────────────────────
    // UpdateEmail
    // ───────────────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateEmail_WhenEmailIsEmptyOrWhitespace_ShouldThrowArgumentException(string email)
    {
        var customer = CreateValidCustomer();

        var act = () => customer.UpdateEmail(email);

        act.Should().Throw<ArgumentException>().WithParameterName("email");
    }

    [Fact]
    public void UpdateEmail_WhenValidEmail_ShouldUpdateAndTrim()
    {
        var customer = CreateValidCustomer();

        customer.UpdateEmail("  yeni@test.com  ");

        customer.Email.Should().Be("yeni@test.com");
    }

    // ───────────────────────────────────────────
    // UpdatePhoneNumber
    // ───────────────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdatePhoneNumber_WhenPhoneIsEmptyOrWhitespace_ShouldThrowArgumentException(string phone)
    {
        var customer = CreateValidCustomer();

        var act = () => customer.UpdatePhoneNumber(phone);

        act.Should().Throw<ArgumentException>().WithParameterName("phoneNumber");
    }

    [Fact]
    public void UpdatePhoneNumber_WhenValidPhone_ShouldUpdateAndTrim()
    {
        var customer = CreateValidCustomer();

        customer.UpdatePhoneNumber("  999  ");

        customer.PhoneNumber.Should().Be("999");
    }

    // ───────────────────────────────────────────
    // HELPER
    // ───────────────────────────────────────────

    private static Customer CreateValidCustomer() =>
        new("Ali", new DateOnly(1990, 1, 1), "ali@test.com", "555");
}
