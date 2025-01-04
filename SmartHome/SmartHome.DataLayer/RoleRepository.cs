using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.DataLayer;

public class RoleRepository(SmartHomeDbContext context) : Repository<Role>(context)
{
    private readonly DbContext _context = context;
    private readonly DbSet<Role> _roles = context.Set<Role>();

    public void AddPermissionToRole(Role role, SystemPermission permission)
    {
        var userRole =
            new Dictionary<string, object> { { "SystemPermissionName", permission.Name }, { "RoleName", role.Name } };
        _context.Set<Dictionary<string, object>>("RoleSystemPermission").Add(userRole);
        _context.SaveChanges();
    }

    public List<SystemPermission> GetRolePermissions(Role role)
    {
        var rolePermissions = _roles.Include(r => r.SystemPermissions).FirstOrDefault(r => r.Name == role.Name);
        return rolePermissions.SystemPermissions.ToList();
    }
}
