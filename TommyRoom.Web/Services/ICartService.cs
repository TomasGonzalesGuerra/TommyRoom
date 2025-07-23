using TommyRoom.Shared.DTOs;

namespace TommyRoom.Web.Services;

public interface ICartService
{
    Task AddItemAsync(CartItemDTO item);
    Task ClearAsync();
    Task LoadAsync();
    Task RemoveItemAsync(CartItemDTO item);
}