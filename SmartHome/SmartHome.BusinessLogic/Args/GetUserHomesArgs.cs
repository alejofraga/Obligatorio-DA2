using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Args;

public class GetUserHomesArgs()
{
    public int Limit { get; set; }

    public int Offset { get; set; }

    public User User { get; set; } = null!;
}
