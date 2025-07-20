﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TommyRoom.Api.Data;
using TommyRoom.Api.Helpers;
using TommyRoom.Shared.DTOs;
using TommyRoom.Shared.Entities;
using TommyRoom.Shared.Enums;

namespace TommyRoom.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController(DataContext datacontext, IUserHelper userHelper, IConfiguration configuration, IFileStorage fileStorage) : ControllerBase
{
    private readonly DataContext _dataContext = datacontext;
    private readonly IUserHelper _userHelper = userHelper;
    private readonly IConfiguration _configuration = configuration;
    private readonly IFileStorage _fileStorage = fileStorage;
    private readonly string _container = "users";

    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser([FromBody] UserDTO model)
    {
        User user = model;

        if (!string.IsNullOrEmpty(model.Photo))
        {
            var photoUser = Convert.FromBase64String(model.Photo);
            model.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);
        }

        var result = await _userHelper.AddUserAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
            return Ok(BuildToken(user));
        }

        return BadRequest(result.Errors.FirstOrDefault());
    }

    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginDTO model)
    {
        var result = await _userHelper.LoginAsync(model);

        if (result.Succeeded)
        {
            var user = await _userHelper.GetUserAsync(model.Email!);
            return Ok(BuildToken(user));
        }

        return BadRequest("Email  o  contraseña  incorrectos.");
    }

    private TokenDTO BuildToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email!),
            new(ClaimTypes.Role, user.UserType.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new("FullName", user.FullName!.ToString()),
            new("Photo", user.Photo  ??  string.Empty),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddDays(30); var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return new TokenDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }


    [HttpGet("Profile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> GetProfile()
    {
        var user = await _userHelper.GetUserAsync(User.Identity!.Name!);
        if (user == null) return Unauthorized("User not authenticated.");
        return Ok(user);
    }

    [HttpGet("AllUsers")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        if (_dataContext.Users == null) return NotFound("Entidad NO Encontrada");
        //List<User> allUsers = await _dataContext.Users.Where(u => u.UserType == UserType.User).OrderBy(u => u.FullName).ToListAsync();
        List<User> allUsers = await _dataContext.Users.OrderBy(u => u.FullName).ToListAsync();
        if (allUsers == null) return NotFound("Usuarios NO Encontrados");
        return Ok(allUsers);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Put(User user)
    {
        try
        {
            if (!string.IsNullOrEmpty(user.Photo))
            {
                var photoUser = Convert.FromBase64String(user.Photo);
                user.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);
            }

            var currentUser = await _userHelper.GetUserAsync(user.Email!);
            if (currentUser == null) return NotFound();

            currentUser.FullName = user.FullName;
            currentUser.PhoneNumber = user.PhoneNumber;
            currentUser.Photo = !string.IsNullOrEmpty(user.Photo) && user.Photo != currentUser.Photo ? user.Photo : currentUser.Photo;

            var result = await _userHelper.UpdateUserAsync(currentUser);
            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()!.Description);
            return Ok(NoContent());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("ChangePassword")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> ChangePasswordAsync(ChangePasswordDTO model)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _userHelper.GetUserAsync(User.Identity!.Name!);

            if (user == null) return NotFound();
            var result = await _userHelper.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()!.Description);
            return Ok(NoContent());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
