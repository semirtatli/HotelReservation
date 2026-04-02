using HotelReservation.Domain.Enums;

namespace HotelReservation.Application.DTO
{
    public class RoomResponse
    {
        public Guid Id { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public Guid HotelId { get; set; }
        public RoomType RoomType { get; set; }
    }
}
