using SmartHome.BusinessLogic.Users;

namespace SmartHome.WebApi.Responses;

public readonly struct CreateUserWithRoleResponse(User user)
{
    public readonly string Email { get; init; } = user.Email!;

    public readonly string Fullname { get; init; } = user!.Name + " " + user.Lastname;

    public readonly string ProfilePicturePath { get; init; } = user.ProfilePicturePath!;

    public readonly string AccountCreationDate { get; init; } = user.AccountCreation.ToString("d");
}
