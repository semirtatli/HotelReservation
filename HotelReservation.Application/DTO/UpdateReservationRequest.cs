namespace HotelReservation.Application.DTO
{
    public class UpdateReservationRequest
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
