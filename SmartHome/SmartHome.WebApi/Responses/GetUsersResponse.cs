using SmartHome.BusinessLogic.Users;

namespace SmartHome.WebApi.Responses;

public readonly struct GetUsersResponse(User user)
{
    public readonly string Name { get; init; } = user.Name!;

    public readonly string Email { get; init; } = user.Email!;

    public readonly string Lastname { get; init; } = user.Lastname!;

    public readonly string Fullname { get; init; } = user.Name + " " + user.Lastname;

    public readonly List<string> Roles { get; init; } = user.Roles.Select(r => r.Name).ToList();

    public readonly string AccountCreationDate { get; init; } = user.AccountCreation.ToString("d");
}
