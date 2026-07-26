using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using TommyRoom.Api.Data;
using TommyRoom.Api.Helpers;
using TommyRoom.Api.Services;
using TommyRoom.Shared.DTOs.Auth;
using TommyRoom.Shared.Entities;

namespace TommyRoom.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(RoomDataContext dataContext, IUserHelper userHelper, IConfiguration configuration, IFileStorage fileStorage, ITokenService tokenService) : ControllerBase
{
    private readonly string _container = "users";
    private readonly IUserHelper _userHelper = userHelper;
    private readonly IFileStorage _fileStorage = fileStorage;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IConfiguration _configuration = configuration;
    private readonly RoomDataContext _dataContext = dataContext;

    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser([FromBody] UserDTO model)
    {
        User user = model;

        if (!string.IsNullOrEmpty(model.Photo))
        {
            var photoUser = Convert.FromBase64String(model.Photo);
            model.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);
        }

        var result = await _userHelper.AddUserAsync(user, model.Password!);

        if (result.Succeeded)
        {
            await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
            var (token, expiresAt) = await _tokenService.GenerateTokenAsync(user);
            return Ok(await BuildTokenResponse(user, token, expiresAt));
        }

        return BadRequest(result.Errors.FirstOrDefault());
    }

    private async Task<TokenDTO> BuildTokenResponse(User user, string token, DateTime expiresAt)
    {
        return new TokenDTO
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
            }
        };
    }

    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginDTO model)
    {
        var result = await _userHelper.LoginAsync(model);

        if (result.Succeeded)
        {
            var user = await _userHelper.GetUserAsync(model.Email!);
            var (token, expiresAt) = await _tokenService.GenerateTokenAsync(user);
            return Ok(await BuildTokenResponse(user, token, expiresAt));
        }

        return BadRequest("Email o Contraseña Incorrectos.");
    }

    [HttpGet("Profile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> GetProfile()
    {
        var user = await _userHelper.GetUserAsync(User.Identity!.Name!);
        if (user == null) return Unauthorized("User not authenticated.");
        return Ok(user);
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
