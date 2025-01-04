namespace SmartHome.WebApi.Requests;

public class CreateHomeRequest
{
    public string? Address { get; set; }

    public string? DoorNumber { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public int? MemberCount { get; set; }

    public string? Name { get; set; }
}
