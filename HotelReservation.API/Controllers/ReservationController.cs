using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController(IReservationService service) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> AddReservation([FromBody] CreateReservationRequest createReservationRequest)
        {
            var response = await service.AddReservationAsync(createReservationRequest);
            return CreatedAtAction(nameof(GetReservationById), new { id = response.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllReservations()
        {
            var reservations = await service.GetAllReservationsAsync();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetReservationById(Guid id)
        {
            var reservation = await service.GetReservationByIdAsync(id);
            return Ok(reservation);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateReservation(Guid id, [FromBody] UpdateReservationRequest updateReservationRequest)
        {
            var response = await service.UpdateReservationAsync(id, updateReservationRequest);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReservation(Guid id)
        {
            var deletedReservation = await service.DeleteReservationAsync(id);
            return Ok(deletedReservation);
        }
    }
}
