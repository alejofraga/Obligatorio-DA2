namespace SmartHome.WebApi.Requests;

public class CreateHomeOwnerRequest
{
    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string? Lastname { get; set; }

    public string? ProfilePicturePath { get; set; }
}
