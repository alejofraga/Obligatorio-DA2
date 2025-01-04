using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Sessions;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Test;

[TestClass]
public class AuthenticationController_Test
{
    private static readonly Mock<ISessionService> _sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict);
    private AuthenticationController _authController = new AuthenticationController(_sessionServiceMock.Object);

    [TestInitialize]
    public void OnInitialize()
    {
        _authController = new AuthenticationController(_sessionServiceMock.Object);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _sessionServiceMock.VerifyAll();
    }

    [TestMethod]
    public void Login_WhenCredentialsAreValid_ShouldReturnToken()
    {
        var loginRequest = new LoginRequest { Email = "user@gmail.com", Password = "pa$s32Word" };
        var user = new User() { Email = loginRequest.Email, Password = loginRequest.Password, Name = "alejo", Lastname = "fraga" };
        var newSession = new Session(user);

        _sessionServiceMock
            .Setup(ss => ss.GetOrCreateSession(loginRequest.Email, loginRequest.Password))
            .Returns(newSession);

        var act = _authController.Login(loginRequest);

        var result = act as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = (LoginResponse)result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        responseValue.Token.Should().Be(newSession.SessionId.ToString());
    }
}
