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
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository _roomRepository)
        {
            this._roomRepository = _roomRepository;
        }

        public RoomResponse AddRoom(CreateRoomRequest createRoomRequest)
        {
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
            var roomEntity = new Room(updateRoomRequest.Capacity, updateRoomRequest.Price, Guid.Empty);
            var result = _roomRepository.UpdateRoom(id, roomEntity);
            if (!result) throw new Exception("Room not found");
            return new RoomResponse { Capacity = roomEntity.Capacity, Price = roomEntity.Price };
        }

        public RoomResponse DeleteRoom(Guid id)
        {
            var deletedRoom = _roomRepository.DeleteRoom(id);
            return new RoomResponse { Id = deletedRoom.Id, Capacity = deletedRoom.Capacity, Price = deletedRoom.Price, HotelId = deletedRoom.HotelId };
        }
    }
}
