using HotelReservation.Application.DTO;

namespace HotelReservation.Application.Interfaces
{
    public interface IHotelService
    {
        Task<HotelResponse> AddHotelAsync(CreateHotelRequest request);
        Task<HotelResponse> UpdateHotelAsync(Guid id, UpdateHotelRequest request);
        Task<List<HotelResponse>> GetAllHotelsAsync();
        Task<HotelResponse> GetHotelByIdAsync(Guid id);
        Task<HotelResponse> DeleteHotelAsync(Guid id);
    }
}
