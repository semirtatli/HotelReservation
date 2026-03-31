using HotelReservation.Application.DTO;

namespace HotelReservation.Application.Interfaces
{
    public interface IReservationService
    {
        public ReservationResponse AddReservation(CreateReservationRequest request);
        public List<ReservationResponse> GetAllReservations();
        public ReservationResponse GetReservationById(Guid id);
        public ReservationResponse DeleteReservation(Guid id);
    }
}
