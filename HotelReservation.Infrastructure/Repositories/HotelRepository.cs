using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AppDbContext _context;
        public HotelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddHotelAsync(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Hotel>> GetAllHotelsAsync()
        {
            return await _context.Hotels.ToListAsync();
        }

        public async Task<Hotel?> GetHotelByIdAsync(Guid id)
        {
            return await _context.Hotels.FindAsync(id);
        }

        public async Task<Hotel?> UpdateHotelAsync(Guid id, Hotel hotel)
        {
            var existingHotel = await _context.Hotels.FindAsync(id);
            if (existingHotel is null) return null;
            existingHotel.UpdateName(hotel.Name);
            await _context.SaveChangesAsync();
            return existingHotel;
        }

        public async Task<Hotel?> DeleteHotelAsync(Guid id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel is null) return null;
            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return hotel;
        }
    }
}
