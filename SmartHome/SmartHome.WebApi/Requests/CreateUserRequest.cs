namespace SmartHome.WebApi.Requests;

public class CreateUserRequest
{
    public string? Email { get; set; } = null!;

    public string? Password { get; set; } = null!;

    public string? Name { get; set; } = null!;

    public string? Lastname { get; set; } = null!;

    public string? Role { get; set; } = null!;
}
