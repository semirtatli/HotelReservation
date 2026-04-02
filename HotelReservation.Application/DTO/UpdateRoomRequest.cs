using HotelReservation.Domain.Enums;

namespace HotelReservation.Application.DTO
{
    public class UpdateRoomRequest
    {
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public RoomType RoomType { get; set; }
    }
}
