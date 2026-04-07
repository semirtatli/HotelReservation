using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.Mappers;
using HotelReservation.Domain.RepositoryInterfaces;
using HotelReservation.Domain.Exceptions;

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
            var hotel = HotelMapper.ToEntity(request);
            await _hotelRepository.AddHotelAsync(hotel);
            return HotelMapper.ToResponse(hotel);
        }

        public async Task<HotelResponse> UpdateHotelAsync(Guid id, UpdateHotelRequest request)
        {
            _updateHotelRequestValidator.ValidateAndThrow(request);
            var hotel = await _hotelRepository.GetHotelByIdAsync(id);
            if (hotel is null) throw new NotFoundException("Hotel not found");
            hotel.UpdateName(request.Name);
            await _hotelRepository.UpdateHotelAsync(hotel);
            return HotelMapper.ToResponse(hotel);
        }

        public async Task<List<HotelResponse>> GetAllHotelsAsync()
        {
            var hotels = await _hotelRepository.GetAllHotelsAsync();
            return hotels.Select(HotelMapper.ToResponse).ToList();
        }

        public async Task<HotelResponse> GetHotelByIdAsync(Guid id)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(id);
            if (hotel is null) throw new NotFoundException("Hotel not found");
            return HotelMapper.ToResponse(hotel);
        }

        public async Task<HotelResponse> DeleteHotelAsync(Guid id)
        {
            var deletedHotel = await _hotelRepository.DeleteHotelAsync(id);
            if (deletedHotel is null) throw new NotFoundException("Hotel not found");
            return HotelMapper.ToResponse(deletedHotel);
        }
    }
}
