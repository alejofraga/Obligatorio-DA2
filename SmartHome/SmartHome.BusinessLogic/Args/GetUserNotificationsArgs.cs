using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Args;
public class GetUserNotificationsArgs
{
    public bool? Read { get; set; }

    public string? DateTime { get; set; }

    public string? DeviceType { get; set; }

    public User LoggedUser { get; set; } = null!;
}
