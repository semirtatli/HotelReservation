using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.RepositoryInterfaces
{
    public interface IReservationRepository
    {
        public void AddReservation(Reservation reservation);
        public bool IsRoomAvailable(Guid roomId, DateTime checkInDate, DateTime checkOutDate);
    }
}
