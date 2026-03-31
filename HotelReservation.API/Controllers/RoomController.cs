using HotelReservation.Application.DTO;
using HotelReservation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController(IRoomService service) : ControllerBase
    {
        [HttpPost]
        public ActionResult AddRoom([FromBody] CreateRoomRequest createRoomRequest)
        {
            var response = service.AddRoom(createRoomRequest);
            return Ok(response);
        }

        [HttpGet]
        public ActionResult GetAllRooms()
        {
            var rooms = service.GetAllRooms();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public ActionResult GetRoomById(Guid id)
        {
            var room = service.GetRoomById(id);
            return Ok(room);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateRoom(Guid id, [FromBody] UpdateRoomRequest updateRoomRequest)
        {
            var updatedRoom = service.UpdateRoom(id, updateRoomRequest);
            return Ok(updatedRoom);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteRoom(Guid id)
        {
            var deletedRoom = service.DeleteRoom(id);
            return Ok(deletedRoom);
        }
    }
}
