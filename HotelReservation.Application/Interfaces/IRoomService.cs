using HotelReservation.Application.DTO;

namespace HotelReservation.Application.Interfaces
{
    public interface IRoomService
    {
        public RoomResponse AddRoom(CreateRoomRequest request);
        public List<RoomResponse> GetAllRooms();
        public RoomResponse GetRoomById(Guid id);
        public RoomResponse UpdateRoom(Guid id, UpdateRoomRequest request);
        public RoomResponse DeleteRoom(Guid id);
    }
}
