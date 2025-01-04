using FluentAssertions;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Test.Users;

[TestClass]
public class User_Test
{
    [TestMethod]
    public void Create_WhenInfoIsCorrect_ShouldCreateUser()
    {
        var expectedName = "Alejo";
        var expectedPassword = "#Adf123456";
        var expectedLastname = "Fraga";
        var expectedEmail = "alejofraga22v2@gmail.com";
        var expectedMembers = new List<Member>();

        var user = new User()
        {
            Email = expectedEmail,
            Name = expectedName,
            Lastname = expectedLastname,
            Password = expectedPassword,
            Members = expectedMembers
        };

        user.Should().NotBeNull();
        user.Email.Should().Be(expectedEmail);
        user.Name.Should().Be(expectedName);
        user.Lastname.Should().Be(expectedLastname);
        user.Password.Should().Be(expectedPassword);
        user.Members.Should().BeEquivalentTo(expectedMembers);
    }

    [TestMethod]
    [DataRow("matias")]
    [DataRow("matias@")]
    [DataRow("matias!.com")]
    [DataRow("matias%com")]
    [DataRow("matias.com")]
    public void Create_WhenEmailFormatIsIncorrect_ShouldThrowFormatException(string incorrectEmail)
    {
        var name = "Matias";
        var password = "#Adf123456";
        var lastname = "Corvetto";

        var act = () => new User()
        {
            Email = incorrectEmail,
            Name = name,
            Lastname = lastname,
            Password = password
        };

        act.Should().Throw<FormatException>().WithMessage("Invalid email format");
    }

    [TestMethod]
    [DataRow("matiascor")]
    [DataRow("123@5678")]
    [DataRow("AAAAAAAAAAAAAAAAAAA")]
    [DataRow("matias123")]
    [DataRow("matias123@")]
    public void Create_WhenPasswordFormatIsIncorrect_ShouldThrowFormatException(string incorrectPassword)
    {
        var name = "Matias";
        var lastname = "Corvetto";
        var email = "maticor93@gmail.com";

        var act = () => new User()
        {
            Email = email,
            Name = name,
            Lastname = lastname,
            Password = incorrectPassword
        };

        act.Should().Throw<FormatException>().WithMessage("Password must contain at least one uppercase letter, one " +
            "lowercase letter, one number and one special character.");
    }

    [TestMethod]
    [DataRow("matias7")]
    [DataRow("mai@3A")]
    public void Create_WhenPasswordLengthIsIncorrect_ShouldThrowFormatException(string incorrectPassword)
    {
        var name = "Matias";
        var lastname = "Corvetto";
        var email = "maticor93@gmail.com";

        var act = () => new User()
        {
            Email = email,
            Name = name,
            Lastname = lastname,
            Password = incorrectPassword
        };

        act.Should().Throw<FormatException>().WithMessage("Password must be at least 8 characters long");
    }

    [TestMethod]
    [DataRow("matias", "corvetto", "maticor93@gmail.com", "     ")]
    [DataRow("matias", "corvetto", "maticor93@gmail.com", "")]
    [DataRow("matias", "corvetto", "maticor93@gmail.com", null)]
    [DataRow("matias", "corvetto", "     ", "#Adf123456")]
    [DataRow("matias", "corvetto", "", "#Adf123456")]
    [DataRow("matias", "corvetto", null, "#Adf123456")]
    [DataRow("matias", "     ", "maticor93@gmail.com", "#Adf123456")]
    [DataRow("matias", "", "maticor93@gmail.com", "#Adf123456")]
    [DataRow("matias", null, "maticor93@gmail.com", "#Adf123456")]
    [DataRow("     ", "corvetto", "maticor93@gmail.com", "#Adf123456")]
    [DataRow("", "corvetto", "maticor93@gmail.com", "#Adf123456")]
    [DataRow(null, "corvetto", "maticor93@gmail.com", "#Adf123456")]
    [DataRow("", "Doe", "", "")]
    [DataRow("", "", "test@example.com", "")]
    [DataRow("", "", "", "Password123!")]
    public void Create_WhenRequiredInfoIsEmpty_ShouldThrowArgumentNullException(string name, string lastname, string email, string password)
    {
        var act = () => new User()
        {
            Email = email,
            Name = name,
            Lastname = lastname,
            Password = password
        };

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void HasPermission_WhenUserHaveRoleWithPermission_ShouldReturnTrue()
    {
        var adminRole = new Role
        {
            Name = "Admin",
            SystemPermissions = [new SystemPermission { Name = "createUser" }]
        };

        var user = new User()
        {
            Email = "alejofraga22v2@gmail.com",
            Name = "Alejo",
            Lastname = "Fraga",
            Password = "#Adf123456",
            Roles = [adminRole]
        };

        user.HasPermission("createUser").Should().BeTrue();
    }

    [TestMethod]
    public void HasPermission_WhenUserNotHavePermission_ShouldReturnFalse()
    {
        var user = new User()
        {
            Email = "alejofraga22v2@gmail.com",
            Name = "Alejo",
            Lastname = "Fraga",
            Password = "#Adf123456"
        };

        user.HasPermission("createUser").Should().BeFalse();
    }

    [TestMethod]
    public void HasPermission_WhenPermissionIsNull_ShouldReturnFalse()
    {
        var user = new User()
        {
            Email = "alejofraga22v2@gmail.com",
            Name = "Alejo",
            Lastname = "Fraga",
            Password = "#Adf123456"
        };

        user.HasPermission(null).Should().BeFalse();
    }
}
