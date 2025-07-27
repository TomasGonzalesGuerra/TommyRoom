using TommyRoom.Shared.DTOs;

namespace TommyRoom.Web.Services;

public class ReservationNotificationState
{
    public bool HasNewNotification { get; private set; }
    public CreatedBookingDTO? LatestReservation { get; private set; }

    public event Action? OnChange;


    public void Notify(CreatedBookingDTO booking)
    {
        HasNewNotification = true;
        LatestReservation = booking;
        NotifyStateChange();
    }

    public void Clear()
    {
        HasNewNotification = false;
        LatestReservation = null;
        NotifyStateChange();
    }

    public void NotifyStateChange() => OnChange?.Invoke();
}
