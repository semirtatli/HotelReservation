using HotelReservation.Application.DTO;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Mappers
{
    public static class RoomMapper
    {
        public static Room ToEntity(CreateRoomRequest request) =>
            new(request.Capacity, request.Price, request.HotelId, request.RoomType);

        public static RoomResponse ToResponse(Room room) => new()
        {
            Id = room.Id,
            Capacity = room.Capacity,
            Price = room.BasePrice,
            HotelId = room.HotelId,
            RoomType = room.RoomType
        };
    }
}
