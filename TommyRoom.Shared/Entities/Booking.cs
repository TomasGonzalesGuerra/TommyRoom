using System.ComponentModel.DataAnnotations;

namespace TommyRoom.Shared.Entities;

public class Booking
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal TotalPrice { get; set; }


    public Room? Room { get; set; }
    public int RoomId { get; set; }

    public User? User { get; set; }
    [Display(Name = "Huesped")]
    public string? UserId { get; set; }
}