using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelReservation.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IValidator<CreateReservationRequest> _createReservationRequestValidator;
        private readonly IValidator<UpdateReservationRequest> _updateReservationRequestValidator;

        public ReservationService(IReservationRepository _reservationRepository, IRoomRepository roomRepository, IValidator<CreateReservationRequest> createReservationRequestValidator, IValidator<UpdateReservationRequest> updateReservationRequestValidator)
        {
            this._reservationRepository = _reservationRepository;
            _roomRepository = roomRepository;
            _createReservationRequestValidator = createReservationRequestValidator;
            _updateReservationRequestValidator = updateReservationRequestValidator;
        }

        public ReservationResponse AddReservation(CreateReservationRequest createReservationRequest)
        {
            _createReservationRequestValidator.ValidateAndThrow(createReservationRequest);

            var checkIn = DateTime.SpecifyKind(createReservationRequest.CheckInDate, DateTimeKind.Utc);
            var checkOut = DateTime.SpecifyKind(createReservationRequest.CheckOutDate, DateTimeKind.Utc);

            if (!_reservationRepository.IsRoomAvailable(createReservationRequest.RoomId, checkIn, checkOut))
                throw new Exception("Room is not available for the selected dates.");

            var room = _roomRepository.GetRoomById(createReservationRequest.RoomId);
            var totalPrice = room.Price * (checkOut - checkIn).Days;

            var reservationEntity = new Reservation(checkIn, checkOut, createReservationRequest.CustomerId, createReservationRequest.RoomId, createReservationRequest.NumberOfGuests, totalPrice);
            _reservationRepository.AddReservation(reservationEntity);
            return new ReservationResponse
            {
                Id = reservationEntity.Id,
                CheckInDate = reservationEntity.CheckInDate,
                CheckOutDate = reservationEntity.CheckOutDate,
                CustomerId = reservationEntity.CustomerId,
                RoomId = reservationEntity.RoomId,
                NumberOfGuests = reservationEntity.NumberOfGuests,
                TotalPrice = reservationEntity.TotalPrice
            };
        }

        public List<ReservationResponse> GetAllReservations()
        {
            var reservations = _reservationRepository.GetAllReservations();
            return reservations.Select(r => new ReservationResponse
            {
                Id = r.Id,
                CheckInDate = r.CheckInDate,
                CheckOutDate = r.CheckOutDate,
                CustomerId = r.CustomerId,
                RoomId = r.RoomId,
                NumberOfGuests = r.NumberOfGuests,
                TotalPrice = r.TotalPrice
            }).ToList();
        }

        public ReservationResponse GetReservationById(Guid id)
        {
            var reservation = _reservationRepository.GetReservationById(id);
            return new ReservationResponse
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

        public ReservationResponse UpdateReservation(Guid id, UpdateReservationRequest updateReservationRequest)
        {
            _updateReservationRequestValidator.ValidateAndThrow(updateReservationRequest);

            var checkIn = DateTime.SpecifyKind(updateReservationRequest.CheckInDate, DateTimeKind.Utc);
            var checkOut = DateTime.SpecifyKind(updateReservationRequest.CheckOutDate, DateTimeKind.Utc);

            var reservation = _reservationRepository.UpdateReservation(id, checkIn, checkOut, updateReservationRequest.NumberOfGuests);
            return new ReservationResponse
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

        public ReservationResponse DeleteReservation(Guid id)
        {
            var deletedReservation = _reservationRepository.DeleteReservation(id);
            return new ReservationResponse
            {
                Id = deletedReservation.Id,
                CheckInDate = deletedReservation.CheckInDate,
                CheckOutDate = deletedReservation.CheckOutDate,
                CustomerId = deletedReservation.CustomerId,
                RoomId = deletedReservation.RoomId,
                NumberOfGuests = deletedReservation.NumberOfGuests,
                TotalPrice = deletedReservation.TotalPrice
            };
        }
    }
}
