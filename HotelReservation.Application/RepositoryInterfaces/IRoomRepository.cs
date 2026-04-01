using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.RepositoryInterfaces
{
    public interface IRoomRepository
    {
        public void AddRoom(Room room);
        public List<Room> GetAllRooms();
        public Room GetRoomById(Guid id);
        public Room UpdateRoom(Guid id, Room room);
        public Room DeleteRoom(Guid id);
    }
}
