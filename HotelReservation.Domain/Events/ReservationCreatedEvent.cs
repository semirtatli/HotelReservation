

namespace HotelReservation.Domain.Events
{
    public record ReservationCreatedEvent
    (
        Guid ReservationId,
        Guid CustomerId,
        string CustomerEmail,
        string CustomerPhone,
        Guid RoomId,
        DateOnly CheckInDate,
        DateOnly CheckOutDate
    );
}
