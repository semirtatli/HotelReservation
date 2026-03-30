using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository _hotelRepository) {
            this._hotelRepository = _hotelRepository;
        }

        public void AddHotel(Hotel hotel)
        {
            _hotelRepository.AddHotel(hotel);
        }
    }
}
