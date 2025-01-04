using SmartHome.BusinessLogic.Users;

namespace SmartHome.WebApi.Responses;

public readonly struct GetRolesReponse(List<Role> roles)
{
    public readonly List<string> Roles { get; init; } = roles.Select(r => r.Name).ToList();
}
