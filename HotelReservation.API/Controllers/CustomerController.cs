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
        public ActionResult AddCustomer(CustomerRequest customer)
        {
            service.AddCustomer(customer);
            return Ok();
        }

    }
}
