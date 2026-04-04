using HotelReservation.Application.DTO;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Mappers
{
    public static class ReservationMapper
    {
        public static Reservation ToEntity(CreateReservationRequest request, decimal totalPrice) =>
            new(request.CheckInDate, request.CheckOutDate, request.CustomerId, request.RoomId, request.NumberOfGuests, totalPrice);

        public static ReservationResponse ToResponse(Reservation reservation) => new()
        {
            Id = reservation.Id,
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            CustomerId = reservation.CustomerId,
            RoomId = reservation.RoomId,
            NumberOfGuests = reservation.NumberOfGuests,
            TotalPrice = reservation.TotalPrice
        };
    }
}
