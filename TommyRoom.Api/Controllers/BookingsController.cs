using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TommyRoom.Api.Data;
using TommyRoom.Shared.Entities;

namespace TommyRoom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController(DataContext dataContext) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;


        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _dataContext.Bookings.ToListAsync();
        }

        // GET: api/Bookings/#
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _dataContext.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/#
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(Booking booking)
        {
            _dataContext.Entry(booking).State = EntityState.Modified;

            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return NoContent();
        }

        // POST: api/Bookings
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            _dataContext.Bookings.Add(booking);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }

        // DELETE: api/Bookings/#
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _dataContext.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _dataContext.Bookings.Remove(booking);
            await _dataContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
