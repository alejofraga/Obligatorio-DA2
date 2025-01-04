using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Exceptions;

namespace SmartHome.BusinessLogic.Users;

public class UserService(IUserRepository userRepository, IRepository<Role> roleRepository) : IUserService
{
    public void Add(User newUser)
    {
        if (IsUserDuplicated(newUser))
        {
            throw new InvalidOperationException("Email already in use");
        }

        userRepository.Add(newUser);
    }

    public void RemoveAdminIfNotHomeOwnerOrThrow(string? newUserEmail, User loggedUser)
    {
        if (newUserEmail == null)
        {
            throw new ArgumentNullException("Email", "Email cannot be empty");
        }

        var user = GetByEmailOrThrow(newUserEmail, "Email");
        var userRoles = GetRoles(newUserEmail);
        var isHomeOwner = userRoles.Any(r => r.Name.ToUpper() == nameof(ValidUserRoles.HomeOwner).ToUpper());

        if (isHomeOwner)
        {
            throw new ForbiddenAccessException("Cannot delete an admin user that is a home owner");
        }

        if (newUserEmail.ToUpper() == loggedUser.Email.ToUpper())
        {
            throw new InvalidOperationException("Cannot delete your own account");
        }

        userRepository.Remove(user);
    }

    public User GetByEmailOrThrow(string email, string? parameterName = null)
    {
        if (email == null)
        {
            throw new ArgumentNullException(parameterName, "User email cannot be empty");
        }

        var user = userRepository.GetOrDefault(u => u.Email.ToUpper() == email.ToUpper());

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        return user;
    }

    public void AddRole(string? email, string? roleName)
    {
        var role = roleRepository.GetOrDefault(r => r.Name.ToUpper() == roleName.ToUpper());
        if (role == null)
        {
            throw new NotFoundException("Role not found");
        }

        var user = userRepository.GetOrDefault(u => u.Email.ToUpper() == email.ToUpper());
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        userRepository.AddRole(user, role);
    }

    public List<Role> GetRoles(string email)
    {
        return userRepository.GetRoles(email);
    }

    private bool IsUserDuplicated(User newUser)
    {
        return userRepository.Exist(u => u.Email.ToUpper() == newUser.Email.ToUpper());
    }

    public List<User> GetUsersWithFilters(GetUsersArgs getUserArgs)
    {
        var users = userRepository.GetUsersWithFilters(getUserArgs);
        return users;
    }

    public User CreateHomeOwner(CreateHomeOwnerArgs createHomeOwnerArgs)
    {
        var user = new User()
        {
            Name = createHomeOwnerArgs.Name,
            Lastname = createHomeOwnerArgs.Lastname,
            Email = createHomeOwnerArgs.Email,
            Password = createHomeOwnerArgs.Password,
            ProfilePicturePath = createHomeOwnerArgs.ProfilePicturePath
        };

        if (createHomeOwnerArgs.ProfilePicturePath == null)
        {
            throw new ArgumentNullException("ProfilePicturePath", "Profile picture path is required");
        }

        Add(user);
        AddRole(user.Email!, nameof(ValidUserRoles.HomeOwner));
        return user;
    }

    public User CreateUserWithRole(CreateUserWithRoleArgs createUserWithRoleArgs)
    {
        if (createUserWithRoleArgs.Role == null)
        {
            throw new ArgumentNullException("Role", "Role cannot be empty");
        }

        var isRoleValid = Enum.IsDefined(typeof(ValidUserRoles), createUserWithRoleArgs.Role);

        if (!isRoleValid)
        {
            throw new ArgumentException("Role name is not valid", "Role");
        }

        var isHomeOwnerRole = createUserWithRoleArgs.Role == nameof(ValidUserRoles.HomeOwner);

        if (isHomeOwnerRole)
        {
            throw new ForbiddenAccessException("Cannot create a home owner from here");
        }

        var user = new User()
        {
            Name = createUserWithRoleArgs.Name,
            Lastname = createUserWithRoleArgs.Lastname,
            Email = createUserWithRoleArgs.Email,
            Password = createUserWithRoleArgs.Password
        };

        Add(user);
        AddRole(createUserWithRoleArgs.Email!, createUserWithRoleArgs.Role!);
        return user;
    }

    public string GetProfilePicturePath(string email)
    {
        var user = GetByEmailOrThrow(email);
        return user.ProfilePicturePath;
    }

    public void SetProfilePicturePath(string email, string profilePicturePath)
    {
        userRepository.SetProfilePicture(email, profilePicturePath);
    }
}
