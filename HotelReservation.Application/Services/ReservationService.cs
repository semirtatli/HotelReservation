using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.Mappers;
using HotelReservation.Application.Notifications;
using HotelReservation.Application.Pipeline;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Application.Strategies;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Events;
using HotelReservation.Domain.Exceptions;

namespace HotelReservation.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<CreateReservationRequest> _createReservationRequestValidator;
        private readonly IValidator<UpdateReservationRequest> _updateReservationRequestValidator;
        private readonly IPricingStrategy _pricingStrategy;
        private readonly IEnumerable<IReservationObserver> _observers;
        private readonly RoomAvailabilityHandler _availabilityHandler;
        private readonly CapacityCheckHandler _capacityHandler;
        private readonly PricingCalculationHandler _pricingHandler;

        public ReservationService(
            IReservationRepository reservationRepository,
            IRoomRepository roomRepository,
            ICustomerRepository customerRepository,
            IValidator<CreateReservationRequest> createReservationRequestValidator,
            IValidator<UpdateReservationRequest> updateReservationRequestValidator,
            IPricingStrategy pricingStrategy,
            IEnumerable<IReservationObserver> observers,
            RoomAvailabilityHandler availabilityHandler,
            CapacityCheckHandler capacityHandler,
            PricingCalculationHandler pricingHandler)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _customerRepository = customerRepository;
            _createReservationRequestValidator = createReservationRequestValidator;
            _updateReservationRequestValidator = updateReservationRequestValidator;
            _pricingStrategy = pricingStrategy;
            _observers = observers;
            _availabilityHandler = availabilityHandler;
            _capacityHandler = capacityHandler;
            _pricingHandler = pricingHandler;
        }

        public async Task<ReservationResponse> AddReservationAsync(CreateReservationRequest request)
        {
            _createReservationRequestValidator.ValidateAndThrow(request);

            var room = await _roomRepository.GetRoomByIdAsync(request.RoomId);
            if (room is null) throw new NotFoundException("Room not found");

            var context = new ReservationContext { Request = request, Room = room };
            _availabilityHandler.SetNext(_capacityHandler).SetNext(_pricingHandler);
            await _availabilityHandler.HandleAsync(context);

            var reservation = ReservationMapper.ToEntity(request, context.TotalPrice);
            await _reservationRepository.AddReservationAsync(reservation);

            var customer = await _customerRepository.GetCustomerByIdAsync(reservation.CustomerId);
            if (customer is null) throw new NotFoundException("Customer not found");

            var @event = new ReservationCreatedEvent(
                reservation.Id, reservation.CustomerId,
                customer.Email, customer.PhoneNumber,
                reservation.RoomId, reservation.CheckInDate, reservation.CheckOutDate);

            foreach (var observer in _observers)
                await observer.OnReservationCreatedAsync(@event);

            return ReservationMapper.ToResponse(reservation);
        }

        public async Task<List<ReservationResponse>> GetAllReservationsAsync()
        {
            var reservations = await _reservationRepository.GetAllReservationsAsync();
            return reservations.Select(ReservationMapper.ToResponse).ToList();
        }

        public async Task<ReservationResponse> GetReservationByIdAsync(Guid id)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation is null) throw new NotFoundException("Reservation not found");
            return ReservationMapper.ToResponse(reservation);
        }

        public async Task<ReservationResponse> UpdateReservationAsync(Guid id, UpdateReservationRequest request)
        {
            _updateReservationRequestValidator.ValidateAndThrow(request);

            var existing = await _reservationRepository.GetReservationByIdAsync(id);
            if (existing is null) throw new NotFoundException("Reservation not found");

            if (!await _reservationRepository.IsRoomAvailableAsync(existing.RoomId, request.CheckInDate, request.CheckOutDate, id))
                throw new RoomNotAvailableException("Room is not available for the selected dates.");

            var room = await _roomRepository.GetRoomByIdAsync(existing.RoomId);
            if (room is null) throw new NotFoundException("Room not found");

            if (request.NumberOfGuests > room.Capacity)
                throw new ArgumentException($"Number of guests ({request.NumberOfGuests}) exceeds room capacity ({room.Capacity}).");

            var totalPrice = _pricingStrategy.Calculate(room, request.CheckInDate, request.CheckOutDate);
            var updatedReservation = new Reservation(request.CheckInDate, request.CheckOutDate, existing.CustomerId, existing.RoomId, request.NumberOfGuests, totalPrice);
            var reservation = await _reservationRepository.UpdateReservationAsync(id, updatedReservation);
            if (reservation is null) throw new NotFoundException("Reservation not found");
            return ReservationMapper.ToResponse(reservation);
        }

        public async Task<ReservationResponse> DeleteReservationAsync(Guid id)
        {
            var deletedReservation = await _reservationRepository.DeleteReservationAsync(id);
            if (deletedReservation is null) throw new NotFoundException("Reservation not found");
            return ReservationMapper.ToResponse(deletedReservation);
        }
    }
}
