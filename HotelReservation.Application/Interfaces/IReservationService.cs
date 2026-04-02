using HotelReservation.Application.DTO;

namespace HotelReservation.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationResponse> AddReservationAsync(CreateReservationRequest request);
        Task<List<ReservationResponse>> GetAllReservationsAsync();
        Task<ReservationResponse> GetReservationByIdAsync(Guid id);
        Task<ReservationResponse> UpdateReservationAsync(Guid id, UpdateReservationRequest request);
        Task<ReservationResponse> DeleteReservationAsync(Guid id);
    }
}
