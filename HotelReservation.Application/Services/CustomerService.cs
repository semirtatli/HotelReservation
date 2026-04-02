using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<CustomerRequest> _customerRequestValidator;
        private readonly IValidator<UpdateCustomerRequest> _updateCustomerRequestValidator;

        public CustomerService(ICustomerRepository customerRepository, IValidator<CustomerRequest> customerRequestValidator, IValidator<UpdateCustomerRequest> updateCustomerRequestValidator)
        {
            _customerRepository = customerRepository;
            _customerRequestValidator = customerRequestValidator;
            _updateCustomerRequestValidator = updateCustomerRequestValidator;
        }

        public async Task AddCustomerAsync(CustomerRequest customerRequest)
        {
            _customerRequestValidator.ValidateAndThrow(customerRequest);
            var customer = new Customer(customerRequest.Name, customerRequest.DateOfBirth);
            await _customerRepository.AddCustomerAsync(customer);
        }

        public async Task<List<CustomerResponse>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();
            return customers.Select(MapToResponse).ToList();
        }

        public async Task<CustomerResponse> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            return MapToResponse(customer);
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(Guid id, UpdateCustomerRequest updateCustomerRequest)
        {
            _updateCustomerRequestValidator.ValidateAndThrow(updateCustomerRequest);
            var customerEntity = new Customer(updateCustomerRequest.Name, updateCustomerRequest.DateOfBirth);
            var updatedCustomer = await _customerRepository.UpdateCustomerAsync(id, customerEntity);
            return MapToResponse(updatedCustomer);
        }

        public async Task<CustomerResponse> DeleteCustomerAsync(Guid id)
        {
            var deletedCustomer = await _customerRepository.DeleteCustomerAsync(id);
            return MapToResponse(deletedCustomer);
        }

        private static CustomerResponse MapToResponse(Customer customer) => new()
        {
            Id = customer.Id,
            Name = customer.Name,
            DateOfBirth = customer.DateOfBirth.ToString("dd/MM/yyyy")
        };
    }
}
