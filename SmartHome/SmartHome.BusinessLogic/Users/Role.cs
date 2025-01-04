using System.ComponentModel.DataAnnotations;

namespace SmartHome.BusinessLogic.Users;

public class Role
{
    [Key]
    public required string Name { get; set; } = null!;
    public List<User> Users { get; set; } = null!;
    public List<SystemPermission> SystemPermissions { get; set; } = [];
}
