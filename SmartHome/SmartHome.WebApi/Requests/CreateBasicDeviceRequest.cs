namespace SmartHome.WebApi.Requests;

public class CreateBasicDeviceRequest
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? ModelNumber { get; set; }

    public List<string>? Photos { get; set; }
}
