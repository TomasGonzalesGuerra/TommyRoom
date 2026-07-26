namespace TommyRoom.Shared.DTOs.Auth;

public class TokenDTO
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserDTO User { get; set; } = null!;
}
