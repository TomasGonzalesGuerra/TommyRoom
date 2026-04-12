namespace TommyRoom.Shared.DTOs;

public class NotificationDTO
{
    public string Type { get; set; } = string.Empty;  // "booking_created" | "booking_cancelled" | "room_updated"
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? RoomId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
