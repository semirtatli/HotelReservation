namespace HotelReservation.Application.DTO
{
    public class UpdateReservationRequest
    {
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
