using System;

namespace HotelReservation.Domain.Entities
{
    public class Hotel
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        

        public Hotel( string name)
        {
            if(name is null) throw new ArgumentNullException("name");
            Id = Guid.NewGuid();
            Name = name;
            
        }

        public void UpdateName(string name) {

            this.Name = name;
        
        }

        
    }
}
