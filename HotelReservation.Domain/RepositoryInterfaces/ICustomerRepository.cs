using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task<Customer?> UpdateCustomerAsync(Guid id, Customer customer);
        Task<Customer?> DeleteCustomerAsync(Guid id);
    }
}
