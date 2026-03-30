using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
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
    }
}
