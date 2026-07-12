using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TommyRoom.Shared.Entities;

namespace TommyRoom.Api.Data;

public class RoomDataContext(DbContextOptions<RoomDataContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<RoomType> RoomTypes => Set<RoomType>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<ReservationRoom> ReservationRooms => Set<ReservationRoom>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ReservationService> ReservationServices => Set<ReservationService>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // OBLIGATORIO primero: configura las tablas AspNetUsers/AspNetRoles/etc.
        base.OnModelCreating(modelBuilder);

        // ---------------- User (columnas propias) ----------------
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.FullName).HasMaxLength(100).IsRequired();
        });

        // ---------------- RoomType ----------------
        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.Property(rt => rt.Name).HasMaxLength(100).IsRequired();
            entity.Property(rt => rt.Description).HasMaxLength(500);
            entity.Property(rt => rt.BaseRate).HasColumnType("decimal(10,2)");
        });

        // ---------------- Room ----------------
        modelBuilder.Entity<Room>(entity =>
        {
            entity.Property(r => r.RoomNumber).HasMaxLength(20).IsRequired();
            entity.Property(r => r.RoomStatus).HasConversion<string>().HasMaxLength(20);
            entity.HasIndex(r => r.RoomNumber).IsUnique();

            entity.HasOne(r => r.RoomType)
                  .WithMany(rt => rt.Rooms)
                  .HasForeignKey(r => r.RoomTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------------- Reservation ----------------
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.Property(r => r.ReservationStatus).HasConversion<string>().HasMaxLength(20);
            entity.Property(r => r.TotalAmount).HasColumnType("decimal(10,2)");
            entity.HasIndex(r => r.ReservationStatus);
            entity.HasIndex(r => new { r.CheckInDate, r.CheckOutDate });

            entity.HasOne(r => r.User)
                  .WithMany(u => u.Reservations)
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.ToTable(t => t.HasCheckConstraint(
                "CK_Reservations_Dates", "\"CheckOutDate\" > \"CheckInDate\""));
        });

        // ---------------- ReservationRoom ----------------
        modelBuilder.Entity<ReservationRoom>(entity =>
        {
            entity.Property(rr => rr.RatePerNight).HasColumnType("decimal(10,2)");
            entity.HasIndex(rr => new { rr.ReservationId, rr.RoomId }).IsUnique();

            entity.HasOne(rr => rr.Reservation)
                  .WithMany(r => r.ReservationRooms)
                  .HasForeignKey(rr => rr.ReservationId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rr => rr.Room)
                  .WithMany(r => r.ReservationRooms)
                  .HasForeignKey(rr => rr.RoomId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------------- Service ----------------
        modelBuilder.Entity<Service>(entity =>
        {
            entity.Property(s => s.Name).HasMaxLength(100).IsRequired();
            entity.Property(s => s.Description).HasMaxLength(500);
            entity.Property(s => s.UnitPrice).HasColumnType("decimal(10,2)");
            entity.Property(s => s.ChargeType).HasConversion<string>().HasMaxLength(20);
        });

        // ---------------- ReservationService ----------------
        modelBuilder.Entity<ReservationService>(entity =>
        {
            entity.Property(rs => rs.UnitPriceSnapshot).HasColumnType("decimal(10,2)");

            entity.HasOne(rs => rs.Reservation)
                  .WithMany(r => r.ReservationServices)
                  .HasForeignKey(rs => rs.ReservationId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rs => rs.Service)
                  .WithMany(s => s.ReservationServices)
                  .HasForeignKey(rs => rs.ServiceId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------------- Payment ----------------
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(p => p.Amount).HasColumnType("decimal(10,2)");
            entity.Property(p => p.PaymentStatus).HasConversion<string>().HasMaxLength(20);
            entity.Property(p => p.Type).HasConversion<string>().HasMaxLength(20);
            entity.Property(p => p.TransactionReference).HasMaxLength(100);
            entity.HasIndex(p => p.ReservationId);

            entity.HasOne(p => p.Reservation)
                  .WithMany(r => r.Payments)
                  .HasForeignKey(p => p.ReservationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
