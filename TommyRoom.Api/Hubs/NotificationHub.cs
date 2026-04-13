using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace TommyRoom.Api.Hubs;

public class NotificationHub(ILogger<NotificationHub> logger) : Hub
{
    private readonly ILogger<NotificationHub> _logger = logger;

    public async Task JoinAdminGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "admins");
        _logger.LogInformation("Client {Id} joined admins group", Context.ConnectionId);
    }

    public async Task JoinUserGroup(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        _logger.LogInformation("Client {Id} joined user_{UserId} group", Context.ConnectionId, userId);
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {Id}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }
}