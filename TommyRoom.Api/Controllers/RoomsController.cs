using Humanizer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TommyRoom.Api.Data;
using TommyRoom.Shared.DTOs;
using TommyRoom.Shared.Entities;

namespace TommyRoom.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RoomsController(DataContext dataContext) : ControllerBase
{
    private readonly DataContext _dataContext = dataContext;


    // GET: api/Rooms
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Room>>> GetRooms() => await _dataContext.Rooms.ToListAsync();

    // GET: api/Rooms/OnlyAvilable
    [HttpGet("OnlyAvilable")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Room>>> GetOnlyAvilableRooms() => await _dataContext.Rooms.Where(r => r.IsAvailable == true).ToListAsync();

    // GET: api/Rooms/#
    [HttpGet("{RoomId:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Room>> GetRoom(int RoomId)
    {
        Room? room = await _dataContext.Rooms.Include(r => r.Bookings).FirstOrDefaultAsync(r => r.Id == RoomId);
        if (room == null) return NotFound();
        return room;
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
    public async Task<ActionResult<RoomCreatedDTO>> PostRoom(RoomCreatedDTO DTO)
    {
        string? user = User.Identity!.Name;
        if (user == null) return BadRequest("Logete Pz Wewasss...");

        Room room = new()
        {
            Name = DTO.Name,
            Location = DTO.Location,
            Description = DTO.Description,
            Capacity = DTO.Capacity,
            PricePerNight = DTO.PricePerNight,
            Photo = string.Empty,
            IsAvailable = true,
        };

        _dataContext.Rooms.Add(room);
        await _dataContext.SaveChangesAsync();

        return Ok();
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
