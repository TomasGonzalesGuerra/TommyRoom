using Microsoft.AspNetCore.SignalR;

namespace TommyRoom.Api.Hubs;

public class NotificationHub : Hub
{
    // El admin se une al grupo "admins" al conectarse
    public async Task JoinAdminGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "admins");
    }

    // El usuario se une a su grupo personal para recibir notifs propias
    public async Task JoinUserGroup(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
    }
}