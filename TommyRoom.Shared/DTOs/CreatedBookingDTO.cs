namespace TommyRoom.Shared.DTOs
{
    public class CreatedBookingDTO
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int RoomId { get; set; }
    }
}
