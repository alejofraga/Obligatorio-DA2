using FluentAssertions;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.DataLayer.Test;

[TestClass]
public class UserRepository_Test
{
    private SmartHomeDbContext _context = DbContextBuilder.BuildSmartHomeDbContext();
    private UserRepository _userRepository = null!;
    private RoleRepository _roleRepository = null!;
    private Repository<SystemPermission> _systemPermissionRepository = null!;
    [TestInitialize]
    public void Setup()
    {
        _context = DbContextBuilder.BuildSmartHomeDbContext();
        _userRepository = new UserRepository(_context);
        _roleRepository = new RoleRepository(_context);
        _systemPermissionRepository = new Repository<SystemPermission>(_context);
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void UpdateUser_WhenUserAndRoleExists_ShouldUpdateUser()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var roleName = "ADMIN";
        var role = new Role() { Name = roleName };
        _roleRepository.Add(role);
        _userRepository.AddRole(user, role);

        var userRoles = _userRepository.GetRoles(user.Email!);

        userRoles.Should().Contain(role);
    }

    [TestMethod]
    public void GetAll_WhenUsersExists_ShouldGetAllUsers()
    {
        var user = GetValidUser();
        _userRepository.Add(user);

        var users = _userRepository.GetAll();

        users.Should().Contain(user);
    }

    [TestMethod]
    public void GetOrDefault_WhenUserExists_ShouldGetUser()
    {
        var user = GetValidUser();
        _userRepository.Add(user);

        var userFromDb = _userRepository.GetOrDefault(u => u.Email == user.Email);

        userFromDb.Should().BeEquivalentTo(user);
    }

    [TestMethod]
    public void GetUserSystemPermissions_WhenUserExists_ShouldGetUserSystemPermissions()
    {
        var user = GetValidUser();
        var adminRole = new Role() { Name = "ADMIN" };
        var createPermission = new SystemPermission() { Name = "CREATE" };
        var readPermission = new SystemPermission() { Name = "READ" };
        _systemPermissionRepository.Add(createPermission);
        _systemPermissionRepository.Add(readPermission);
        _roleRepository.Add(adminRole);
        _roleRepository.AddPermissionToRole(adminRole, createPermission);
        _roleRepository.AddPermissionToRole(adminRole, readPermission);
        _userRepository.Add(user);
        _userRepository.AddRole(user, adminRole);

        var userSystemPermissions = _userRepository.GetSystemPermissions(user.Email!);

        userSystemPermissions.Should().HaveCount(2);
        userSystemPermissions.First().Should().BeEquivalentTo(_roleRepository.GetRolePermissions(adminRole).First());
        userSystemPermissions[1].Should().BeEquivalentTo(_roleRepository.GetRolePermissions(adminRole)[1]);
    }

    [TestMethod]
    public void UserHasPermission_WhenUserHasPermission_ShouldReturnTrue()
    {
        var user = GetValidUser();
        var adminRole = new Role() { Name = "ADMIN" };
        var createPermission = new SystemPermission() { Name = "CREATE" };
        var readPermission = new SystemPermission() { Name = "READ" };
        _systemPermissionRepository.Add(createPermission);
        _systemPermissionRepository.Add(readPermission);
        _roleRepository.Add(adminRole);
        _roleRepository.AddPermissionToRole(adminRole, createPermission);
        _roleRepository.AddPermissionToRole(adminRole, readPermission);
        _userRepository.Add(user);
        _userRepository.AddRole(user, adminRole);

        var hasPermissionCreate = _userRepository.UserHasPermission(user, "CREATE");
        var hasPermissionRead = _userRepository.UserHasPermission(user, "READ");

        hasPermissionCreate.Should().BeTrue();
        hasPermissionRead.Should().BeTrue();
    }

    [TestMethod]
    public void GetUsers_WhenIsCalled_ShouldReturnUsersFiltered()
    {
        var firstUser = new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456"
        };
        var secondUser = new User()
        {
            Email = "messi@gmail.com",
            Name = "Lionel",
            Lastname = "Messi",
            Password = "#Adf123456"
        };
        _userRepository.Add(firstUser);
        _userRepository.Add(secondUser);
        var getAllUsersArgs = new GetUsersArgs()
        {
            Offset = 1,
            Limit = 2
        };

