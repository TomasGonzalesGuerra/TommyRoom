using TommyRoom.Shared.Enums;

namespace TommyRoom.Shared.Entities;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public ServiceChargeType ChargeType { get; set; } = ServiceChargeType.OneTime;

    public ICollection<ReservationService> ReservationServices { get; set; } = [];
}