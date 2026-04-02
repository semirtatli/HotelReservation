using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Enums;
using HotelReservation.Domain.Exceptions;
using HotelReservation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;
        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRoomAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(Guid id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room is null) throw new NotFoundException("Room not found");
            return room;
        }

        public async Task<Room> UpdateRoomAsync(Guid id, int capacity, decimal price, RoomType roomType)
        {
            var existingRoom = await _context.Rooms.FindAsync(id);
            if (existingRoom is null) throw new NotFoundException("Room not found");
            existingRoom.UpdateCapacity(capacity);
            existingRoom.UpdatePrice(price);
            existingRoom.UpdateRoomType(roomType);
            await _context.SaveChangesAsync();
            return existingRoom;
        }

        public async Task<Room> DeleteRoomAsync(Guid id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room is null) throw new NotFoundException("Room not found");
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return room;
        }
    }
}
