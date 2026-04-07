using HotelReservation.Domain.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
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

        public async Task<Room?> GetRoomByIdAsync(Guid id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task UpdateRoomAsync(Room room)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Room?> DeleteRoomAsync(Guid id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room is null) return null;
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return room;
        }
    }
}
