using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Interfaces
{
    public interface IReservationService
    {
        public void AddReservation(Reservation reservation);
    }
}
