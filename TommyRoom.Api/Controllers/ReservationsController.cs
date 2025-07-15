using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TommyRoom.Api.Data;
using TommyRoom.Shared.Entities;

namespace TommyRoom.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController(DataContext dataContext) : ControllerBase
{
    private readonly DataContext _dataContext = dataContext;


    // GET: api/Reservations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
    {
        return await _dataContext.Reservations.ToListAsync();
    }

    // GET: api/Reservations/#
    [HttpGet("{id}")]
    public async Task<ActionResult<Reservation>> GetReservation(int id)
    {
        var reservation = await _dataContext.Reservations.FindAsync(id);

        if (reservation == null)
        {
            return NotFound();
        }

        return reservation;
    }

    // PUT: api/Reservations/#
    [HttpPut]
    public async Task<IActionResult> PutReservation(Reservation reservation)
    {
        _dataContext.Entry(reservation).State = EntityState.Modified;

        try
        {
            await _dataContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {

        }

        return NoContent();
    }

    // POST: api/Reservations
    [HttpPost]
    public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
    {
        _dataContext.Reservations.Add(reservation);
        await _dataContext.SaveChangesAsync();

        return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);
    }

    // DELETE: api/Reservations/#
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        var reservation = await _dataContext.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }

        _dataContext.Reservations.Remove(reservation);
        await _dataContext.SaveChangesAsync();

        return NoContent();
    }

}
