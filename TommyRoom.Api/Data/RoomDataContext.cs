using TommyRoom.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TommyRoom.Api.Data;

public class RoomDataContext(DbContextOptions<RoomDataContext> options) : IdentityDbContext<User>(options)
{
}
