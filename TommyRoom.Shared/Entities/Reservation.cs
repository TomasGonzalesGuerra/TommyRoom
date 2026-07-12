using TommyRoom.Shared.Enums;

namespace TommyRoom.Shared.Entities;

public class Reservation
{
    public int Id { get; set; }
    public ReservationStatus ReservationStatus { get; set; } = ReservationStatus.Pending;
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; } = null!;
    public User? User { get; set; }

    public ICollection<ReservationRoom> ReservationRooms { get; set; } = [];
    public ICollection<ReservationService> ReservationServices { get; set; } = [];
    public ICollection<Payment> Payments { get; set; } = [];
}