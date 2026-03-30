using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository _reservationRepository) {
            this._reservationRepository = _reservationRepository;
        }

        public void AddReservation(Reservation reservation)
        {
            if(IsRoomAvailable(reservation.RoomId, reservation.CheckInDate, reservation.CheckOutDate))
            _reservationRepository.AddReservation(reservation);
            else
            {
                throw new Exception("Room is not available for the selected dates.");
            }
        }

        private bool IsRoomAvailable(Guid roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            return _reservationRepository.IsRoomAvailable(roomId, checkInDate, checkOutDate);
        }
    }
}
