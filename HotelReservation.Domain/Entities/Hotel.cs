using System;

namespace HotelReservation.Domain.Entities
{
    public class Hotel
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public Hotel(string name)
        {
            Id = Guid.NewGuid();
            Name = ValidateName(name);
        }

        public void UpdateName(string name)
        {
            Name = ValidateName(name);
        }

        private static string ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Hotel name cannot be empty or whitespace.", nameof(name));
            return name;
        }
    }
}
