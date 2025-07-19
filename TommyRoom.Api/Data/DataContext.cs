using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TommyRoom.Shared.Entities;

namespace TommyRoom.Api.Data;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Booking>()
            .HasOne(r => r.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Booking>()
            .HasOne(r => r.Room)
            .WithMany(s => s.Bookings)
            .HasForeignKey(r => r.RoomId);

        modelBuilder.Entity<Room>()
            .HasOne(r => r.User)
            .WithMany(s => s.Rooms)
            .HasForeignKey(r => r.UserId);
    }
}
