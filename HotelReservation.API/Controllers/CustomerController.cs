using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController(ICustomerService service) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> AddCustomer(CustomerRequest customerRequest)
        {
            await service.AddCustomerAsync(customerRequest);
            return Created();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCustomers()
        {
            var customers = await service.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomerById(Guid id)
        {
            var customer = await service.GetCustomerByIdAsync(id);
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerRequest updateCustomerRequest)
        {
            var updatedCustomer = await service.UpdateCustomerAsync(id, updateCustomerRequest);
            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(Guid id)
        {
            var deletedCustomer = await service.DeleteCustomerAsync(id);
            return Ok(deletedCustomer);
        }
    }
}
