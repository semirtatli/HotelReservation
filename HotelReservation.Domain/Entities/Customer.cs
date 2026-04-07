using System;

namespace HotelReservation.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public DateOnly DateOfBirth { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;

        private Customer() { }

        public Customer(string name, DateOnly dateOfBirth, string email, string phoneNumber)
        {
            Id = Guid.NewGuid();
            Name = ValidateName(name);
            DateOfBirth = ValidateDateOfBirth(dateOfBirth);
            Email = ValidateEmail(email);
            PhoneNumber = ValidatePhoneNumber(phoneNumber);
        }

        public void UpdateName(string name) => Name = ValidateName(name);
        public void UpdateDateOfBirth(DateOnly dateOfBirth) => DateOfBirth = ValidateDateOfBirth(dateOfBirth);
        public void UpdateEmail(string email) => Email = ValidateEmail(email);
        public void UpdatePhoneNumber(string phoneNumber) => PhoneNumber = ValidatePhoneNumber(phoneNumber);

        private static string ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Customer name cannot be empty or whitespace.", nameof(name));
            return name.Trim();
        }

        private static DateOnly ValidateDateOfBirth(DateOnly dateOfBirth)
        {
            if (dateOfBirth >= DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException("Date of birth must be in the past.", nameof(dateOfBirth));
            return dateOfBirth;
        }

        private static string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty or whitespace.", nameof(email));
            return email.Trim();
        }

        private static string ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty or whitespace.", nameof(phoneNumber));
            return phoneNumber.Trim();
        }
    }
}
