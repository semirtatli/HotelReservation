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
        public ActionResult AddCustomer(CustomerRequest customerRequest)
        {
            service.AddCustomer(customerRequest);
            return Ok();
        }

        [HttpGet]
        public ActionResult GetAllCustomers()
        {
            var customers = service.GetAllCustomers();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public ActionResult GetCustomerById(Guid id)
        {
            var customer = service.GetCustomerById(id);
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCustomer(Guid id, [FromBody] UpdateCustomerRequest updateCustomerRequest)
        {
            var updatedCustomer = service.UpdateCustomer(id, updateCustomerRequest);
            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCustomer(Guid id)
        {
            var deletedCustomer = service.DeleteCustomer(id);
            return Ok(deletedCustomer);
        }
    }
}
