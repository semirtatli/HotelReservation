namespace HotelReservation.Application.DTO
{
    public class CreateReservationRequest
    {
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid RoomId { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
