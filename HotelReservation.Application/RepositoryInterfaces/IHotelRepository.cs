using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.RepositoryInterfaces
{
    public interface IHotelRepository
    {
        public void AddHotel(Hotel hotel);
    }
}
