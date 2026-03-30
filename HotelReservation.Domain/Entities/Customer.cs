using System;

namespace HotelReservation.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public DateTime DateOfBirth { get; private set; }

        public Customer( string name, DateTime dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty or whitespace.", nameof(name));

            if (dateOfBirth >= DateTime.UtcNow)
                throw new ArgumentException("Date of birth must be in the past.", nameof(dateOfBirth));
            Id = Guid.NewGuid();
            Name = name.Trim();
            DateOfBirth = dateOfBirth;
        }
    }
}
