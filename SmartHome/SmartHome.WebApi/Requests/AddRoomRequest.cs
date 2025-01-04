namespace SmartHome.WebApi.Requests;

public class AddRoomRequest
{
    public string Name { get; set; } = null!;

    public List<Guid> HardwareIds { get; set; } = null!;
}
