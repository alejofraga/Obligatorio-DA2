using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Sessions;

public class Session()
{
    public User User { get; } = null!;
    public Guid SessionId { get; private init; } = Guid.NewGuid();
    public string? UserEmail { get; set; }

    public Session(User user)
        : this()
    {
        User = user;
    }
}
