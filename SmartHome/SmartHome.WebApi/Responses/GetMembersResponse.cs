using SmartHome.BusinessLogic.Homes;

namespace SmartHome.WebApi.Responses;

public readonly struct GetMembersResponse(Member member)
{
    public readonly string Name { get; init; } = member.User.Name!;

    public readonly string Lastname { get; init; } = member.User.Lastname!;

    public readonly string Email { get; init; } = member.User.Email!;

    public readonly string ProfilePicturePath { get; init; } = member.User.ProfilePicturePath!;

    public readonly List<string> HomePermissions { get; init; } = member.HomePermissions.Select(permission => permission.Name).ToList()!;

    public readonly bool ReceiveNotifications { get; init; } = member.HomePermissions.Select(permission => permission.Name).ToList()
                                                                    .Contains(nameof(ValidHomePermissions.ReceiveNotifications));

    public readonly Guid Id { get; init; } = member.Id;
}
