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
        public async Task<ActionResult> AddRoom([FromBody] CreateRoomRequest createRoomRequest)
        {
            var response = await service.AddRoomAsync(createRoomRequest);
            return CreatedAtAction(nameof(GetRoomById), new { id = response.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRooms()
        {
            var rooms = await service.GetAllRoomsAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetRoomById(Guid id)
        {
            var room = await service.GetRoomByIdAsync(id);
            return Ok(room);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomRequest updateRoomRequest)
        {
            var updatedRoom = await service.UpdateRoomAsync(id, updateRoomRequest);
            return Ok(updatedRoom);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoom(Guid id)
        {
            var deletedRoom = await service.DeleteRoomAsync(id);
            return Ok(deletedRoom);
        }
    }
}
