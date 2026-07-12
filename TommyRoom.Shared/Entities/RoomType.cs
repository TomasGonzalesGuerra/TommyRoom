namespace TommyRoom.Shared.Entities;

public class RoomType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal BaseRate { get; set; }
    public int MaxCapacity { get; set; }

    public ICollection<Room> Rooms { get; set; } = [];
}