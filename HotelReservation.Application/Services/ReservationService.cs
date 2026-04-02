using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Application.Strategies;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Exceptions;

namespace HotelReservation.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IValidator<CreateReservationRequest> _createReservationRequestValidator;
        private readonly IValidator<UpdateReservationRequest> _updateReservationRequestValidator;
        private readonly IPricingStrategy _pricingStrategy;

        public ReservationService(IReservationRepository reservationRepository, IRoomRepository roomRepository, IValidator<CreateReservationRequest> createReservationRequestValidator, IValidator<UpdateReservationRequest> updateReservationRequestValidator, IPricingStrategy pricingStrategy)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _createReservationRequestValidator = createReservationRequestValidator;
            _updateReservationRequestValidator = updateReservationRequestValidator;
            _pricingStrategy = pricingStrategy;
        }

        public async Task<ReservationResponse> AddReservationAsync(CreateReservationRequest request)
        {
            _createReservationRequestValidator.ValidateAndThrow(request);

            if (!await _reservationRepository.IsRoomAvailableAsync(request.RoomId, request.CheckInDate, request.CheckOutDate))
                throw new RoomNotAvailableException("Room is not available for the selected dates.");

            var room = await _roomRepository.GetRoomByIdAsync(request.RoomId);

            if (request.NumberOfGuests > room.Capacity)
                throw new ArgumentException($"Number of guests ({request.NumberOfGuests}) exceeds room capacity ({room.Capacity}).");

            var totalPrice = _pricingStrategy.Calculate(room, request.CheckInDate, request.CheckOutDate);
            var reservation = new Reservation(request.CheckInDate, request.CheckOutDate, request.CustomerId, request.RoomId, request.NumberOfGuests, totalPrice);
            await _reservationRepository.AddReservationAsync(reservation);
            return MapToResponse(reservation);
        }

        public async Task<List<ReservationResponse>> GetAllReservationsAsync()
        {
            var reservations = await _reservationRepository.GetAllReservationsAsync();
            return reservations.Select(MapToResponse).ToList();
        }

        public async Task<ReservationResponse> GetReservationByIdAsync(Guid id)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            return MapToResponse(reservation);
        }

        public async Task<ReservationResponse> UpdateReservationAsync(Guid id, UpdateReservationRequest request)
        {
            _updateReservationRequestValidator.ValidateAndThrow(request);

            var existing = await _reservationRepository.GetReservationByIdAsync(id);

            if (!await _reservationRepository.IsRoomAvailableAsync(existing.RoomId, request.CheckInDate, request.CheckOutDate))
                throw new RoomNotAvailableException("Room is not available for the selected dates.");

            var room = await _roomRepository.GetRoomByIdAsync(existing.RoomId);

            if (request.NumberOfGuests > room.Capacity)
                throw new ArgumentException($"Number of guests ({request.NumberOfGuests}) exceeds room capacity ({room.Capacity}).");

            var totalPrice = _pricingStrategy.Calculate(room, request.CheckInDate, request.CheckOutDate);
            var reservation = await _reservationRepository.UpdateReservationAsync(id, request.CheckInDate, request.CheckOutDate, request.NumberOfGuests, totalPrice);
            return MapToResponse(reservation);
        }

        public async Task<ReservationResponse> DeleteReservationAsync(Guid id)
        {
            var deletedReservation = await _reservationRepository.DeleteReservationAsync(id);
            return MapToResponse(deletedReservation);
        }

        private static ReservationResponse MapToResponse(Reservation reservation) => new()
        {
            Id = reservation.Id,
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            CustomerId = reservation.CustomerId,
            RoomId = reservation.RoomId,
            NumberOfGuests = reservation.NumberOfGuests,
            TotalPrice = reservation.TotalPrice
        };
    }
}
