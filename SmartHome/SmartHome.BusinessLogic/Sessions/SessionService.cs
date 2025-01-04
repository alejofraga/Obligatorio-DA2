using SmartHome.BusinessLogic.Exceptions;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Sessions;

public class SessionService(ISessionRepository sessionRepository, IUserRepository userRepository) : ISessionService
{
    public bool IsUserAlreadyLogged(string email, string password)
    {
        return sessionRepository.Exist(s => s.UserEmail!.ToUpper() == email.ToUpper() && s.User.Password == password);
    }

    public Session GetOrCreateSession(string? email, string? password)
    {
        if (email == null || password == null)
        {
            throw new ArgumentException("Email and password are required to authenticate");
        }

        if (IsUserAlreadyLogged(email, password))
        {
            return sessionRepository.GetOrDefault(s => s.UserEmail!.ToUpper() == email.ToUpper());
        }

        var user = GetValidUserOrThrow(email, password);
        var newSession = new Session(user);
        sessionRepository.Add(newSession);

        return newSession;
    }

    public User GetValidUserOrThrow(string email, string password)
    {
        var user = userRepository.GetOrDefault(u => u.Email!.ToUpper() == email.ToUpper());

        if (user == null || user.Password != password)
        {
            throw new ArgumentException("Failed to authenticate");
        }

        return user;
    }

    public User GetUserBySessionId(string? token)
    {
        if (token == null)
        {
            throw new UnauthorizedAccessException("Invalid authentication token");
        }

        var session = sessionRepository.GetOrDefault(s => s.SessionId.ToString() == token);

        if (session == null)
        {
            throw new NotFoundException("Session not found");
        }

        return session.User;
    }
}
