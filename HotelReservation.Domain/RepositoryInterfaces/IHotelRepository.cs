using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.RepositoryInterfaces
{
    public interface IHotelRepository
    {
        Task AddHotelAsync(Hotel hotel);
        Task<Hotel?> UpdateHotelAsync(Guid id, Hotel hotel);
        Task<List<Hotel>> GetAllHotelsAsync();
        Task<Hotel?> GetHotelByIdAsync(Guid id);
        Task<Hotel?> DeleteHotelAsync(Guid id);
    }
}
