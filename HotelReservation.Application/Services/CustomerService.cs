using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.Mappers;
using HotelReservation.Domain.RepositoryInterfaces;
using HotelReservation.Domain.Exceptions;

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
            var customer = CustomerMapper.ToEntity(customerRequest);
            await _customerRepository.AddCustomerAsync(customer);
        }

        public async Task<List<CustomerResponse>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();
            return customers.Select(CustomerMapper.ToResponse).ToList();
        }

        public async Task<CustomerResponse> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer is null) throw new NotFoundException("Customer not found");
            return CustomerMapper.ToResponse(customer);
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(Guid id, UpdateCustomerRequest request)
        {
            _updateCustomerRequestValidator.ValidateAndThrow(request);
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer is null) throw new NotFoundException("Customer not found");
            customer.UpdateName(request.Name);
            customer.UpdateDateOfBirth(request.DateOfBirth);
            customer.UpdateEmail(request.Email);
            customer.UpdatePhoneNumber(request.PhoneNumber);
            await _customerRepository.UpdateCustomerAsync(customer);
            return CustomerMapper.ToResponse(customer);
        }

        public async Task<CustomerResponse> DeleteCustomerAsync(Guid id)
        {
            var deletedCustomer = await _customerRepository.DeleteCustomerAsync(id);
            if (deletedCustomer is null) throw new NotFoundException("Customer not found");
            return CustomerMapper.ToResponse(deletedCustomer);
        }
    }
}
