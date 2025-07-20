namespace TommyRoom.Shared.DTOs
{
    public class CreatedRoomDTO
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? Photo { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
    }
}
