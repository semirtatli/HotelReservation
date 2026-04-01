using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.RepositoryInterfaces
{
    public interface IReservationRepository
    {
        public void AddReservation(Reservation reservation);
        public bool IsRoomAvailable(Guid roomId, DateTime checkInDate, DateTime checkOutDate);
        public List<Reservation> GetAllReservations();
        public Reservation GetReservationById(Guid id);
        public Reservation UpdateReservation(Guid id, DateTime checkInDate, DateTime checkOutDate, int numberOfGuests);
        public Reservation DeleteReservation(Guid id);
    }
}
