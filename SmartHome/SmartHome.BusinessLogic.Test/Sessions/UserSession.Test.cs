using FluentAssertions;
using SmartHome.BusinessLogic.Sessions;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Test.Sessions;

[TestClass]
public class UserSession_Test
{
    [TestMethod]
    public void Create_WhenInfoIsCorrect_ShouldCreateUserSession()
    {
        const string expectedEmail = "alejofraga22v2@gmail.com";
        const string expectedName = "Alejo";
        const string expectedLastname = "Fraga";
        const string expectedPassword = "#Adf123456";
        var user = new User()
        {
            Email = expectedEmail,
            Name = expectedName,
            Lastname = expectedLastname,
            Password = expectedPassword
        };

        var userSession = new Session(user);

        userSession.Should().NotBeNull();
        userSession.User.Should().NotBeNull();
        userSession.User.Email.Should().Be(expectedEmail);
        userSession.User.Name.Should().Be(expectedName);
        userSession.User.Lastname.Should().Be(expectedLastname);
        userSession.User.Password.Should().Be(expectedPassword);
        userSession.SessionId.Should().NotBe(Guid.Empty);
    }
}
