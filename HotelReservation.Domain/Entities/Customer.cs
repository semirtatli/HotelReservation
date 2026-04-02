using System;

namespace HotelReservation.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public DateOnly DateOfBirth { get; private set; }

        public Customer( string name, DateOnly dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Customer name cannot be empty or whitespace.", nameof(name));

            if (dateOfBirth >= DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException("Date of birth must be in the past.", nameof(dateOfBirth));
            Id = Guid.NewGuid();
            Name = name.Trim();
            DateOfBirth = dateOfBirth;
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
    }
}
