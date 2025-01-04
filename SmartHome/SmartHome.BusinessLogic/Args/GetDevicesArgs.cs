namespace SmartHome.BusinessLogic.Args;
public class GetDevicesArgs
{
    public int Offset { get; set; }

    public int Limit { get; set; }

    public string? ModelNumber { get; set; }

    public string? DeviceType { get; set; }

    public string? DeviceName { get; set; }

    public string? CompanyName { get; set; }
}
