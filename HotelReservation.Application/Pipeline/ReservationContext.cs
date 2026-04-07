using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Pipeline
{
    public class ReservationContext
    {
        public Guid RoomId { get; init; }
        public DateOnly CheckInDate { get; init; }
        public DateOnly CheckOutDate { get; init; }
        public int NumberOfGuests { get; init; }
        public Guid? ExcludeReservationId { get; init; }
        public Room Room { get; set; } = null!;
        public decimal TotalPrice { get; set; }
    }
}
