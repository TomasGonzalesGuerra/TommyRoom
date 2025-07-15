using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TommyRoom.Api.Data;
using TommyRoom.Shared.Entities;

namespace TommyRoom.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(DataContext dataContext) : ControllerBase
{
    private readonly DataContext _dataContext = dataContext;


    // GET: api/Rooms
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
    {
        return await _dataContext.Rooms.ToListAsync();
    }

    // GET: api/Rooms/#
    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> GetRoom(int id)
    {
        Room? room = await _dataContext.Rooms.FindAsync(id);
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
    public async Task<ActionResult<Room>> PostRoom(Room room)
    {
        _dataContext.Rooms.Add(room);
        await _dataContext.SaveChangesAsync();

        return Ok(room);
    }

    // DELETE: api/Rooms/#
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        Room? room = await _dataContext.Rooms.FindAsync(id);
        if (room == null) return NotFound();

        _dataContext.Rooms.Remove(room);
        await _dataContext.SaveChangesAsync();

        return Ok(NoContent());
    }

}
