using SmartHome.BusinessLogic.Sessions;

namespace SmartHome.WebApi.Responses;

public readonly struct LoginResponse(Session session)
{
    public readonly string Token { get; init; } = session.SessionId.ToString();
}
