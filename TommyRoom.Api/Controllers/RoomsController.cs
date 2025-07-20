using Humanizer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TommyRoom.Api.Data;
using TommyRoom.Api.Helpers;
using TommyRoom.Shared.DTOs;
using TommyRoom.Shared.Entities;

namespace TommyRoom.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RoomsController(DataContext dataContext, IUserHelper userHelper) : ControllerBase
{
    private readonly DataContext _dataContext = dataContext;
    private readonly IUserHelper _userHelper = userHelper;


    // GET: api/Rooms
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Room>>> GetRooms() => await _dataContext.Rooms.Include(r => r.User).Include(r => r.Bookings).ToListAsync();

    // GET: api/Rooms/OnlyAvilable
    [HttpGet("OnlyAvilable")]
    public async Task<ActionResult<IEnumerable<Room>>> GetOnlyAvilableRooms() => await _dataContext.Rooms.Where(r => r.IsAvailable == true).ToListAsync();

    // GET: api/Rooms/#
    [HttpGet("{RoomId:int}")]
    public async Task<ActionResult<Room>> GetRoom(int RoomId)
    {
        Room? room = await _dataContext.Rooms
            .Include(r => r.User)
            .Include(r => r.Bookings!).ThenInclude(b => b.User)
            .FirstOrDefaultAsync(r => r.Id == RoomId);

        if (room == null) return NotFound();

        return Ok(room);
    }

    // PUT: api/Rooms/#
    [HttpPut]
    public async Task<IActionResult> PutRoom(Room room)
    {
        _dataContext.Entry(room).State = EntityState.Modified;

        try
        {
            await _dataContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {

        }

        return NoContent();
    }

    // POST: api/Rooms
    [HttpPost]
    public async Task<ActionResult<CreatedRoomDTO>> PostRoom(CreatedRoomDTO DTO)
    {
        User userLog = await _userHelper.GetUserAsync(User.Identity!.Name!);
        if (userLog == null) return Unauthorized("User not authenticated 😖 ...");

        Room room = new()
        {
            Name = DTO.Name,
            Location = DTO.Location,
            Description = DTO.Description,
            Capacity = DTO.Capacity,
            PricePerNight = DTO.PricePerNight,
            Photo = "https://img.freepik.com/vector-premium/ilustracion-sala-reuniones-negocios_135595-39779.jpg",
            IsAvailable = true,
            OwnerId = userLog.Id,
        };

        try
        {
            _dataContext.Rooms.Add(room);
            await _dataContext.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/Rooms/#
    [HttpDelete("{RoomId:int}")]
    public async Task<IActionResult> DeleteRoom(int RoomId)
    {
        Room? room = await _dataContext.Rooms.FindAsync(RoomId);
        if (room == null) return BadRequest("Tabla NO Encontrada");

        _dataContext.Rooms.Remove(room);
        await _dataContext.SaveChangesAsync();

        return Ok(NoContent());
    }

}
