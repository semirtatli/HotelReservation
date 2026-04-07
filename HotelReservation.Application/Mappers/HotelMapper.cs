using HotelReservation.Application.DTO;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Mappers
{
    public static class HotelMapper
    {
        public static Hotel ToEntity(CreateHotelRequest request) =>
            new(request.Name);

        public static HotelResponse ToResponse(Hotel hotel) => new()
        {
            Id = hotel.Id,
            Name = hotel.Name
        };
    }
}
