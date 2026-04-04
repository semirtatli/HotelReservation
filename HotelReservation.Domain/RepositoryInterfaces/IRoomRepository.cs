using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Enums;

namespace HotelReservation.Application.RepositoryInterfaces
{
    public interface IRoomRepository
    {
        Task AddRoomAsync(Room room);
        Task<List<Room>> GetAllRoomsAsync();
        Task<Room?> GetRoomByIdAsync(Guid id);
        Task<Room?> UpdateRoomAsync(Guid id, int capacity, decimal price, RoomType roomType);
        Task<Room?> DeleteRoomAsync(Guid id);
    }
}
