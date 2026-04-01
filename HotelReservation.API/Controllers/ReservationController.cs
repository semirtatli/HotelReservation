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
        public ActionResult AddReservation([FromBody] CreateReservationRequest createReservationRequest)
        {
            var response = service.AddReservation(createReservationRequest);
            return Ok(response);
        }

        [HttpGet]
        public ActionResult GetAllReservations()
        {
            var reservations = service.GetAllReservations();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public ActionResult GetReservationById(Guid id)
        {
            var reservation = service.GetReservationById(id);
            return Ok(reservation);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateReservation(Guid id, [FromBody] UpdateReservationRequest updateReservationRequest)
        {
            var response = service.UpdateReservation(id, updateReservationRequest);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteReservation(Guid id)
        {
            var deletedReservation = service.DeleteReservation(id);
            return Ok(deletedReservation);
        }
    }
}