        var obtainedUsers = _userRepository.GetUsersWithFilters(getAllUsersArgs);

        obtainedUsers.Should().BeEquivalentTo([firstUser, secondUser]);
    }

    [TestMethod]
    public void GetUsers_WhenNoUsers_ShouldReturnEmptyList()
    {
        var getAllUsersArgs = new GetUsersArgs()
        {
            Offset = 1,
            Limit = 2
        };

        var obtainedUsers = _userRepository.GetUsersWithFilters(getAllUsersArgs);

        obtainedUsers.Should().Equal([]);
    }

    [TestMethod]
    public void GetUsers_WhenInfoIsOk_ShouldReturnFilteredUsersByOffsetAndLimit()
    {
        var firstUser = new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456"
        };
        var secondUser = new User()
        {
            Email = "messi@gmail.com",
            Name = "Lionel",
            Lastname = "Messi",
            Password = "#Adf123456"
        };
        _userRepository.Add(firstUser);
        _userRepository.Add(secondUser);

        var getAllUsersArgs = new GetUsersArgs()
        {
            Offset = 1, // La seedData ya incluye el usuario sa@smarthome.com
            Limit = 1
        };

        var obtainedUsers = _userRepository.GetUsersWithFilters(getAllUsersArgs);

        obtainedUsers.Should().Equal([firstUser]);
    }

    [TestMethod]
    public void GetUsersWithFilters_WhenRoleAndFullNameAreCorrect_ShouldReturnFilteredUsers()
    {
        var firstUser = new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456"
        };
        var secondUser = new User()
        {
            Email = "messi@gmail.com",
            Name = "Lionel",
            Lastname = "Messi",
            Password = "#Adf123456"
        };
        _userRepository.Add(firstUser);
        _userRepository.Add(secondUser);
        var expectedRole = new Role() { Name = "Administrator" };
        _context.Roles.Add(expectedRole);
        _userRepository.AddRole(secondUser, expectedRole);

        var getAllUsersArgs = new GetUsersArgs()
        {
            Offset = 0,
            Limit = 2,
            Role = expectedRole.Name,
            FullName = $"{secondUser.Name} {secondUser.Lastname}"
        };

        var obtainedUsers = _userRepository.GetUsersWithFilters(getAllUsersArgs);

        obtainedUsers.Should().Equal([secondUser]);
    }

    [TestMethod]
    public void GetUsersWithFilters_WhenRoleAndFullNameAreIncorrect_ShouldReturnEmptyList()
    {
        var firstUser = new User()
        {
            Email = "maticor@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456"
        };
        var secondUser = new User()
        {
            Email = "messi@gmail.com",
            Name = "Lionel",
            Lastname = "Messi",
            Password = "#Adf123456"
        };
        _userRepository.Add(firstUser);
        _userRepository.Add(secondUser);
        var expectedRole = new Role() { Name = "role" };
        _context.Roles.Add(expectedRole);
        _userRepository.AddRole(secondUser, expectedRole);

        var getAllUsersArgs = new GetUsersArgs()
        {
            Offset = 1,
            Limit = 2,
            Role = "Wrong role",
            FullName = "Wrong FullName"
        };

        var obtainedUsers = _userRepository.GetUsersWithFilters(getAllUsersArgs);

        obtainedUsers.Should().Equal([]);
    }

    [TestMethod]
    public void SetProfilePicture_WhenUserExists_ShouldUpdateProfilePicturePath()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var newPicturePath = "new/path/to/profile/picture.jpg";

        _userRepository.SetProfilePicture(user.Email!, newPicturePath);
        var updatedUser = _userRepository.GetOrDefault(u => u.Email == user.Email);

        updatedUser.Should().NotBeNull();
        updatedUser!.ProfilePicturePath.Should().Be(newPicturePath);
    }
    #region SampleData
    private User GetValidUser()
    {
        var expectedName = "Alejo";
        var expectedPassword = "#Adf123456";
        var expectedLastname = "Fraga";
        var expectedEmail = "alejofraga22v2@gmail.com";
        var expectedProfilePicturePath = "PATH";
        var user = new User()
        {
            Email = expectedEmail,
            Name = expectedName,
            Lastname = expectedLastname,
            Password = expectedPassword,
            ProfilePicturePath = expectedProfilePicturePath
        };
        return user;
    }
    #endregion
}
