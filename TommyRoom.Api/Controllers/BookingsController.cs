using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TommyRoom.Api.Data;
using TommyRoom.Api.Helpers;
using TommyRoom.Shared.DTOs;
using TommyRoom.Shared.Entities;

namespace TommyRoom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookingsController(DataContext dataContext, IUserHelper userHelper) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IUserHelper _userHelper = userHelper;


        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            try
            {
                return Ok(await _dataContext.Bookings
                        .Include(b => b.User)
                        .Include(b => b.Room).ThenInclude(r => r!.User)
                        .OrderByDescending(b => b.EndTime)
                        .ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/Bookings/#
        [HttpGet("{BookingId:int}")]
        public async Task<ActionResult<Booking>> GetBooking(int BookingId)
        {
            Booking? booking = await _dataContext.Bookings.FindAsync(BookingId);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/#
        [HttpPut]
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
        public async Task<ActionResult<CreatedBookingDTO>> PostBooking([FromBody] CreatedBookingDTO DTO)
        {
            if (DTO.StartTime is null || DTO.EndTime is null) return BadRequest("Las Fechas son Obligatorias");
            if (DTO.EndTime <= DTO.StartTime) return BadRequest("La fecha de fin debe ser posterior a la de inicio.");

            bool conflic = _dataContext.Bookings.Any(b => b.RoomId == DTO.RoomId && b.StartTime < DTO.EndTime && b.EndTime > DTO.StartTime);
            if (conflic) return BadRequest("La Habitación ya está Reservada en esas Fechas 😖 ...");

            User userLog = await _userHelper.GetUserAsync(User.Identity!.Name!);
            if (userLog == null) return Unauthorized("User not authenticated 😖 ...");

            Room? room = await _dataContext.Rooms.FindAsync(DTO.RoomId);
            if (room == null) return Unauthorized("Habitación NoTa 😖 ...");

            int days = (DTO.EndTime!.Value - DTO.StartTime!.Value).Days;
            decimal totalPrice = days * room.PricePerNight;

            Booking booking = new()
            {
                StartTime = DTO.StartTime.Value.ToUniversalTime(),
                EndTime = DTO.EndTime.Value.ToUniversalTime(),
                RoomId = DTO.RoomId,
                TotalPrice = totalPrice,
                GuestId = userLog.Id,
            };

            try
            {
                _dataContext.Bookings.Add(booking);
                await _dataContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Bookings/#
        [HttpDelete("{BookingId:int}")]
        public async Task<IActionResult> DeleteBooking(int BookingId)
        {
            var booking = await _dataContext.Bookings.FindAsync(BookingId);
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
