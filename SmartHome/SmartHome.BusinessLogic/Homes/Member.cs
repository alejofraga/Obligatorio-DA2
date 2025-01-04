using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Homes;

public sealed class Member
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    public required string UserEmail { get; set; }
    public required Guid HomeId { get; set; }
    public User User { get; set; } = null!;
    public Home Home { get; set; } = null!;
    public List<HomePermission> HomePermissions { get; set; } = [];
    public List<NotiAction> NotiActions { get; set; } = [];

    public bool HasHomePermission(string? permissionName)
    {
        return HomePermissions.Any(hp => hp.Name.ToUpper() == permissionName.ToUpper());
    }
}
