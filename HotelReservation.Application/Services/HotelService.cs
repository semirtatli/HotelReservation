using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelReservation.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IValidator<CreateHotelRequest> _createHotelRequestValidator;
        private readonly IValidator<UpdateHotelRequest> _updateHotelRequestValidator;

        public HotelService(IHotelRepository _hotelRepository, IValidator<CreateHotelRequest> createHotelRequestValidator, IValidator<UpdateHotelRequest> updateHotelRequestValidator)
        {
            this._hotelRepository = _hotelRepository;
            _createHotelRequestValidator = createHotelRequestValidator;
            _updateHotelRequestValidator = updateHotelRequestValidator;
        }

        public HotelResponse AddHotel(CreateHotelRequest CreateHotelRequest)
        {
            _createHotelRequestValidator.ValidateAndThrow(CreateHotelRequest);

            var HotelEntity = new Hotel
            (
                CreateHotelRequest.Name
            );
            _hotelRepository.AddHotel(HotelEntity);
            return new HotelResponse { Name = HotelEntity.Name };
        }

        public HotelResponse UpdateHotel(Guid id, UpdateHotelRequest UpdateHotelRequest)
        {
            _updateHotelRequestValidator.ValidateAndThrow(UpdateHotelRequest);

            var HotelEntity = new Hotel(UpdateHotelRequest.Name);
            var updatedHotel = _hotelRepository.UpdateHotel(id, HotelEntity);
            return new HotelResponse { Id = updatedHotel.Id, Name = updatedHotel.Name };
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
