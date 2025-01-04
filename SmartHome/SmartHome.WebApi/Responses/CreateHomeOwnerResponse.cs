using SmartHome.BusinessLogic.Users;

namespace SmartHome.WebApi.Responses;

public readonly struct CreateHomeOwnerResponse(User homeOwner)
{
    public readonly string Email { get; init; } = homeOwner.Email!;

    public readonly string Fullname { get; init; } = homeOwner!.Name + " " + homeOwner.Lastname;

    public readonly string ProfilePicturePath { get; init; } = homeOwner.ProfilePicturePath!;

    public readonly string AccountCreationDate { get; init; } = homeOwner.AccountCreation.ToString("d");
}
