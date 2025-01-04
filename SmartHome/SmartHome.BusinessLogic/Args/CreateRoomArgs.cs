namespace SmartHome.BusinessLogic.Args;

public class CreateRoomArgs
{
    public required string? Name { get; set; }

    public required List<Guid>? HardwareIds { get; set; }

    public required Guid HomeId { get; set; }
}
