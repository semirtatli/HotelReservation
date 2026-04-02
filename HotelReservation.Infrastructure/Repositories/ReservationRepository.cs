using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Exceptions;
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

        public async Task<bool> IsRoomAvailableAsync(Guid roomId, DateOnly checkInDate, DateOnly checkOutDate)
        {
            var hasConflict = await _context.Reservations.AnyAsync(r =>
                r.RoomId == roomId &&
                r.CheckInDate < checkOutDate &&
                r.CheckOutDate > checkInDate
            );
            return !hasConflict;
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation is null) throw new NotFoundException("Reservation not found");
            return reservation;
        }

        public async Task<Reservation> UpdateReservationAsync(Guid id, DateOnly checkInDate, DateOnly checkOutDate, int numberOfGuests, decimal totalPrice)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation is null) throw new NotFoundException("Reservation not found");

            var hasConflict = await _context.Reservations.AnyAsync(r =>
                r.Id != id &&
                r.RoomId == reservation.RoomId &&
                r.CheckInDate < checkOutDate &&
                r.CheckOutDate > checkInDate
            );
            if (hasConflict) throw new RoomNotAvailableException("Room is not available for the selected dates.");

            reservation.Update(checkInDate, checkOutDate, numberOfGuests, totalPrice);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<Reservation> DeleteReservationAsync(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation is null) throw new NotFoundException("Reservation not found");
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }
    }
}
