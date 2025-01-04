using SmartHome.BusinessLogic.Args;

namespace SmartHome.BusinessLogic.Users;

public interface IUserService
{
    User GetByEmailOrThrow(string email, string? parameterName = null);
    void AddRole(string? email, string? roleName);
    List<User> GetUsersWithFilters(GetUsersArgs getUserArgs);
    List<Role> GetRoles(string email);
    void RemoveAdminIfNotHomeOwnerOrThrow(string? newUserEmail, User loggedUser);
    User CreateHomeOwner(CreateHomeOwnerArgs createHomeOwnerArgs);
    User CreateUserWithRole(CreateUserWithRoleArgs createUserWithRoleArgs);
    string GetProfilePicturePath(string email);
    void SetProfilePicturePath(string email, string path);
}
