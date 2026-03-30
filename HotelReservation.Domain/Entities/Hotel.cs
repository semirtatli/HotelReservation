using System;

namespace HotelReservation.Domain.Entities
{
    public class Hotel
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public List<Room> Rooms { get; private set; } = new List<Room>();

        public Hotel( string name)
        {
            if(name is null) throw new ArgumentNullException("name");
            Id = Guid.NewGuid();
            Name = name;
            
            
        }
    }
}
