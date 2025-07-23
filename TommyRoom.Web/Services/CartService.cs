using Blazored.LocalStorage;
using TommyRoom.Shared.DTOs;

namespace TommyRoom.Web.Services;

public class CartService(ILocalStorageService localStorage) : ICartService
{
    private readonly ILocalStorageService _localStorage = localStorage;
    private const string CartKey = "CartKey";
    private List<CartItemDTO> Items { get; set; } = [];


    public async Task LoadAsync() => Items = await _localStorage.GetItemAsync<List<CartItemDTO>>(CartKey) ?? [];

    public async Task AddItemAsync(CartItemDTO item)
    {
        Items.Add(item);
        await _localStorage.SetItemAsync(CartKey, Items);
    }

    public async Task RemoveItemAsync(CartItemDTO item)
    {
        Items.Remove(item);
        await _localStorage.SetItemAsync(CartKey, Items);
    }

    public async Task ClearAsync()
    {
        Items.Clear();
        await _localStorage.RemoveItemAsync(CartKey);
    }

    //TODO: Implement API synchronization
    //public async Task SyncWithApiAsync(string userId) => await _http.PostAsJsonAsync($"api/cart/sync/{userId}", Items);
    //public async Task SaveToApiAsync() => await _http.PostAsJsonAsync("api/cart/temp", Items);
}
