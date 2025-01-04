namespace SmartHome.BusinessLogic.Args;
public class GetUsersArgs
{
    public string? Role { get; set; }

    public string? FullName { get; set; }

    public int Offset { get; set; }

    public int Limit { get; set; }
}
