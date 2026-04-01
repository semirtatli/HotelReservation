using FluentValidation;
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HotelReservation.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<CustomerRequest> _customerRequestValidator;
        private readonly IValidator<UpdateCustomerRequest> _updateCustomerRequestValidator;

        public CustomerService(ICustomerRepository _customerRepository, IValidator<CustomerRequest> customerRequestValidator, IValidator<UpdateCustomerRequest> updateCustomerRequestValidator)
        {
            this._customerRepository = _customerRepository;
            _customerRequestValidator = customerRequestValidator;
            _updateCustomerRequestValidator = updateCustomerRequestValidator;
        }

        public void AddCustomer(CustomerRequest customerRequest)
        {
            _customerRequestValidator.ValidateAndThrow(customerRequest);

            var dob = DateOnly.ParseExact(customerRequest.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var customer = new Customer(customerRequest.Name, dob);
            _customerRepository.AddCustomer(customer);
        }

        public List<CustomerResponse> GetAllCustomers()
        {
            var customers = _customerRepository.GetAllCustomers();
            return customers.Select(c => new CustomerResponse
            {
                Id = c.Id,
                Name = c.Name,
                DateOfBirth = c.DateOfBirth.ToString("dd/MM/yyyy")
            }).ToList();
        }

        public CustomerResponse GetCustomerById(Guid id)
        {
            var customer = _customerRepository.GetCustomerById(id);
            return new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                DateOfBirth = customer.DateOfBirth.ToString("dd/MM/yyyy")
            };
        }

        public CustomerResponse UpdateCustomer(Guid id, UpdateCustomerRequest updateCustomerRequest)
        {
            _updateCustomerRequestValidator.ValidateAndThrow(updateCustomerRequest);

            var dob = DateOnly.ParseExact(updateCustomerRequest.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var customerEntity = new Customer(updateCustomerRequest.Name, dob);
            var updatedCustomer = _customerRepository.UpdateCustomer(id, customerEntity);
            return new CustomerResponse { Id = updatedCustomer.Id, Name = updatedCustomer.Name, DateOfBirth = updatedCustomer.DateOfBirth.ToString("dd/MM/yyyy") };
        }

        public CustomerResponse DeleteCustomer(Guid id)
        {
            var deletedCustomer = _customerRepository.DeleteCustomer(id);
            return new CustomerResponse
            {
                Id = deletedCustomer.Id,
                Name = deletedCustomer.Name,
                DateOfBirth = deletedCustomer.DateOfBirth.ToString("dd/MM/yyyy")
            };
        }
    }
}
