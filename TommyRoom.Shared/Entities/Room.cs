using System.ComponentModel.DataAnnotations;

namespace TommyRoom.Shared.Entities;

public class Room
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public bool IsAvailable { get; set; }
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    [Display(Name = "Foto")]
    public string? Photo { get; set; }

    public User? User { get; set; }
    public string? OwnerId { get; set; }

    public ICollection<Booking>? Bookings { get; set; }
}