using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Args;
public class CreateHomeArgs
{
    public string? Address { get; set; }

    public string? DoorNumber { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public int? MemberCount { get; set; }

    public string? Name { get; set; }

    public User? Owner { get; set; }
}
