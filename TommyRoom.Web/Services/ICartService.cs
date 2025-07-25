using TommyRoom.Shared.DTOs;

namespace TommyRoom.Web.Services
{
    public interface ICartService
    {
        event Func<Task>? OnChangeAsync;

        int GetCount();
        Task ClearAsync();
        Task NotifyStateChangedAsync();
        Task<List<CartItemDTO>> LoadAsync();
        Task AddItemAsync(CartItemDTO item);
        Task RemoveItemAsync(int roomId);
    }
}