namespace TommyRoom.Shared.Entities;

public class Reservation
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }


    public Room? Room { get; set; }
    public int RoomId { get; set; }

    public User? User { get; set; }
    public string? UserId { get; set; }
}