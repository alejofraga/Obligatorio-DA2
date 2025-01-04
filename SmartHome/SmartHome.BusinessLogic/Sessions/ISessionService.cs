using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Sessions;

public interface ISessionService
{
    Session GetOrCreateSession(string? email, string? password);
    User GetUserBySessionId(string? token);
}
