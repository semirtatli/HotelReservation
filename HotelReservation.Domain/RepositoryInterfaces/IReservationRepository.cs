using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.RepositoryInterfaces
{
    public interface IReservationRepository
    {
        Task AddReservationAsync(Reservation reservation);
        Task<bool> IsRoomAvailableAsync(Guid roomId, DateOnly checkInDate, DateOnly checkOutDate, Guid? excludeReservationId = null);
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<Reservation?> GetReservationByIdAsync(Guid id);
        Task<Reservation?> UpdateReservationAsync(Guid id, Reservation reservation);
        Task<Reservation?> DeleteReservationAsync(Guid id);
    }
}
