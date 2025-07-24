namespace TommyRoom.Shared.DTOs;

public class CartItemDTO
{
    public int RoomId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Photo { get; set; }
    public int Quantity { get; set; }
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
}
