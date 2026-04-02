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
        public async Task<ActionResult> AddHotel([FromBody] CreateHotelRequest createHotelRequest)
        {
            var response = await service.AddHotelAsync(createHotelRequest);
            return CreatedAtAction(nameof(GetHotelById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateHotel(Guid id, [FromBody] UpdateHotelRequest updateHotelRequest)
        {
            var updatedHotel = await service.UpdateHotelAsync(id, updateHotelRequest);
            return Ok(updatedHotel);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllHotels()
        {
            var hotels = await service.GetAllHotelsAsync();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetHotelById(Guid id)
        {
            var hotel = await service.GetHotelByIdAsync(id);
            return Ok(hotel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHotel(Guid id)
        {
            var deletedHotel = await service.DeleteHotelAsync(id);
            return Ok(deletedHotel);
        }
    }
}
