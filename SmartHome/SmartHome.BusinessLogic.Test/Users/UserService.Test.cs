using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Exceptions;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Test.Users;

[TestClass]
public class UserService_Test
{
    private static Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);

    private static Mock<IRepository<Role>> _roleRepositoryMock = new Mock<IRepository<Role>>(MockBehavior.Strict);

    private UserService _userService = new UserService(_userRepositoryMock.Object, _roleRepositoryMock.Object);

    [TestInitialize]
    public void OnInitialize()
    {
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _roleRepositoryMock = new Mock<IRepository<Role>>(MockBehavior.Strict);
        _userService = new UserService(_userRepositoryMock.Object, _roleRepositoryMock.Object);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _userRepositoryMock.VerifyAll();
        _roleRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void Add_WhenInfoIsCorrect_ShouldAddUser()
    {
        var user = GetValidUser();

        _userRepositoryMock
            .Setup(repo => repo.Exist(u => u.Email.ToUpper() == user.Email.ToUpper()))
            .Returns(false);
        _userRepositoryMock
            .Setup(repo => repo.Add(It.Is<User>(u => u == user)));

        var act = () => _userService.Add(user);

        act.Should().NotThrow();
    }

    [TestMethod]
    public void Add_WhenEmailIsDuplicated_ShouldThrowInvalidOperationException()
    {
        var user = GetValidUser();

        _userRepositoryMock
            .Setup(repo => repo.Exist(u => u.Email.ToUpper() == user.Email.ToUpper()))
            .Returns(true);

        var act = () => _userService.Add(user);

        act.Should().Throw<InvalidOperationException>().WithMessage("Email already in use");
    }

    [TestMethod]
    public void Add_WhenEmailIsDuplicated_ShouldThrowArgumentException()
    {
        const string userName = "Matias";
        const string userPassword = "#Adf123456";
        const string userLastname = "Corvetto";
        const string userEmail = "maticor93@gmail.com";
        var duplicatedUser = new User()
        {
            Email = userEmail,
            Name = userName,
            Lastname = userLastname,
            Password = userPassword
        };

        _userRepositoryMock
            .Setup(repo => repo.Exist(u => u.Email.ToUpper() == duplicatedUser.Email.ToUpper()))
            .Returns(() => true);

        var act = () => _userService.Add(duplicatedUser);

        act.Should().Throw<InvalidOperationException>().WithMessage("Email already in use");
    }

    [TestMethod]
    public void AddRole_WhenInfoIsCorrect_ShouldAddRoleToUser()
    {
        var user = GetValidUser();
        var userEmail = user.Email;
        var role = new Role() { Name = "Admin" };
        var roleName = role.Name;

        _userRepositoryMock
            .Setup(repo => repo.GetOrDefault(u => u.Email.ToUpper() == userEmail.ToUpper()))
            .Returns(user);
        _roleRepositoryMock
            .Setup(repo => repo.GetOrDefault(r => r.Name.ToUpper() == roleName.ToUpper()))
            .Returns(role);
        _userRepositoryMock
            .Setup(repo => repo.AddRole(It.Is<User>(u => u == user), It.Is<Role>(r => r == role)));

        var act = () => _userService.AddRole(user.Email!, role.Name);

        act.Should().NotThrow();
    }

    [TestMethod]
    public void AddRole_WhenRoleNotExists_ShouldThrowException()
    {
        var user = GetValidUser();
        var userEmail = user.Email;
        var role = new Role() { Name = "Admin" };
        var roleName = role.Name;

        _roleRepositoryMock
           .Setup(repo => repo.GetOrDefault(r => r.Name.ToUpper() == roleName.ToUpper()))
           .Returns(value: null);

        var act = () => _userService.AddRole(user.Email!, role.Name);

        act.Should().Throw<NotFoundException>().WithMessage("Role not found");
    }

    [TestMethod]
    public void AddRole_WhenUserNotExists_ShouldThrowNotFoundNotFoundException()
    {
        var user = GetValidUser();
        var userEmail = user.Email;
        var role = new Role() { Name = "Admin" };
        var roleName = role.Name;

        _roleRepositoryMock
            .Setup(repo => repo.GetOrDefault(r => r.Name.ToUpper() == roleName.ToUpper()))
            .Returns(value: null);

        var act = () => _userService.AddRole(user.Email!, role.Name);

        act.Should().Throw<NotFoundException>().WithMessage("Role not found");
    }

    [TestMethod]
    public void RemoveAdmin_WhenUserExists_ShouldRemoveUser()
    {
        var user = GetValidUser();
        var userEmail = user.Email;
        var adminRole = new Role() { Name = "Admin" };
        var loggedUser = new User { Name = "matias", Lastname = "corvetto", Email = "mati@cor.com", Password = "pa$s32Word", Roles = [adminRole] };

        _userRepositoryMock
            .Setup(repo => repo.GetOrDefault(u => u.Email.ToUpper() == userEmail.ToUpper()))
            .Returns(user);
        _userRepositoryMock
            .Setup(repo => repo.Remove(It.Is<User>(u => u == user)));
        _userRepositoryMock
            .Setup(repo => repo.GetRoles(It.IsAny<string>()))
            .Returns([adminRole]);

        var act = () => _userService.RemoveAdminIfNotHomeOwnerOrThrow(user.Email, loggedUser);

        act.Should().NotThrow();
    }

    [TestMethod]
    public void GetByEmailOrThrow_WhenEmailIsNull_ShouldThrowArgumentNullException()
    {
        var act = () => _userService.GetByEmailOrThrow(null);

        act.Should().Throw<ArgumentNullException>().WithMessage("User email cannot be empty");
    }

    [TestMethod]
    public void GetByEmailOrThrow_WhenUserInfoIsCorrect_ShouldGetUser()
    {
        var user = GetValidUser();
        var expectedEmail = user.Email;

        _userRepositoryMock.Setup(repo => repo.GetOrDefault(u => u.Email.ToUpper() == expectedEmail.ToUpper()))
            .Returns(() => user);

        var obtainedEmail = _userService.GetByEmailOrThrow(expectedEmail!);

        obtainedEmail.Should().NotBeNull();
        obtainedEmail!.Email.Should().Be(expectedEmail);
    }

    [TestMethod]
    public void GetByEmailOrThrow_WhenUserIsNull_ShouldThrowNotFoundException()
    {
        var expectedEmail = "seba@vega.com";

        _userRepositoryMock.Setup(repo => repo.GetOrDefault(u => u.Email.ToUpper() == expectedEmail.ToUpper()))
            .Returns(() => null);

        var act = () => _userService.GetByEmailOrThrow(expectedEmail);

        act.Should().Throw<NotFoundException>().WithMessage("User not found");
    }

    [TestMethod]
    public void AddRole_WhenRoleAndUserExists_ShouldAddRoleToUser()
    {
        const string expectedEmail = "alejofraga22v2@gmail.com";
        const string expectedRole = "Admin";
        var expectedUser = GetValidUser();
        var role = new Role() { Name = expectedRole };

        _userRepositoryMock
            .Setup(repo => repo.GetOrDefault(u => u.Email.ToUpper() == expectedEmail.ToUpper()))
            .Returns(expectedUser);
        _roleRepositoryMock
            .Setup(repo => repo.GetOrDefault(r => r.Name.ToUpper() == expectedRole.ToUpper()))
            .Returns(role);
        _userRepositoryMock
            .Setup(repo => repo.AddRole(expectedUser, role));

        _userService.AddRole(expectedEmail, expectedRole);
    }

    [TestMethod]
    public void GetRoles_WhenRoleAndUserExists_ShouldGetUserRoles()
    {
        const string expectedRoleAdmin = "Admin";
        const string expectedRoleHomeOwner = "HomeOwner";
        var adminRole = new Role() { Name = expectedRoleAdmin };
        var homeOwnerRole = new Role() { Name = expectedRoleHomeOwner };
        var user = GetValidUser();
        user.Roles = [adminRole, homeOwnerRole];
        const int expectedRoleCount = 2;

        _userRepositoryMock
            .Setup(repo => repo.GetRoles(user.Email!))
            .Returns(user.Roles);

        var userRoles = _userService.GetRoles(user.Email!);

        userRoles.Count.Should().Be(expectedRoleCount);
        userRoles.Any(r => r.Name == expectedRoleAdmin).Should().BeTrue();
        userRoles.Any(r => r.Name == expectedRoleHomeOwner).Should().BeTrue();
    }

    [TestMethod]
    public void GetUsers_WhenIsCalled_ShouldReturnAllUsers()
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
        var getAllUsersArgs = new GetUsersArgs()
        {
            Offset = 0,
            Limit = 25
        };

        _userRepositoryMock
            .Setup(repo => repo.GetUsersWithFilters(It.IsAny<GetUsersArgs>()))
            .Returns([firstUser, secondUser]);

        var obtainedUsers = _userService.GetUsersWithFilters(getAllUsersArgs);

        obtainedUsers.Should().Equal([firstUser, secondUser]);
    }

    [TestMethod]
    public void GetUsers_WhenNoUsers_ShouldReturnEmptyList()
    {
        var getAllUsersArgs = new GetUsersArgs()
        {
            Offset = 0,
            Limit = 25
        };

        _userRepositoryMock
            .Setup(repo => repo.GetUsersWithFilters(It.IsAny<GetUsersArgs>()))
            .Returns([]);

        var obtainedUsers = _userService.GetUsersWithFilters(getAllUsersArgs);

        obtainedUsers.Should().BeEmpty();
    }

    [TestMethod]
    public void GetUsers_WhenRoleAndFullNameAreCorrect_ShouldReturnFilteredUsers()
    {
        const int offset = 0;
        const int limit = 25;
        var expectedRole = new Role() { Name = "admin" };
        var expectedUser = new User()
        {
            Email = "test@test.com",
            Name = "John",
            Lastname = "Doe",
            Password = "1232524SASADaa!!"
        };
        var getAllUsersArgs = new GetUsersArgs()
        {
            Offset = 0,
            Limit = 25,
            Role = expectedRole.Name,
            FullName = $"{expectedUser.Name} {expectedUser.Lastname}"
        };

        _userRepositoryMock
            .Setup(repo => repo.GetUsersWithFilters(It.IsAny<GetUsersArgs>()))
            .Returns([expectedUser]);

        var result = _userService.GetUsersWithFilters(getAllUsersArgs);

        result.Should().BeEquivalentTo([expectedUser]);
        result[0].Name.Should().Be("John");
        result[0].Lastname.Should().Be("Doe");
    }

    [TestMethod]
    public void GetUsers_WhenRoleAndFullNameAreIncorrect_ShouldReturnEmptyList()
    {
        var expectedRole = new Role() { Name = "admin" };
        var expectedUser = new User()
        {
            Email = "test@test.com",
            Name = "John",
            Lastname = "Doe",
            Password = "1232524SASADaa!!",
            Roles = [expectedRole]
        };
        var getAllUsersArgs = new GetUsersArgs()
        {
            Offset = 0,
            Limit = 25,
            Role = "Wrong Role",
            FullName = "Wrong Name Wrong Lastname"
        };

        _userRepositoryMock
            .Setup(repo => repo.GetUsersWithFilters(It.IsAny<GetUsersArgs>()))
            .Returns([]);

        var result = _userService.GetUsersWithFilters(getAllUsersArgs);

        result.Should().BeEmpty();
    }

    [TestMethod]
    public void CreateHomeOwner_WhenInfoIsCorrect_ShouldCreateUserAndAddHomeOwnerRole()
    {
        var args = new CreateHomeOwnerArgs
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456",
            ProfilePicturePath = "/images/profile.jpg"
        };
        var user = GetValidUser();
        var userEmail = args.Email;
        var role = new Role() { Name = nameof(ValidUserRoles.HomeOwner) };
        var roleName = nameof(ValidUserRoles.HomeOwner);

        _userRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(false);
        _userRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<User>()));
        _userRepositoryMock
            .Setup(repo => repo.GetOrDefault(u => u.Email.ToUpper() == userEmail.ToUpper()))
            .Returns(user);
        _roleRepositoryMock
            .Setup(repo => repo.GetOrDefault(r => r.Name.ToUpper() == roleName.ToUpper()))
            .Returns(role);
        _userRepositoryMock
            .Setup(repo => repo.AddRole(user, role));

        var newHomeOwner = _userService.CreateHomeOwner(args);

        newHomeOwner.Should().NotBeNull();
        newHomeOwner.Email.Should().Be(args.Email);
        newHomeOwner.Name.Should().Be(args.Name);
        newHomeOwner.Lastname.Should().Be(args.Lastname);
        newHomeOwner.Password.Should().Be(args.Password);
        newHomeOwner.ProfilePicturePath.Should().Be(args.ProfilePicturePath);
    }

    [TestMethod]
    public void CreateHomeOwner_WhenProfilePicturePathIsNull_ShouldThrowArgumentNullException()
    {
        var args = new CreateHomeOwnerArgs
        {
            Name = "Matias",
            Lastname = "Corvetto",
            Email = "maticor93@gmail.com",
            Password = "#Adf123456",
            ProfilePicturePath = null
        };

        var act = () => _userService.CreateHomeOwner(args);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateUserWithRole_WhenInfoIsCorrect_ShouldCreateUserAndAddHomeOwnerRole()
    {
        var args = new CreateUserWithRoleArgs
        {
            Name = "Matias",
            Lastname = "Corvetto",
            Email = "maticor93@gmail.com",
            Password = "#Adf123456",
            Role = "Admin"
        };
        var user = GetValidUser();
        var userEmail = args.Email;
        var role = new Role() { Name = nameof(ValidUserRoles.Admin) };
        var roleName = args.Role;

        _userRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(false);
        _userRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<User>()));
        _userRepositoryMock
            .Setup(repo => repo.GetOrDefault(u => u.Email.ToUpper() == userEmail.ToUpper()))
            .Returns(user);
        _roleRepositoryMock
            .Setup(repo => repo.GetOrDefault(r => r.Name.ToUpper() == roleName.ToUpper()))
            .Returns(role);
        _userRepositoryMock
            .Setup(repo => repo.AddRole(user, role));

        var newHomeOwner = _userService.CreateUserWithRole(args);

        newHomeOwner.Should().NotBeNull();
        newHomeOwner.Email.Should().Be(args.Email);
        newHomeOwner.Name.Should().Be(args.Name);
        newHomeOwner.Lastname.Should().Be(args.Lastname);
        newHomeOwner.Password.Should().Be(args.Password);
    }

    [TestMethod]
    public void CreateUserWithRole_WhenRoleIsEmpty_ShouldThrowArgumentNullException()
    {
        var args = new CreateUserWithRoleArgs
        {
            Name = "Matias",
            Lastname = "Corvetto",
            Email = "maticor93@gmail.com",
            Password = "#Adf123456"
        };

        var act = () => _userService.CreateUserWithRole(args);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateUserWithRole_WhenRoleIsInvalid_ShouldThrowArgumentException()
    {
        var args = new CreateUserWithRoleArgs
        {
            Name = "Matias",
            Lastname = "Corvetto",
            Email = "maticor93@gmail.com",
            Password = "#Adf123456",
            Role = "InvalidRole"
        };

        var act = () => _userService.CreateUserWithRole(args);

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void GetProfilePicturePath_WhenUserExists_ShouldReturnProfilePicturePath()
    {
        var expectedEmail = "maticor93@gmail.com";
        var expectedProfilePicturePath = "/images/profile.jpg";
        var user = new User
        {
            Email = expectedEmail,
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456",
            ProfilePicturePath = expectedProfilePicturePath
        };

        _userRepositoryMock
            .Setup(repo => repo.GetOrDefault(u => u.Email.ToUpper() == expectedEmail.ToUpper()))
            .Returns(user);

        var profilePicturePath = _userService.GetProfilePicturePath(expectedEmail);

        profilePicturePath.Should().Be(expectedProfilePicturePath);
    }

    [TestMethod]
    public void SetProfilePicturePath_WhenInfoIsCorrect_ShouldSetProfilePicturePath()
    {
        var email = "test@example.com";
        var profilePicturePath = "/images/profile.jpg";

        _userRepositoryMock
            .Setup(repo => repo.SetProfilePicture(email, profilePicturePath));

        var act = () => _userService.SetProfilePicturePath(email, profilePicturePath);

        act.Should().NotThrow();
        _userRepositoryMock.Verify(repo => repo.SetProfilePicture(email, profilePicturePath), Times.Once);
    }
    #region SampleData
    private User GetValidUser()
    {
        return new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456"
        };
    }
    #endregion
}
