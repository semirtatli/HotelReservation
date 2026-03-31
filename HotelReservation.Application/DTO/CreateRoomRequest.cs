namespace HotelReservation.Application.DTO
{
    public class CreateRoomRequest
    {
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public Guid HotelId { get; set; }
    }
}
