
using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository _customerRepository) { 
            this._customerRepository = _customerRepository;
        }

        public void AddCustomer(CustomerRequest customerRequest)
        {
            var customer = new Customer
            (
                customerRequest.name,
               customerRequest.DateOfBirth
            );
            // Implementation for adding a customer to the database or in-memory collection
            // This is a placeholder and should be replaced with actual data access code
            _customerRepository.AddCustomer(customer);
            Console.WriteLine($"Customer added: Name={customer.Name}, DateOfBirth={customer.DateOfBirth}");
        }
    }
}
