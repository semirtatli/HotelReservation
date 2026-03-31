using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelController(IHotelService service) : ControllerBase
    {
        [HttpPost]
        public ActionResult AddHotel([FromBody] CreateHotelRequest createHotelRequest)
        {
            var response = service.AddHotel(createHotelRequest);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateHotel(Guid id, [FromBody] UpdateHotelRequest updateHotelRequest)
        {
            var updatedHotel = service.UpdateHotel(id, updateHotelRequest);
            return Ok(updatedHotel);
        }

        [HttpGet]
        public ActionResult GetAllHotels()
        {
            var hotels = service.GetAllHotels();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public ActionResult GetHotelById(Guid id)
        {
            var hotel = service.GetHotelById(id);
            return Ok(hotel);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteHotel(Guid id)
        {
            var deletedHotel = service.DeleteHotel(id);
            return Ok(deletedHotel);
        }
    }
}
