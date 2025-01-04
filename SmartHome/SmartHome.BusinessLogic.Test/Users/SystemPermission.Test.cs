using FluentAssertions;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Test.Users;

[TestClass]
public class SystemPermission_Test
{
    [TestMethod]
    public void Create_WhenInfoIsCorrect_ShouldCreateSystemPermission()
    {
        var expectedName = "CompanyOwnerAccountCreation";
        var expectedRoles = new List<Role>();

        var systemPermission = new SystemPermission()
        {
            Name = expectedName,
            Roles = expectedRoles,
        };

        systemPermission.Should().NotBeNull();
        systemPermission.Name.Should().Be(expectedName);
        systemPermission.Roles.Should().BeEquivalentTo(expectedRoles);
    }
}
