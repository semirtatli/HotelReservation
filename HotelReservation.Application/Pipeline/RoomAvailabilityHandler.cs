using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Pipeline
{
    public class RoomAvailabilityHandler : BaseReservationHandler
    {
        private readonly IReservationRepository _reservationRepository;

        public RoomAvailabilityHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public override async Task HandleAsync(ReservationContext context)
        {
            var isAvailable = await _reservationRepository.IsRoomAvailableAsync(
                context.Request.RoomId, context.Request.CheckInDate, context.Request.CheckOutDate);
            if (!isAvailable)
            {
                throw new RoomNotAvailableException("The selected room is not available for the specified dates.");
            }
            await CallNextAsycn(context);
        }
}
