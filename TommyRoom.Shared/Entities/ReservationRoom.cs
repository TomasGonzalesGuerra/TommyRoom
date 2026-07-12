namespace TommyRoom.Shared.Entities;

public class ReservationRoom
{
    public int Id { get; set; }

    // Tarifa congelada al momento de crear la reserva, para no
    // verse afectada si más adelante cambia la tarifa del RoomType.
    public decimal RatePerNight { get; set; }

    public int ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    public int RoomId { get; set; }
    public Room? Room { get; set; }
}