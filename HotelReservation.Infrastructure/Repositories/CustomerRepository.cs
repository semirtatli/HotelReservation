using HotelReservation.Domain.RepositoryInterfaces;
using HotelReservation.Domain.Entities;
using HotelReservation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Customer?> DeleteCustomerAsync(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer is null) return null;
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}
