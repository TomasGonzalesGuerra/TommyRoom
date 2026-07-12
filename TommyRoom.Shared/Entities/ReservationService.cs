namespace TommyRoom.Shared.Entities;

public class ReservationService
{
    public int Id { get; set; }
    public int Quantity { get; set; } = 1;

    // Precio congelado al momento de agregar el servicio a la reserva.
    public decimal UnitPriceSnapshot { get; set; }

    public int ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    public int ServiceId { get; set; }
    public Service? Service { get; set; }
}