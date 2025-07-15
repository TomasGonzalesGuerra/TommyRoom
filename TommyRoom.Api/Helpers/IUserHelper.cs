﻿using Microsoft.AspNetCore.Identity;
using TommyRoom.Shared.Entities;
using TommyRoom.Shared.DTOs;

namespace TommyRoom.Api.Helpers
{
    public interface IUserHelper
    {
        Task CheckRoleAsync(string roleName);
        Task AddUserToRoleAsync(User user, string roleName);
        Task<bool> IsUserInRoleAsync(User user, string roleName);
        Task LogoutAsync();
        Task<User> GetUserAsync(string email);
        Task<User> GetUserAsync(Guid userId);
        Task<SignInResult> LoginAsync(LoginDTO model);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    }
}
