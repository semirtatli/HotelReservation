using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.Mappers;
using HotelReservation.Domain.RepositoryInterfaces;
using HotelReservation.Domain.Exceptions;

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
            var room = RoomMapper.ToEntity(request);
            await _roomRepository.AddRoomAsync(room);
            return RoomMapper.ToResponse(room);
        }

        public async Task<List<RoomResponse>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetAllRoomsAsync();
            return rooms.Select(RoomMapper.ToResponse).ToList();
        }

        public async Task<RoomResponse> GetRoomByIdAsync(Guid id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room is null) throw new NotFoundException("Room not found");
            return RoomMapper.ToResponse(room);
        }

        public async Task<RoomResponse> UpdateRoomAsync(Guid id, UpdateRoomRequest request)
        {
            _updateRoomRequestValidator.ValidateAndThrow(request);
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room is null) throw new NotFoundException("Room not found");
            room.UpdateCapacity(request.Capacity);
            room.UpdatePrice(request.Price);
            room.UpdateRoomType(request.RoomType);
            await _roomRepository.UpdateRoomAsync(room);
            return RoomMapper.ToResponse(room);
        }

        public async Task<RoomResponse> DeleteRoomAsync(Guid id)
        {
            var deletedRoom = await _roomRepository.DeleteRoomAsync(id);
            if (deletedRoom is null) throw new NotFoundException("Room not found");
            return RoomMapper.ToResponse(deletedRoom);
        }
    }
}
