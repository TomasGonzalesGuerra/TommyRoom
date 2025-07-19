﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TommyRoom.Api.Data;
using TommyRoom.Shared.DTOs;
using TommyRoom.Shared.Entities;

namespace TommyRoom.Api.Helpers
{
    public class UserHelper(DataContext datacontext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : IUserHelper
    {
        private readonly DataContext _datacontext = datacontext;
        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SignInManager<User> _signInManager = signInManager;

        public async Task LogoutAsync() => await _signInManager.SignOutAsync();
        public async Task AddUserToRoleAsync(User user, string roleName) => await _userManager.AddToRoleAsync(user, roleName);
        public async Task<bool> IsUserInRoleAsync(User user, string roleName) => await _userManager.IsInRoleAsync(user, roleName);
        public async Task<SignInResult> LoginAsync(LoginDTO model) => await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, false, false);
        public async Task<IdentityResult> UpdateUserAsync(User user) => await _userManager.UpdateAsync(user);
        public async Task<IdentityResult> AddUserAsync(User user, string password) => await _userManager.CreateAsync(user, password);
        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword) => await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists) await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
        }

        public async Task<User> GetUserAsync(string email)
        {
            User? user = await _datacontext.Users.Include(u => u.Bookings).FirstOrDefaultAsync(u => u.Email == email);
            return user!;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            User? user = await _datacontext.Users.Include(u => u.Bookings).FirstOrDefaultAsync(u => u.Id == userId.ToString());
            return user!;
        }
    }
}
