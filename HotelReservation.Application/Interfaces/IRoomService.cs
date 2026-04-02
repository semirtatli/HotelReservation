using HotelReservation.Application.DTO;

namespace HotelReservation.Application.Interfaces
{
    public interface IRoomService
    {
        Task<RoomResponse> AddRoomAsync(CreateRoomRequest request);
        Task<List<RoomResponse>> GetAllRoomsAsync();
        Task<RoomResponse> GetRoomByIdAsync(Guid id);
        Task<RoomResponse> UpdateRoomAsync(Guid id, UpdateRoomRequest request);
        Task<RoomResponse> DeleteRoomAsync(Guid id);
    }
}
