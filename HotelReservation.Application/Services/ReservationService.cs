using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.Mappers;
using HotelReservation.Application.Notifications.Observers;
using HotelReservation.Application.Pipeline;
using HotelReservation.Domain.RepositoryInterfaces;
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
        private readonly IEnumerable<IReservationObserver> _observers;
        private readonly IReservationHandler _reservationPipeline;

        public ReservationService(
            IReservationRepository reservationRepository,
            IRoomRepository roomRepository,
            ICustomerRepository customerRepository,
            IValidator<CreateReservationRequest> createReservationRequestValidator,
            IValidator<UpdateReservationRequest> updateReservationRequestValidator,
            IEnumerable<IReservationObserver> observers,
            IReservationHandler reservationPipeline)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _customerRepository = customerRepository;
            _createReservationRequestValidator = createReservationRequestValidator;
            _updateReservationRequestValidator = updateReservationRequestValidator;
            _observers = observers;
            _reservationPipeline = reservationPipeline;
        }

        public async Task<ReservationResponse> AddReservationAsync(CreateReservationRequest request)
        {
            _createReservationRequestValidator.ValidateAndThrow(request);

            var room = await _roomRepository.GetRoomByIdAsync(request.RoomId);
            if (room is null) throw new NotFoundException("Room not found");

            var context = new ReservationContext
            {
                RoomId = request.RoomId,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                NumberOfGuests = request.NumberOfGuests,
                Room = room
            };
            await _reservationPipeline.HandleAsync(context);

            var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId);
            if (customer is null) throw new NotFoundException("Customer not found");

            var reservation = ReservationMapper.ToEntity(request, context.TotalPrice);
            await _reservationRepository.AddReservationAsync(reservation);

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

            var room = await _roomRepository.GetRoomByIdAsync(existing.RoomId);
            if (room is null) throw new NotFoundException("Room not found");

            var context = new ReservationContext
            {
                RoomId = existing.RoomId,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                NumberOfGuests = request.NumberOfGuests,
                ExcludeReservationId = id,
                Room = room
            };
            await _reservationPipeline.HandleAsync(context);

            existing.Update(request.CheckInDate, request.CheckOutDate, request.NumberOfGuests, context.TotalPrice);
            await _reservationRepository.UpdateReservationAsync(existing);
            return ReservationMapper.ToResponse(existing);
        }

        public async Task<ReservationResponse> DeleteReservationAsync(Guid id)
        {
            var deletedReservation = await _reservationRepository.DeleteReservationAsync(id);
            if (deletedReservation is null) throw new NotFoundException("Reservation not found");
            return ReservationMapper.ToResponse(deletedReservation);
        }
    }
}
