namespace SmartHome.WebApi.Requests;

public class GrantHomePermissionRequest
{
    public string? MemberEmail { get; set; }

    public List<string>? HomePermissions { get; set; }
}
