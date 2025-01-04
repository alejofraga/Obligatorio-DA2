using FluentAssertions;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Test.Users;

[TestClass]
public class Role_Test
{
    [TestMethod]
    public void Create_WhenInfoIsCorrect_ShouldCreateRole()
    {
        var expectedName = "Admin";
        var expectedUsers = new List<User>();
        var expectedSystemPermissions = new List<SystemPermission>();

        var role = new Role()
        {
            Name = expectedName,
            Users = expectedUsers,
            SystemPermissions = expectedSystemPermissions,
        };

        role.Should().NotBeNull();
        role.Name.Should().Be(expectedName);
        role.Users.Should().BeEquivalentTo(expectedUsers);
        role.SystemPermissions.Should().BeEquivalentTo(expectedSystemPermissions);
    }
}
