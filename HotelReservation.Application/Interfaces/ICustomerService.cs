using HotelReservation.Application.DTO;

namespace HotelReservation.Application.Interfaces
{
    public interface ICustomerService
    {
        Task AddCustomerAsync(CustomerRequest customerRequest);
        Task<List<CustomerResponse>> GetAllCustomersAsync();
        Task<CustomerResponse> GetCustomerByIdAsync(Guid id);
        Task<CustomerResponse> UpdateCustomerAsync(Guid id, UpdateCustomerRequest request);
        Task<CustomerResponse> DeleteCustomerAsync(Guid id);
    }
}
