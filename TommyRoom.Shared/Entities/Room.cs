using System.ComponentModel.DataAnnotations;
using TommyRoom.Shared.Enums;

namespace TommyRoom.Shared.Entities;

public class Room
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
    public RoomStatus RoomStatus { get; set; } = RoomStatus.Available;
    public string? Photo { get; set; }

    public int RoomTypeId { get; set; }
    public RoomType RoomType { get; set; } = null!;

    public ICollection<ReservationRoom> ReservationRooms { get; set; } = [];
}
