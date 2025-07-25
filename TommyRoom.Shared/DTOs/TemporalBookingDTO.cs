namespace TommyRoom.Shared.DTOs;

public class TemporalBookingDTO
{
    public int RoomId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Photo { get; set; }
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}
