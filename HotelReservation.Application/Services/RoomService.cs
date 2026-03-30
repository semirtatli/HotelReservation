using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository _roomRepository) { 
            this._roomRepository = _roomRepository;
        }

        public void AddRoom(Room room)
        {
            _roomRepository.AddRoom(room);
        }
    }
}
