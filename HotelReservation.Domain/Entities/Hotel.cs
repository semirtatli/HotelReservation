using System;

namespace HotelReservation.Domain.Entities
{
    public class Hotel
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        

        public Hotel( string name)
        {
            if(string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Hotel name cannot be empty or whitespace.", nameof(name));
            Id = Guid.NewGuid();
            Name = name;
            
        }

        public void UpdateName(string name) {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Hotel name cannot be empty or whitespace.", nameof(name));
            this.Name = name;
        
        }

        
    }
}
