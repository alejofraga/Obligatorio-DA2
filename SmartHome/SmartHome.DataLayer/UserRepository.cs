using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.DataLayer;

public class UserRepository(SmartHomeDbContext context) : Repository<User>(context), IUserRepository
{
    private readonly DbContext _context = context;
    private readonly DbSet<User> _users = context.Set<User>();

    public void AddRole(User user, Role role)
    {
        var userRole = new Dictionary<string, object> { { "RoleName", role.Name }, { "UserEmail", user.Email! } };
        _context.Set<Dictionary<string, object>>("UserRole").Add(userRole);
        _context.SaveChanges();
    }

    public List<Role> GetRoles(string email)
    {
        var user = _users.Include(u => u.Roles).FirstOrDefault(u => u.Email == email);
        return user.Roles.ToList();
    }

    public List<SystemPermission> GetSystemPermissions(string email)
    {
        var user = _users.Include(u => u.Roles).ThenInclude(r => r.SystemPermissions)
            .FirstOrDefault(u => u.Email == email);
        return user.Roles.SelectMany(r => r.SystemPermissions).ToList();
    }

    public bool UserHasPermission(User user, string permissionName)
    {
        var userPermissions = GetSystemPermissions(user.Email!);
        return userPermissions.Any(p => p.Name == permissionName);
    }

    public List<User> GetUsersWithFilters(GetUsersArgs getUsersArgs)
    {
        var query = _users.Include(u => u.Roles).AsQueryable();

        if (!string.IsNullOrEmpty(getUsersArgs.Role))
        {
            var upperRole = getUsersArgs.Role.ToUpper();
            query = query.Where(u => u.Roles.Any(r => r.Name.ToUpper().StartsWith(upperRole)));
        }

        if (!string.IsNullOrEmpty(getUsersArgs.FullName))
        {
            var upperFullName = getUsersArgs.FullName.ToUpper();
            query = query.Where(u =>
                (u.Name + " " + u.Lastname).ToUpper().StartsWith(upperFullName));
        }

        query = query.Skip(getUsersArgs.Offset)
            .Take(getUsersArgs.Limit);

        return query.ToList();
    }

    public override User? GetOrDefault(Expression<Func<User, bool>> predicate)
    {
        var user = _users.Include(u => u.Roles)
            .ThenInclude(r => r.SystemPermissions)
            .FirstOrDefault(predicate);
        return user;
    }

    public void SetProfilePicture(string email, string picturePath)
    {
        var user = _users.FirstOrDefault(u => u.Email == email);
        if (user != null)
        {
            user.ProfilePicturePath = picturePath;
            _context.SaveChanges();
        }
    }
}
