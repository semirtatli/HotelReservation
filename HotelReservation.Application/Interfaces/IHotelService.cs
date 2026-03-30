using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Interfaces
{
    public interface IHotelService
    {
        public void AddHotel(Hotel hotel);
    }
}
