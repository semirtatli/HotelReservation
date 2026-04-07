using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.RepositoryInterfaces
{
    public interface IHotelRepository
    {
        Task AddHotelAsync(Hotel hotel);
        Task UpdateHotelAsync(Hotel hotel);
        Task<List<Hotel>> GetAllHotelsAsync();
        Task<Hotel?> GetHotelByIdAsync(Guid id);
        Task<Hotel?> DeleteHotelAsync(Guid id);
    }
}
