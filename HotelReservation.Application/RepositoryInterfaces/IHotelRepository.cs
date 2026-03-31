using HotelReservation.Application.DTO;
using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.RepositoryInterfaces
{
    public interface IHotelRepository
    {
        public void AddHotel(Hotel hotel);

        public bool UpdateHotel(Guid id, Hotel hotel);

        public List<Hotel> GetAllHotels();

        public Hotel GetHotelById(Guid id);

        public Hotel DeleteHotel(Guid id);
    }
}
