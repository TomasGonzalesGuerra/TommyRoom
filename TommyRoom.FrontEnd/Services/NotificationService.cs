using TommyRoom.Shared.DTOs;
using Microsoft.AspNetCore.SignalR.Client;

namespace TommyRoom.Client.Services;

public class NotificationService : IAsyncDisposable
{
    private HubConnection? _connection;
    public event Action<NotificationDTO>? OnNotification;
    public bool IsConnected => _connection?.State == HubConnectionState.Connected;

    public async Task StartAsync(string hubUrl, string? userId = null, bool isAdmin = false)
    {
        if (_connection is not null) return;

        _connection = new HubConnectionBuilder().WithUrl(hubUrl).WithAutomaticReconnect().Build();

        _connection.On<NotificationDTO>("ReceiveNotification", notif =>
        {
            OnNotification?.Invoke(notif);
        });

        await _connection.StartAsync();

        if (isAdmin) await _connection.InvokeAsync("JoinAdminGroup");

        if (!string.IsNullOrEmpty(userId)) await _connection.InvokeAsync("JoinUserGroup", userId);
    }

    public async Task StopAsync()
    {
        if (_connection is not null) await _connection.StopAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null) await _connection.DisposeAsync();
    }
}