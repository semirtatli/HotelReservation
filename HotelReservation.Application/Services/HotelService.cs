using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IValidator<CreateHotelRequest> _createHotelRequestValidator;
        private readonly IValidator<UpdateHotelRequest> _updateHotelRequestValidator;

        public HotelService(IHotelRepository hotelRepository, IValidator<CreateHotelRequest> createHotelRequestValidator, IValidator<UpdateHotelRequest> updateHotelRequestValidator)
        {
            _hotelRepository = hotelRepository;
            _createHotelRequestValidator = createHotelRequestValidator;
            _updateHotelRequestValidator = updateHotelRequestValidator;
        }

        public async Task<HotelResponse> AddHotelAsync(CreateHotelRequest request)
        {
            _createHotelRequestValidator.ValidateAndThrow(request);
            var hotel = new Hotel(request.Name);
            await _hotelRepository.AddHotelAsync(hotel);
            return MapToResponse(hotel);
        }

        public async Task<HotelResponse> UpdateHotelAsync(Guid id, UpdateHotelRequest request)
        {
            _updateHotelRequestValidator.ValidateAndThrow(request);
            var hotelEntity = new Hotel(request.Name);
            var updatedHotel = await _hotelRepository.UpdateHotelAsync(id, hotelEntity);
            return MapToResponse(updatedHotel);
        }

        public async Task<List<HotelResponse>> GetAllHotelsAsync()
        {
            var hotels = await _hotelRepository.GetAllHotelsAsync();
            return hotels.Select(MapToResponse).ToList();
        }

        public async Task<HotelResponse> GetHotelByIdAsync(Guid id)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(id);
            return MapToResponse(hotel);
        }

        public async Task<HotelResponse> DeleteHotelAsync(Guid id)
        {
            var deletedHotel = await _hotelRepository.DeleteHotelAsync(id);
            return MapToResponse(deletedHotel);
        }

        private static HotelResponse MapToResponse(Hotel hotel) => new()
        {
            Id = hotel.Id,
            Name = hotel.Name
        };
    }
}
