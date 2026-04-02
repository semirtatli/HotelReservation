using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IValidator<CreateRoomRequest> _createRoomRequestValidator;
        private readonly IValidator<UpdateRoomRequest> _updateRoomRequestValidator;

        public RoomService(IRoomRepository roomRepository, IValidator<CreateRoomRequest> createRoomRequestValidator, IValidator<UpdateRoomRequest> updateRoomRequestValidator)
        {
            _roomRepository = roomRepository;
            _createRoomRequestValidator = createRoomRequestValidator;
            _updateRoomRequestValidator = updateRoomRequestValidator;
        }

        public async Task<RoomResponse> AddRoomAsync(CreateRoomRequest request)
        {
            _createRoomRequestValidator.ValidateAndThrow(request);
            var room = new Room(request.Capacity, request.Price, request.HotelId, request.RoomType);
            await _roomRepository.AddRoomAsync(room);
            return MapToResponse(room);
        }

        public async Task<List<RoomResponse>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetAllRoomsAsync();
            return rooms.Select(MapToResponse).ToList();
        }

        public async Task<RoomResponse> GetRoomByIdAsync(Guid id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            return MapToResponse(room);
        }

        public async Task<RoomResponse> UpdateRoomAsync(Guid id, UpdateRoomRequest request)
        {
            _updateRoomRequestValidator.ValidateAndThrow(request);
            var room = await _roomRepository.UpdateRoomAsync(id, request.Capacity, request.Price, request.RoomType);
            return MapToResponse(room);
        }

        public async Task<RoomResponse> DeleteRoomAsync(Guid id)
        {
            var deletedRoom = await _roomRepository.DeleteRoomAsync(id);
            return MapToResponse(deletedRoom);
        }

        private static RoomResponse MapToResponse(Room room) => new()
        {
            Id = room.Id,
            Capacity = room.Capacity,
            Price = room.BasePrice,
            HotelId = room.HotelId,
            RoomType = room.RoomType
        };
    }
}
