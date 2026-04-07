using HotelReservation.Domain.Entities;

namespace HotelReservation.Domain.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task UpdateCustomerAsync(Customer customer);
        Task<Customer?> DeleteCustomerAsync(Guid id);
    }
}
