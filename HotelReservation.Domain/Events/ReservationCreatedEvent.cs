

namespace HotelReservation.Domain.Events
{
    public record ReservationCreatedEvent
    (
        Guid ReservationId,
        Guid CustomerId,
        Guid RoomId,
        DateOnly CheckInDate,
        DateOnly CheckOutDate
    );
}
