using HotelReservation.Application.DTO;
using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Interfaces
{
    public interface IHotelService
    {
        public HotelResponse AddHotel(CreateHotelRequest CreateHotelRequest);

        public HotelResponse UpdateHotel(Guid Id, UpdateHotelRequest UpdateHotelRequest);

        public List<HotelResponse> GetAllHotels();

        public HotelResponse GetHotelById(Guid id);

        public HotelResponse DeleteHotel(Guid id);
    }
}
