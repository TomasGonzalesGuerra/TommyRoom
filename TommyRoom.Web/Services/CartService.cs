using Blazored.LocalStorage;
using TommyRoom.Shared.DTOs;

namespace TommyRoom.Web.Services;

public class CartService(ILocalStorageService localStorage) : ICartService
{
    private readonly ILocalStorageService _localStorage = localStorage;
    private const string CartKey = "CartKey";
    public event Func<Task>? OnChangeAsync;
    private List<CartItemDTO> Items { get; set; } = [];


    public async Task<List<CartItemDTO>> LoadAsync() => Items = await _localStorage.GetItemAsync<List<CartItemDTO>>(CartKey) ?? [];

    public async Task AddItemAsync(CartItemDTO item)
    {
        Items.Add(item);
        await _localStorage.SetItemAsync(CartKey, Items);
        await NotifyStateChangedAsync();
    }

    public async Task RemoveItemAsync(CartItemDTO item)
    {
        Items.Remove(item);
        await _localStorage.SetItemAsync(CartKey, Items);
        await NotifyStateChangedAsync();
    }

    public async Task ClearAsync()
    {
        Items.Clear();
        await _localStorage.RemoveItemAsync(CartKey);
        await NotifyStateChangedAsync();
    }

    public int GetCount() => Items.Count;

    public async Task NotifyStateChangedAsync()
    {
        if (OnChangeAsync != null) await OnChangeAsync.Invoke();
    }

    //TODO: Implement API synchronization
    //public async Task SyncWithApiAsync(string userId) => await _http.PostAsJsonAsync($"api/cart/sync/{userId}", Items);
    //public async Task SaveToApiAsync() => await _http.PostAsJsonAsync("api/cart/temp", Items);
}
