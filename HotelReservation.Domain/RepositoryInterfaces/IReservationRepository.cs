using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.RepositoryInterfaces
{
    public interface IReservationRepository
    {
        Task AddReservationAsync(Reservation reservation);
        Task<bool> IsRoomAvailableAsync(Guid roomId, DateOnly checkInDate, DateOnly checkOutDate, Guid? excludeReservationId = null);
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<Reservation?> GetReservationByIdAsync(Guid id);
        Task UpdateReservationAsync(Reservation reservation);
        Task<Reservation?> DeleteReservationAsync(Guid id);
    }
}
