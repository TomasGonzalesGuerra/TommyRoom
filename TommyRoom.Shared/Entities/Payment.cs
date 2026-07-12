using TommyRoom.Shared.Enums;

namespace TommyRoom.Shared.Entities;

public class Payment
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public PaymentType Type { get; set; } = PaymentType.Payment;
    public string? TransactionReference { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int ReservationId { get; set; }
    public Reservation? Reservation { get; set; }
}