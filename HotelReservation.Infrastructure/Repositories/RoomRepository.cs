using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Exceptions;
using HotelReservation.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;
        public RoomRepository(AppDbContext _context)
        {
            this._context = _context;
        }

        public void AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
        }

        public List<Room> GetAllRooms()
        {
            return _context.Rooms.ToList();
        }

        public Room GetRoomById(Guid id)
        {
            var room = _context.Rooms.Find(id);
            if (room is null) throw new NotFoundException("Room not found");
            return room;
        }

        public Room UpdateRoom(Guid id, Room room)
        {
            var existingRoom = _context.Rooms.Find(id);
            if (existingRoom is null) throw new NotFoundException("Room not found");
            existingRoom.UpdateCapacity(room.Capacity);
            existingRoom.UpdatePrice(room.Price);
            _context.SaveChanges();
            return existingRoom;
        }

        public Room DeleteRoom(Guid id)
        {
            var room = _context.Rooms.Find(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
                return room;
            }
            throw new NotFoundException("Room not found");
        }
    }
}
