using System.ComponentModel.DataAnnotations;

namespace SmartHome.BusinessLogic.Users;

public class SystemPermission
{
    [Key]
    public required string Name { get; set; } = null!;
    public List<Role> Roles { get; set; } = [];
}
