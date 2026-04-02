namespace HotelReservation.Application.DTO
{
    public class ReservationResponse
    {
        public Guid Id { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid RoomId { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
