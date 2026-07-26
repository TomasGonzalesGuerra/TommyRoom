using Microsoft.AspNetCore.Identity;
using TommyRoom.Shared.Entities;
using TommyRoom.Shared.DTOs.Auth;

namespace TommyRoom.Api.Helpers;

public interface IUserHelper
{
    Task<IdentityResult> AddUserAsync(User user, string password);
    Task AddUserToRoleAsync(User user, string roleName);
    Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    Task CheckRoleAsync(string roleName);
    Task<User> GetUserAsync(Guid userId);
    Task<User> GetUserAsync(string email);
    Task<bool> IsUserInRoleAsync(User user, string roleName);
    Task<SignInResult> LoginAsync(LoginDTO model);
    Task LogoutAsync();
    Task<IdentityResult> UpdateUserAsync(User user);
}