namespace SmartHome.BusinessLogic.Args;

public class CreateBasicDeviceArgs
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? ModelNumber { get; set; }

    public List<string>? Photos { get; set; }

    public string? DeviceTypeName { get; set; }
}
