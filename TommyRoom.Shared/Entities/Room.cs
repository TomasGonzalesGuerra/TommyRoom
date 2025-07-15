namespace TommyRoom.Shared.Entities;

public class Room
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public int Capacity { get; set; }

    public ICollection<Reservation>? Reservations { get; set; }
}