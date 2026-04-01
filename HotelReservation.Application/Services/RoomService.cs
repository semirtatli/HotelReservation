using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelReservation.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IValidator<CreateRoomRequest> _createRoomRequestValidator;
        private readonly IValidator<UpdateRoomRequest> _updateRoomRequestValidator;

        public RoomService(IRoomRepository _roomRepository, IValidator<CreateRoomRequest> createRoomRequestValidator, IValidator<UpdateRoomRequest> updateRoomRequestValidator)
        {
            this._roomRepository = _roomRepository;
            _createRoomRequestValidator = createRoomRequestValidator;
            _updateRoomRequestValidator = updateRoomRequestValidator;
        }

        public RoomResponse AddRoom(CreateRoomRequest createRoomRequest)
        {
            _createRoomRequestValidator.ValidateAndThrow(createRoomRequest);

            var roomEntity = new Room(createRoomRequest.Capacity, createRoomRequest.Price, createRoomRequest.HotelId);
            _roomRepository.AddRoom(roomEntity);
            return new RoomResponse { Id = roomEntity.Id, Capacity = roomEntity.Capacity, Price = roomEntity.Price, HotelId = roomEntity.HotelId };
        }

        public List<RoomResponse> GetAllRooms()
        {
            var rooms = _roomRepository.GetAllRooms();
            return rooms.Select(r => new RoomResponse
            {
                Id = r.Id,
                Capacity = r.Capacity,
                Price = r.Price,
                HotelId = r.HotelId
            }).ToList();
        }

        public RoomResponse GetRoomById(Guid id)
        {
            var room = _roomRepository.GetRoomById(id);
            return new RoomResponse { Id = room.Id, Capacity = room.Capacity, Price = room.Price, HotelId = room.HotelId };
        }

        public RoomResponse UpdateRoom(Guid id, UpdateRoomRequest updateRoomRequest)
        {
            _updateRoomRequestValidator.ValidateAndThrow(updateRoomRequest);

            var roomEntity = new Room(updateRoomRequest.Capacity, updateRoomRequest.Price, Guid.Empty);
            var updatedRoom = _roomRepository.UpdateRoom(id, roomEntity);
            return new RoomResponse { Id = updatedRoom.Id, Capacity = updatedRoom.Capacity, Price = updatedRoom.Price, HotelId = updatedRoom.HotelId };
        }

        public RoomResponse DeleteRoom(Guid id)
        {
            var deletedRoom = _roomRepository.DeleteRoom(id);
            return new RoomResponse { Id = deletedRoom.Id, Capacity = deletedRoom.Capacity, Price = deletedRoom.Price, HotelId = deletedRoom.HotelId };
        }
    }
}
