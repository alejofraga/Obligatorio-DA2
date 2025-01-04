using System.Linq.Expressions;
using SmartHome.BusinessLogic.Args;

namespace SmartHome.BusinessLogic.Users;

public interface IUserRepository : IRepository<User>
{
    void AddRole(User user, Role role);
    List<Role> GetRoles(string email);
    List<SystemPermission> GetSystemPermissions(string email);
    bool UserHasPermission(User user, string permissionName);
    new List<User> GetUsersWithFilters(GetUsersArgs getAllUsersArgs);
    new User? GetOrDefault(Expression<Func<User, bool>> predicate);
    void SetProfilePicture(string email, string profilePicturePath);
}
