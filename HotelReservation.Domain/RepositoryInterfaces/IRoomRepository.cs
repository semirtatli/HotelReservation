using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.RepositoryInterfaces
{
    public interface IRoomRepository
    {
        Task AddRoomAsync(Room room);
        Task<List<Room>> GetAllRoomsAsync();
        Task<Room?> GetRoomByIdAsync(Guid id);
        Task UpdateRoomAsync(Room room);
        Task<Room?> DeleteRoomAsync(Guid id);
    }
}
