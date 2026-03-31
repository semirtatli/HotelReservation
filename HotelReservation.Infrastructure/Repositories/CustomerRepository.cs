using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext _context)
        {
            this._context = _context;
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer GetCustomerById(Guid id)
        {
            var customer = _context.Customers.Find(id);
            if (customer is null) throw new Exception("Customer not found");
            return customer;
        }

        public bool UpdateCustomer(Guid id, Customer customer)
        {
            var existingCustomer = _context.Customers.Find(id);
            if (existingCustomer != null)
            {
                existingCustomer.UpdateName(customer.Name);
                existingCustomer.UpdateDateOfBirth(customer.DateOfBirth);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public Customer DeleteCustomer(Guid id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
                return customer;
            }
            throw new Exception("Customer not found");
        }
    }
}
