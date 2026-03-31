namespace HotelReservation.Application.DTO
{
    public class CreateReservationRequest
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid RoomId { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
