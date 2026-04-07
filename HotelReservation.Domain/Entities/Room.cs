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
            Id = Guid.NewGuid();
            Capacity = ValidateCapacity(capacity);
            BasePrice = ValidateBasePrice(basePrice);
            HotelId = hotelId;
            RoomType = roomType;
        }

        private Room() { }

        public void UpdateCapacity(int capacity) => Capacity = ValidateCapacity(capacity);

        public void UpdatePrice(decimal price) => BasePrice = ValidateBasePrice(price);

        public void UpdateRoomType(RoomType roomType) => RoomType = roomType;

        public decimal GetPricePerNight() => BasePrice * PriceMultipliers[RoomType];

        private static int ValidateCapacity(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            return capacity;
        }

        private static decimal ValidateBasePrice(decimal price)
        {
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));
            return price;
        }
    }
}
