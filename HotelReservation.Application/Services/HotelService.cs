using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelReservation.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository _hotelRepository)
        {
            this._hotelRepository = _hotelRepository;
        }

        public HotelResponse AddHotel(CreateHotelRequest CreateHotelRequest)
        {
            var HotelEntity = new Hotel
            (
                CreateHotelRequest.Name
            );
            _hotelRepository.AddHotel(HotelEntity);
            return new HotelResponse { Name = HotelEntity.Name };
        }

        public HotelResponse UpdateHotel(Guid id, UpdateHotelRequest UpdateHotelRequest)
        {
            var HotelEntity = new Hotel
            (
                UpdateHotelRequest.Name
            );
            var result = _hotelRepository.UpdateHotel(id, HotelEntity);
            if(!result)
            {
                throw new Exception("Hotel not found");
            }
            return new HotelResponse { Name = HotelEntity.Name };
        }

        public List<HotelResponse> GetAllHotels()
        {
            var hotels = _hotelRepository.GetAllHotels();
            return hotels.Select(h => new HotelResponse
            {
                Id = h.Id,
                Name = h.Name
            }).ToList();
        }

        public HotelResponse GetHotelById(Guid id)
        {
            var hotel = _hotelRepository.GetHotelById(id);
            return new HotelResponse
            {
                Id = hotel.Id,
                Name = hotel.Name
            };
        }

        public HotelResponse DeleteHotel(Guid id)
        {
            var deletedHotel = _hotelRepository.DeleteHotel(id);
            var deletedHotelResponse = new HotelResponse
            {
                Id = deletedHotel.Id,
                Name = deletedHotel.Name
            };
            return deletedHotelResponse;

        }
    }
}
