using TommyRoom.Shared.Entities;

namespace TommyRoom.Api.Services;

public interface ITokenService
{
    Task<(string Token, DateTime ExpiresAt)> GenerateTokenAsync(User user);
}