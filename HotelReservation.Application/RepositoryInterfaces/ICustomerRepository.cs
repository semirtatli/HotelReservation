using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        public void AddCustomer(Customer customer);
        public List<Customer> GetAllCustomers();
        public Customer GetCustomerById(Guid id);
        public bool UpdateCustomer(Guid id, Customer customer);
        public Customer DeleteCustomer(Guid id);
    }
}
