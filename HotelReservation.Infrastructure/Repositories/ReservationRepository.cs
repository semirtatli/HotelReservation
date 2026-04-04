using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;
        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsRoomAvailableAsync(Guid roomId, DateOnly checkInDate, DateOnly checkOutDate, Guid? excludeReservationId = null)
        {
            var hasConflict = await _context.Reservations.AnyAsync(r =>
                r.RoomId == roomId &&
                r.CheckInDate < checkOutDate &&
                r.CheckOutDate > checkInDate &&
                (excludeReservationId == null || r.Id != excludeReservationId)
            );
            return !hasConflict;
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task<Reservation?> GetReservationByIdAsync(Guid id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task<Reservation?> UpdateReservationAsync(Guid id, Reservation updatedReservation)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation is null) return null;

            reservation.Update(updatedReservation.CheckInDate, updatedReservation.CheckOutDate, updatedReservation.NumberOfGuests, updatedReservation.TotalPrice);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<Reservation?> DeleteReservationAsync(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation is null) return null;
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }
    }
}
