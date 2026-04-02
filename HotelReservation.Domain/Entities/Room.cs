using HotelReservation.Domain.Enums;
using System;

namespace HotelReservation.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; private set; }
        public int Capacity { get; private set; }
        public decimal BasePrice { get; private set; }
        public Guid HotelId { get; private set; }
        public Hotel? Hotel { get; private set; }
        public RoomType RoomType { get; private set; }

        private static readonly Dictionary<RoomType, decimal> PriceMultipliers = new()
    {
        { RoomType.Standard, 1.0m },
        { RoomType.Deluxe,   1.5m },
        { RoomType.Suite,    2.5m }
    };



        public Room(int capacity, decimal basePrice, Guid hotelId, RoomType roomType)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            if (basePrice < 0) throw new ArgumentOutOfRangeException(nameof(basePrice));
            Id = Guid.NewGuid();
            Capacity = capacity;
            BasePrice = basePrice;
            HotelId = hotelId;
            RoomType = roomType;
        }

        private Room() { }
        public void UpdateCapacity(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
        }

        public void UpdatePrice(decimal price)
        {
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));
            BasePrice = price;
        }

        public decimal GetPricePerNight()
        {
            return BasePrice * PriceMultipliers[RoomType];

        }

        public void UpdateRoomType(RoomType roomType)
        {
            RoomType = roomType;
        }
    }
}
