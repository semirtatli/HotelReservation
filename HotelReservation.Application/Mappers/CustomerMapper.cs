using HotelReservation.Application.DTO;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Mappers
{
    public static class CustomerMapper
    {
        public static Customer ToEntity(CustomerRequest request) =>
            new(request.Name, request.DateOfBirth, request.Email, request.PhoneNumber);

        public static Customer ToEntity(UpdateCustomerRequest request) =>
            new(request.Name, request.DateOfBirth, request.Email, request.PhoneNumber);

        public static CustomerResponse ToResponse(Customer customer) => new()
        {
            Id = customer.Id,
            Name = customer.Name,
            DateOfBirth = customer.DateOfBirth.ToString("dd/MM/yyyy"),
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber
        };
    }
}
