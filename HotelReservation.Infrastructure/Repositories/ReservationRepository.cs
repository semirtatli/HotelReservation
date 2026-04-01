using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Exceptions;
using HotelReservation.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;
        public ReservationRepository(AppDbContext _context)
        {
            this._context = _context;
        }

        public void AddReservation(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
        }

        public bool IsRoomAvailable(Guid roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var hasConflict = _context.Reservations
                .Any(r =>
                    r.RoomId == roomId &&
                    r.CheckInDate < checkOutDate &&
                    r.CheckOutDate > checkInDate
                );
            return !hasConflict;
        }

        public List<Reservation> GetAllReservations()
        {
            return _context.Reservations.ToList();
        }

        public Reservation GetReservationById(Guid id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation is null) throw new NotFoundException("Reservation not found");
            return reservation;
        }

        public Reservation DeleteReservation(Guid id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
                return reservation;
            }
            throw new NotFoundException("Reservation not found");
        }
    }
}
