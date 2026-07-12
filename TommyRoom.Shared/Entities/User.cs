using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TommyRoom.Shared.Enums;

namespace TommyRoom.Shared.Entities;

public class User : IdentityUser
{
    [Display(Name = "Nombre")]
    [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string? FullName { get; set; }
    [Display(Name = "Tipo de usuario")]
    public UserType UserType { get; set; }
    public string? Photo { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public ICollection<Reservation> Reservations { get; set; } = [];
    public ICollection<Room>? Rooms { get; set; }
}
