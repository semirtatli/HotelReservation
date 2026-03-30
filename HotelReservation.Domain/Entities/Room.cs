using System;

namespace HotelReservation.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; private set; }
        public int Capacity { get; private set; }
        public decimal Price { get; private set; }
        public Guid HotelId { get; private set; }
        public Hotel? Hotel { get; private set; }

        public Room(int capacity, decimal price, Guid hotelId)
        {
            if(capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            if(price < 0) throw new ArgumentOutOfRangeException(nameof(price));
            Id = Guid.NewGuid();
            Capacity = capacity;
            Price = price;
            HotelId = hotelId;
        }
    }
}
