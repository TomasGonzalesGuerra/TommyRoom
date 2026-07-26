using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using TommyRoom.Web.Auth;
using TommyRoom.Shared.DTOs;
using TommyRoom.Web.Helpers;

namespace TommyRoom.Web.Services;

public class HubClient(IJSRuntime jsRuntime, IConfiguration Configuration) : IAsyncDisposable
{
    private Task? _startTask;
    private HubConnection? _hubConnection;
    private readonly string _tokenKey = "TOKEN_KEY";
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    private readonly IConfiguration _configuration = Configuration;

    //public event Action<OrderDTO>? OnNewOrderReceived;
    //public event Action<OrderDTO>? OnOrderStatusChanged;
    //public event Action<OrderDTO>? OnOrderAssignedToDriver;
    //public event Action<OrderStatusChangedDTO>? OnOrderReadyForPickup;   
    //public event Action<OrderStatusChangedDTO>? OnOrderStatusChangedLite; 


    public Task EnsureStartedAsync() => _startTask ??= StartAsync();

    private async Task StartAsync()
    {
        var token = await _jsRuntime.GetLocalStorage(_tokenKey);
        if (token is null) return;
        string baseApiUrl = _configuration["BackEndApiUrl"]!;
        string hubUrl = new Uri(new Uri(baseApiUrl!), "hubs/NotificationHub").ToString();

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult<string?>(token.ToString());
            })
            .WithAutomaticReconnect()
            .Build();

        //_hubConnection.On<OrderDTO>("NewOrderReceived", order => OnNewOrderReceived?.Invoke(order));
        //_hubConnection.On<OrderDTO>("OrderStatusChanged", order => OnOrderStatusChanged?.Invoke(order));
        //_hubConnection.On<OrderDTO>("OrderAssignedToDriver", order => OnOrderAssignedToDriver?.Invoke(order));
        //_hubConnection.On<OrderStatusChangedDTO>("OrderReadyForPickup", order => OnOrderReadyForPickup?.Invoke(order));
        //_hubConnection.On<OrderStatusChangedDTO>("OrderStatusChangedLite", update => OnOrderStatusChangedLite?.Invoke(update));

        await _hubConnection.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
}