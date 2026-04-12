using TommyRoom.Shared.DTOs;

namespace TommyRoom.Api.Hubs;

public interface INotificationClient
{
    Task ReceiveNotification(NotificationDTO notification);
}