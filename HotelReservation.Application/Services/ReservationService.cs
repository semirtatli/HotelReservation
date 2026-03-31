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

        public ReservationService(IReservationRepository _reservationRepository)
        {
            this._reservationRepository = _reservationRepository;
        }

        public ReservationResponse AddReservation(CreateReservationRequest createReservationRequest)
        {
            var checkIn = DateTime.SpecifyKind(createReservationRequest.CheckInDate, DateTimeKind.Utc);
            var checkOut = DateTime.SpecifyKind(createReservationRequest.CheckOutDate, DateTimeKind.Utc);

            if (!_reservationRepository.IsRoomAvailable(createReservationRequest.RoomId, checkIn, checkOut))
                throw new Exception("Room is not available for the selected dates.");

            var reservationEntity = new Reservation(checkIn, checkOut, createReservationRequest.CustomerId, createReservationRequest.RoomId, createReservationRequest.NumberOfGuests, createReservationRequest.TotalPrice);
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
