using HotelReservation.Application.DTO;

namespace HotelReservation.Application.Interfaces
{
    public interface ICustomerService
    {
        public void AddCustomer(CustomerRequest customerRequest);
        public List<CustomerResponse> GetAllCustomers();
        public CustomerResponse GetCustomerById(Guid id);
        public CustomerResponse UpdateCustomer(Guid id, UpdateCustomerRequest request);
        public CustomerResponse DeleteCustomer(Guid id);
    }
}
