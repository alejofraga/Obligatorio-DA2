using System.ComponentModel.DataAnnotations;
using SmartHome.BusinessLogic.Devices;

namespace SmartHome.BusinessLogic.Homes;

public class Hardware
{
    [Key]
    public Guid Id { get; private init; } = Guid.NewGuid();
    public required string DeviceModelNumber { get; set; }
    public Room? Room { get; set; } = null!;
    public Guid? RoomId { get; set; }
    public Device? Device { get; set; }
    public bool Connected { get; set; } = true;
    public required Guid HomeId { get; set; }
    public Home? Home { get; set; }
    public string? Name { get; set; }
}
