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
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Customer name cannot be empty or whitespace.", nameof(name));
            if (dateOfBirth >= DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException("Date of birth must be in the past.", nameof(dateOfBirth));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty or whitespace.", nameof(email));
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty or whitespace.", nameof(phoneNumber));

            Id = Guid.NewGuid();
            Name = name.Trim();
            DateOfBirth = dateOfBirth;
            Email = email.Trim();
            PhoneNumber = phoneNumber.Trim();
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Customer name cannot be empty or whitespace.", nameof(name));
            Name = name.Trim();
        }

        public void UpdateDateOfBirth(DateOnly dateOfBirth)
        {
            if (dateOfBirth >= DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException("Date of birth must be in the past.", nameof(dateOfBirth));
            DateOfBirth = dateOfBirth;
        }

        public void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty or whitespace.", nameof(email));
            Email = email.Trim();
        }

        public void UpdatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty or whitespace.", nameof(phoneNumber));
            PhoneNumber = phoneNumber.Trim();
        }
    }
}
